using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public class KaleidoscopeFilter : BaseCardRewardFilter
{
    private readonly List<CharacterOptions> _charListFiltered;

    public KaleidoscopeFilter(List<CardOptions> cardOptions, PriorRngConsumption? slot1Consumption = null)
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
    }

    protected override bool IsCharacterRequired => true;
    protected override Rng GetRewardRng(ulong seed) => RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    protected override Rng GetCardPoolRng(ulong seed) => RngHelper.GetRunRngType(seed, RunRngType.Niche);
    protected override List<List<CardDefinition>> GetRewardPools(Rng rng)
    {
        var charList = _charListFiltered.ToList();
        charList.UnstableShuffle(rng);
        return charList.Take(CardsPerReward)
            .Select(character => CardRules.EntireCardPools[character].ToList())
            .ToList();
    }
}