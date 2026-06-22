using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

public class NeowsBonesFilter(HashSet<NeowOptions> neowOptions, CardOptions? curse) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var allPossibleOptions = new List<NeowOptions>()
        {
            NeowOptions.CursedPearl,
            NeowOptions.HeftyTablet,
            NeowOptions.LargeCapsule,
            NeowOptions.LeafyPoultice,
            NeowOptions.PrecariousShears,
            NeowOptions.SilkenTress,
            NeowOptions.SilverCrucible,
            
            NeowOptions.ArcaneScroll,
            NeowOptions.BoomingConch,
            NeowOptions.FishingRod,
            NeowOptions.GoldenPearl,
            NeowOptions.Kaleidoscope,
            NeowOptions.LeadPaperweight,
            NeowOptions.LostCoffer,
            NeowOptions.NeowsTorment,
            NeowOptions.NewLeaf,
            NeowOptions.PhialHolster,
            NeowOptions.PreciseScissors,
            NeowOptions.ScrollBoxes,
            NeowOptions.WingedBoots,
            
            NeowOptions.LavaRock,
            NeowOptions.NeowsTalisman,
            NeowOptions.NutritiousOyster,
            NeowOptions.Pomander,
            NeowOptions.SmallCapsule,
            NeowOptions.StoneHumidifier
        };

        var numSeed = (uint)StringHelper.GetDeterministicHashCode(seed);
        var rng = RngHelper.GetPlayerRngType(numSeed, PlayerRngType.Rewards);
        rng.Shuffle(allPossibleOptions);
        var chosen = allPossibleOptions.Take(2);
        
        var availableCurses = CardRules.CursePool;
        var nicheRng = RngHelper.GetRunRngType(numSeed, RunRngType.Niche);
        var fastForwardCounter = 0;
        if (neowOptions.Contains(NeowOptions.Kaleidoscope))
        {
            fastForwardCounter += RngHelper.RngCounters.KaleidoscopeNicheCounter;
        }
        
        if (neowOptions.Contains(NeowOptions.NewLeaf))
        {
            fastForwardCounter +=  RngHelper.RngCounters.NewLeafNicheCounter;
        }
        nicheRng.FastForwardCounter(fastForwardCounter);
        
        var chosenCurse = nicheRng.NextItem(availableCurses.ToArray());
        
        return (neowOptions.Count == 0 || neowOptions.All(chosen.Contains)) &&
               (curse == null || chosenCurse == curse);
    }
}