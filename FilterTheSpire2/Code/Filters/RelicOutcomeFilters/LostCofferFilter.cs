using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class LostCofferFilter(
    List<CardOptions> cardOptions,
    PotionOptions potionOption,
    PriorRngConsumption? slot1Consumption = null)
    : BaseCardRewardFilter(CardRarityOddsType.RegularEncounter, cardOptions, 1,
        slot1Consumption: slot1Consumption)
{
    private readonly PriorRngConsumption? _slot1Consumption = slot1Consumption;
    protected override bool IsCharacterRequired => true;
    protected override Rng GetRewardRng(ulong seed) => RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    protected override Rng? GetCardPoolRng(ulong seed) => null;
    protected override List<List<CardDefinition>> GetRewardPools(Rng rng) =>
        Enumerable.Repeat(CardRules.EntireCardPools[FilterTheSpire2Config.Character].ToList(), CardsPerReward).ToList();

    public override bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        if (!base.IsSeedValid(request, seed))
        {
            return false;
        }

        if (potionOption == PotionOptions.Any)
        {
            return true;
        }

        var priorRewardsConsumption = (_slot1Consumption ?? PriorRngConsumption.None).RewardsRngSteps;
        var generated = PotionRewardSimulator.Generate(
            seed,
            new LostCofferPotionSource(priorRewardsConsumption));

        return generated[0] == potionOption;
    }
}