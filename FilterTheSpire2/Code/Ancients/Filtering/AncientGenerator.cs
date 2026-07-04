using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public static class AncientGenerator
{
    public static Ancient Generate(ActDefinition act, Rng rng)
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

        rng.NextInt(3); // boss roll

        return rng.NextItem(
            act.NativeAncients.Concat(act.SharedAncients));
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