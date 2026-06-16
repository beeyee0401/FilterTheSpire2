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
    
    public KaleidoscopeFilter(List<CardOptions> cardOptions) : base(CardRarityOddsType.RegularEncounter, cardOptions, 2)
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
    protected override Rng GetRewardRng(uint seed) => RngHelper.GetPlayerRngType(seed, PlayerRngType.Rewards);
    protected override Rng? GetCardPoolRng(uint seed) => RngHelper.GetRunRngType(seed, RunRngType.Niche);
    protected override List<List<CardDefinition>> GetRewardPools(Rng rng)
    {
        var charList = _charListFiltered.ToList();
        charList.UnstableShuffle(rng);

        var selectedChars = charList.Take(CardsPerReward);
        
        return selectedChars
            .Select(character => CardRules.EntireCardPools[character].ToList())
            .ToList();
    }
}