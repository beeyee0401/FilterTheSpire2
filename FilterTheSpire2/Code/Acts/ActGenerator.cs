using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Acts;

/// <summary>
/// Result of rolling all 3 acts for a seed: which ancient and which boss lands in each.
/// Index 0 = Act 1, index 1 = Act 2, index 2 = Act 3.
/// </summary>
public sealed record ActRollResult(
    IReadOnlyList<Ancient> Ancients,
    IReadOnlyList<BossOptions> Bosses);

public static class ActGenerator
{
    // Rolling all 3 acts walks a lot of RNG state and is shared by every Ancient/Boss filter for a
    // given seed. Cached per-thread, single-slot rather than a dictionary: SeedSearchWorker only
    // ever has one candidate seed "in flight" per thread, so this stays O(1) memory per thread even
    // across a billion+ seed search — the slot is simply overwritten as soon as the seed changes.
    [ThreadStatic] private static string? _cachedSeed;
    [ThreadStatic] private static ActRollResult? _cachedResult;

    public static ActRollResult GetActRollResult(string seed)
    {
        if (_cachedSeed == seed && _cachedResult != null)
        {
            Console.WriteLine("used cached result");
            return _cachedResult;
        }

        var result = RollActs(seed);
        _cachedSeed = seed;
        _cachedResult = result;
        return result;
    }

    private static ActRollResult RollActs(string seed)
    {
        var actSelectionRng = RngHelper.GetActSelectionRng(seed);
        var actList = GetRandomActDefinitions(actSelectionRng);

        var runRng = new RunRngSet(seed);
        var upfrontRng = runRng.UpFront;
        upfrontRng.FastForwardCounter(RngHelper.RngCounters.AncientCounter);

        var multiActAncients = AncientRules.MultiActAncientsAndRelics.Keys.ToList();
        multiActAncients.UnstableShuffle(upfrontRng);

        foreach (var act in actList.Skip(1))
        {
            var count = upfrontRng.NextInt(multiActAncients.Count + 1);

            var sharedAncientsForAct = multiActAncients
                .Take(count)
                .ToList();

            multiActAncients = multiActAncients
                .Except(sharedAncientsForAct)
                .ToList();

            act.SharedAncients.AddRange(sharedAncientsForAct);
        }

        var rolled = actList
            .Select(act => Generate(act, upfrontRng))
            .ToList();

        return new ActRollResult(
            [..rolled.Select(r => r.Ancient)],
            [..rolled.Select(r => r.Boss)]);
    }

    private static List<ActDefinition> GetRandomActDefinitions(Rng actSelectionRng)
    {
        return
        [
            actSelectionRng.NextItem([
                ActDefinition.Overgrowth.Clone(),
                ActDefinition.Underdocks.Clone()
            ])!,
            ActDefinition.Hive.Clone(),
            ActDefinition.Glory.Clone()
        ];
    }

    public static (Ancient Ancient, BossOptions Boss) Generate(ActDefinition act, Rng rng)
    {
        ConsumeShuffle(rng, act.EventCount);

        var normalEncounters = new List<SimpleEncounterDef>();

        GenerateEncounterPool(
            act.WeakEncounters,
            act.WeakEncounterCount,
            rng,
            normalEncounters);

        GenerateEncounterPool(
            act.RegularEncounters,
            act.RoomCount - act.WeakEncounterCount,
            rng,
            normalEncounters);

        GenerateEncounterPool(
            act.EliteEncounters,
            15,
            rng,
            []);

        var bossIndex = rng.NextInt(3);
        var boss = act.Bosses[bossIndex];

        var ancient = rng.NextItem(act.NativeAncients.Concat(act.SharedAncients));

        return (ancient, boss);
    }

    private static void GenerateEncounterPool(
        IReadOnlyList<SimpleEncounterDef> source,
        int count,
        Rng rng,
        List<SimpleEncounterDef> chosen)
    {
        var bag = new GrabBag<SimpleEncounterDef>();

        for (var i = 0; i < count; i++)
        {
            if (!bag.Any())
            {
                foreach (var encounter in source)
                {
                    bag.Add(encounter, 1.0);
                }
            }

            AddWithoutRepeatingTags(chosen, bag, rng);
        }
    }

    private static void ConsumeShuffle(Rng rng, int count)
    {
        while (count > 1)
        {
            count--;
            rng.NextInt(count + 1);
        }
    }

    private static void AddWithoutRepeatingTags(
        List<SimpleEncounterDef> chosen,
        GrabBag<SimpleEncounterDef> bag,
        Rng rng)
    {
        var last = chosen.LastOrDefault();

        var encounter = bag.GrabAndRemove(
                            rng,
                            e => last is null ||
                                 (!SharesTags(e, last) && e != last))
                        ?? bag.GrabAndRemove(rng);

        if (encounter is not null)
        {
            chosen.Add(encounter);
        }
    }

    private static bool SharesTags(SimpleEncounterDef current, SimpleEncounterDef previous)
    {
        return current.Tags.Overlaps(previous.Tags);
    }
}