using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Events;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Acts;

public static class ActGenerator
{
    [ThreadStatic] private static string? _cachedSeed;
    [ThreadStatic] private static AscensionLevel _cachedAscensionLevel;
    [ThreadStatic] private static List<ActDefinition>? _cachedRolledActs;
    
    public static List<ActDefinition> GetRolledActs(string seed, AscensionLevel ascensionLevel)
    {
        if (_cachedSeed == seed && _cachedAscensionLevel == ascensionLevel && _cachedRolledActs != null)
        {
            return _cachedRolledActs;
        }

        var result = RollActs(seed, ascensionLevel);
        _cachedSeed = seed;
        _cachedAscensionLevel = ascensionLevel;
        _cachedRolledActs = result;
        return result;
    }

    private static List<ActDefinition> RollActs(string seed, AscensionLevel ascensionLevel)
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

        foreach (var act in actList)
        {
            Roll(act, upfrontRng);
        }
        
        if (ascensionLevel < AscensionLevel.DoubleBoss)
        {
            return actList;
        }
        
        var lastAct = actList[^1];
        var remainingBosses = lastAct.Bosses
            .Where(b => b != lastAct.RolledBoss)
            .ToList();

        lastAct.RolledSecondBoss = upfrontRng.NextItem(remainingBosses);

        return actList;
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
    
    private static void Roll(ActDefinition act, Rng rng)
    {
        var events = act.ActEvents.AddRange(EventRules.SharedEvents).ToList();
        events.UnstableShuffle(rng);
        var firstEvent = events[1]; // event 0 is the Ancient! What a weird caveat!

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

        act.RolledAncient = ancient;
        act.RolledBoss = boss;
        act.RolledFirstEvent = firstEvent;
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