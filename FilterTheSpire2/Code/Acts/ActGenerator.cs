using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Acts;

/// <summary>
/// Result of rolling all 3 acts for a seed: which ancient and which boss lands in each, plus the
/// Act 3 second boss (only non-null when the ascension used to roll it was Double Boss or higher).
/// Index 0 = Act 1, index 1 = Act 2, index 2 = Act 3.
/// </summary>
public sealed record ActRollResult(
    IReadOnlyList<Ancient> Ancients,
    IReadOnlyList<BossOptions> Bosses,
    BossOptions? SecondBoss);

public static class ActGenerator
{
    // Cached per-thread, single-slot: SeedSearchWorker only has one candidate seed "in flight" per
    // thread at a time, so this stays O(1) memory per thread even across a billion+ seed search.
    // Ascension is part of the cache key too since it affects whether a second boss gets rolled —
    // in practice it's constant for a whole search, so this never actually causes a cache miss
    // mid-search, but it keeps the cache correct if that ever changes.
    [ThreadStatic] private static string? _cachedSeed;
    [ThreadStatic] private static AscensionLevel _cachedAscensionLevel;
    [ThreadStatic] private static ActRollResult? _cachedResult;

    public static ActRollResult GetActRollResult(string seed, AscensionLevel ascensionLevel)
    {
        if (_cachedSeed == seed && _cachedAscensionLevel == ascensionLevel && _cachedResult != null)
        {
            return _cachedResult;
        }

        var result = RollActs(seed, ascensionLevel);
        _cachedSeed = seed;
        _cachedAscensionLevel = ascensionLevel;
        _cachedResult = result;
        return result;
    }

    private static ActRollResult RollActs(string seed, AscensionLevel ascensionLevel)
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

        // Double Boss ascension rolls one additional boss, but only for the LAST act, and only
        // right after that act's normal generation — mirrors RunManager.GenerateRooms, where this
        // check lives inside the same per-act loop iteration, gated on `index == Acts.Count - 1`.
        BossOptions? secondBoss = null;
        if (ascensionLevel >= AscensionLevel.DoubleBoss)
        {
            var lastAct = actList[^1];
            var firstBossOfLastAct = rolled[^1].Boss;

            // Preserves roll order, matching the game's own exclusion logic:
            // AllBossEncounters.Where(e => e.Id != act.BossEncounter.Id)
            var remainingBosses = lastAct.Bosses
                .Where(b => b != firstBossOfLastAct)
                .ToList();

            secondBoss = upfrontRng.NextItem(remainingBosses);
        }

        return new ActRollResult(
            rolled.Select(r => r.Ancient).ToImmutableArray(),
            rolled.Select(r => r.Boss).ToImmutableArray(),
            secondBoss);
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