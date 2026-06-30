using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

public class LostCofferFilter(List<CardOptions> cardOptions, NeowRngConsumption? slot1Consumption = null)
    : BaseCardRewardFilter(CardRarityOddsType.RegularEncounter, cardOptions, 1,
        slot1Consumption: slot1Consumption)
{
    // 1 reward × 3 cards × (rarity + pick + NextFloat) = 9 Rewards calls
    public override NeowRngConsumption RngConsumption => new(9, 0, 0);
    protected override bool IsCharacterRequired => true;
    protected override Rng GetRewardRng(uint seed) => RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    protected override Rng? GetCardPoolRng(uint seed) => null;
    protected override List<List<CardDefinition>> GetRewardPools(Rng rng) =>
        Enumerable.Repeat(CardRules.EntireCardPools[FilterTheSpire2Config.Character].ToList(), CardsPerReward).ToList();
}