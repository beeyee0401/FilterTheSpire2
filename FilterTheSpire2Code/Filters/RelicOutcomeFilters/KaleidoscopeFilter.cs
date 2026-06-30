using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;

public class KaleidoscopeFilter : BaseCardRewardFilter
{
    private readonly List<CharacterOptions> _charListFiltered;

    // 2 rewards × 3 cards × 3 = 18 Rewards calls
    // 2 GetRewardPools calls, each shuffles 4 chars = 6 (N-1)*2 Niche calls
    public override NeowRngConsumption RngConsumption { get; }

    public KaleidoscopeFilter(List<CardOptions> cardOptions, NeowRngConsumption? slot1Consumption = null)
        : base(CardRarityOddsType.RegularEncounter, cardOptions, 2,
            slot1Consumption: slot1Consumption)
    {
        var charListSorted = new List<CharacterOptions>
        {
            CharacterOptions.Defect,
            CharacterOptions.Ironclad,
            CharacterOptions.Necrobinder,
            CharacterOptions.Regent,
            CharacterOptions.Silent
        };
        _charListFiltered = charListSorted.Except([FilterTheSpire2Config.Character]).ToList();
        
        // 3 per card in each card reward
        var rewardsSteps = CardsPerReward * 3 * 2;
        // (N-1) Niche calls per shuffle × 2 rewards
        var nicheSteps = (_charListFiltered.Count - 1) * 2;
        RngConsumption = new NeowRngConsumption(rewardsSteps, 0, nicheSteps);
    }

    protected override bool IsCharacterRequired => true;
    protected override Rng GetRewardRng(uint seed) => RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    protected override Rng? GetCardPoolRng(uint seed) => RngHelper.GetRunRngType(seed, RunRngType.Niche);
    protected override List<List<CardDefinition>> GetRewardPools(Rng rng)
    {
        var charList = _charListFiltered.ToList();
        charList.UnstableShuffle(rng);
        return charList.Take(CardsPerReward)
            .Select(character => CardRules.EntireCardPools[character].ToList())
            .ToList();
    }
}