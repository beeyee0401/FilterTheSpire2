using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Neow : AbstractAncient
{
    public Neow()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Neow>().Id.Entry;
    }
    
    public override bool CheckOptions(uint seed, RelicModel relic)
    {
        var rng = RngHelper.GetEventRng(seed, Id!);

        var cursedOptions = new List<NeowOptions>()
        {
            NeowOptions.CursedPearl,
            NeowOptions.HeftyTablet,
            NeowOptions.LargeCapsule,
            NeowOptions.LeafyPoultice,
            NeowOptions.NeowsBones,
            NeowOptions.PrecariousShears,
            NeowOptions.SilverCrucible,
            NeowOptions.ScrollBoxes
        };

        var cursedOption = rng.NextItem(cursedOptions);
        var positiveOptions = new List<NeowOptions>()
        {
            NeowOptions.ArcaneScroll,
            NeowOptions.BoomingConch,
            NeowOptions.FishingRod,
            NeowOptions.GoldenPearl,
            NeowOptions.Kaleidoscope,
            NeowOptions.LeadPaperweight,
            NeowOptions.LostCoffer,
            // NeowOptions.MassiveScroll,
            NeowOptions.NeowsTorment,
            NeowOptions.NewLeaf,
            NeowOptions.PhialHolster,
            NeowOptions.PreciseScissors,
            NeowOptions.SilkenTress,
            NeowOptions.WingedBoots
        };

        switch (cursedOption)
        {
            case NeowOptions.CursedPearl:
                positiveOptions.Remove(NeowOptions.GoldenPearl);
                break;
            case NeowOptions.HeftyTablet:
                positiveOptions.Remove(NeowOptions.ArcaneScroll);
                break;
            case NeowOptions.LeafyPoultice:
                positiveOptions.Remove(NeowOptions.NewLeaf);
                break;
            case NeowOptions.PrecariousShears:
                positiveOptions.Remove(NeowOptions.PreciseScissors);
                break;
        }

        if (cursedOption != NeowOptions.LargeCapsule)
        {
            positiveOptions.Add(rng.NextBool() ? NeowOptions.LavaRock : NeowOptions.SmallCapsule);
        }

        positiveOptions.Add(rng.NextBool() ? NeowOptions.NutritiousOyster : NeowOptions.StoneHumidifier);
        positiveOptions.Add(rng.NextBool() ? NeowOptions.NeowsTalisman : NeowOptions.Pomander);

        positiveOptions.UnstableShuffle(rng);
        var finalOptions = positiveOptions.Take(2).ToList();
        finalOptions.Add(cursedOption);
        return finalOptions.Select(o => RelicModelFactory.GetRelicModel(o)!.Id).Contains(relic.Id);
    }
}