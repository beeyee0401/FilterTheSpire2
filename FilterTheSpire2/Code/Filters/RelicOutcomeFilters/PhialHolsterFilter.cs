using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Rngs;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

// TODO: maybe make this more generic to potion generation as a whole, since Lost coffer also gives a potion
public class PhialHolsterFilter(
    List<PotionOptions> potionsToMatch,
    RngConsumptionSteps? slot1Consumption = null) : INeowOutcomeFilter
{
    private const int GeneratedPotionCount = 2;

    public RngConsumptionSteps RngConsumptionSteps => new(0, 0, 0, GeneratedPotionCount * 2);

    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var requestedPotions = potionsToMatch
            .Where(p => p != PotionOptions.Any)
            .ToList();

        if (requestedPotions.Count == 0 || requestedPotions.Count > GeneratedPotionCount)
        {
            return true;
        }

        if (FilterTheSpire2Config.Character == CharacterOptions.Any)
        {
            return true;
        }

        var baseRng = RngHelper.GetBaseRng(seed);
        var rng = RngHelper.GetRunRngType(baseRng.Seed, RunRngType.CombatPotionGeneration);
        if (slot1Consumption != null)
        {
            rng.FastForwardCounter(slot1Consumption.CombatPotionGenerationRngSteps);
        }

        var pool = PotionRules.GetFullPoolForCharacter(FilterTheSpire2Config.Character).ToList();
        var generatedPotions = new List<PotionOptions>();

        for (var i = 0; i < GeneratedPotionCount; i++)
        {
            var roll = rng.NextFloat();
            var rarity =
                roll <= 0.10000000149011612 ? PotionRarity.Rare :
                roll <= 0.3499999940395355 ? PotionRarity.Uncommon :
                PotionRarity.Common;

            var candidates = pool.Where(p => p.Rarity == rarity).ToList();
            var picked = rng.NextItem(candidates);

            generatedPotions.Add(picked!.Potion);
            pool.Remove(picked);
        }

        var generatedToCheck = generatedPotions
            .Take(requestedPotions.Count)
            .ToHashSet();

        return requestedPotions.All(generatedToCheck.Contains);
    }
}