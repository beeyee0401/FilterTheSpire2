using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

public class NeowsBonesFilter(IReadOnlyList<NeowOptions> neowOptions, CardOptions? curse) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        if (neowOptions.Count > 1 && neowOptions[0] == neowOptions[1])
        {
            return true;
        }
        
        var allPossibleOptions = AncientRules.NeowsBonesOptions.ToList();

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
        var optionsMatch = neowOptions.Count switch
        {
            0 => true,
            1 => chosen.Contains(neowOptions[0]),
            _ => neowOptions.SequenceEqual(chosen)
        };
        return optionsMatch &&
               (curse == null || chosenCurse == curse);
    }
}