using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Extensions;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class NeowsBonesFilter(
    ImmutableArray<NeowOptions> neowOptions, 
    CardOptions? curse,
    bool requireSequenceForTwoOptions = false) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var requested = neowOptions.Distinct().ToArray();
        var allPossibleOptions = AncientRules.NeowsBonesOptions.ToList();

        var numSeed = RngHelper.GetSeedHash(seed);
        var rng = RngHelper.GetPlayerRngType(numSeed, PlayerRngType.NonCombatGeneration);
        allPossibleOptions.UnstableShuffle(rng);
        var chosen = allPossibleOptions.Take(2).ToList();
        
        var availableCurses = CardRules.CursePool;
        // var nonCombatGenerationRng = RngHelper.GetPlayerRngType(numSeed, PlayerRngType.NonCombatGeneration);
        var fastForwardCounter = 0;
        if (chosen.Contains(NeowOptions.Kaleidoscope))
        {
            fastForwardCounter += RngHelper.RngCounters.KaleidoscopeNonCombatGenerationCounter;
        }
        
        rng.FastForwardCounter(fastForwardCounter);
        
        var chosenCurse = rng.NextItem(availableCurses.ToArray());

        var optionsMatch = requested.Length switch
        {
            0 => true,
            1 => chosen.Contains(requested[0]),
            2 when requireSequenceForTwoOptions =>
                requested.SequenceEqual(chosen),
            2 => requested.All(chosen.Contains),
            _ => true
        };

        return optionsMatch &&
               (curse == null || chosenCurse == curse);
    }
    
}