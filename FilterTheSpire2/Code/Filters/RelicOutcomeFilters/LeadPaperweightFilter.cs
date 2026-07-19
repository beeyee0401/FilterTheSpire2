using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class LeadPaperweightFilter(List<CardOptions> cardOptions, PriorRngConsumption? slot1Consumption = null)
    : BaseCardRewardFilter(CardRarityOddsType.RegularEncounter, cardOptions, 1, 2,
        slot1Consumption: slot1Consumption)
{
    protected override bool IsCharacterRequired => false;
    protected override Rng GetRewardRng(ulong seed) => RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    protected override Rng? GetCardPoolRng(ulong seed) => null;
    protected override List<List<CardDefinition>> GetRewardPools(Rng rng)
    {
        var pool = CardRules.EntireCardPools[CharacterOptions.Any]
            .Where(c => c.Rarity.In(CardRarity.Uncommon, CardRarity.Rare)).ToList();
        return Enumerable.Repeat(pool, CardsPerReward).ToList();
    }
}