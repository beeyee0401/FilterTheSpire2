using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using CardRarityOdds = FilterTheSpire2.FilterTheSpire2Code.Cards.CardRarityOdds;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public abstract class BaseCardRewardFilter(
    CardRarityOddsType cardRarityOddsType, 
    List<CardOptions> requestedCards, 
    int cardRewardCount, 
    int cardsPerReward = 3) : IFilter
{
    protected abstract Rng GetRewardRng(uint seed);
    
    protected abstract List<CardDefinition> GetCardPool();
    
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var baseRng = new Rng((uint) StringHelper.GetDeterministicHashCode(seed));
        var rng = GetRewardRng(baseRng.Seed);
        var cardPool = GetCardPool();
        var remaining = requestedCards
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());
        var cardPoolRarities = cardPool.Select(c => c.Rarity).ToHashSet();
        var cardRarityOdds = new CardRarityOdds(rng);
        for (var i = 0; i < cardRewardCount; i++)
        {
            for (var j = 0; j < cardsPerReward; j++)
            {
                var cardRarity = RollForRarity(cardRarityOdds, 
                    cardRarityOddsType, 
                    CardCreationSource.Other, 
                    cardPoolRarities, 
                    false, 
                    request.AscensionLevel);
                var cardsWithRarity = cardPool.Where(c => c.Rarity == cardRarity).ToList();
                var card = rng.NextItem(cardsWithRarity);
                if (remaining.ContainsKey(card!.Card))
                {
                    remaining[card.Card]--;
                }

                if (remaining.Values.All(count => count == 0))
                {
                    return true;
                }

                rng.NextFloat();
            }
        }
        
        return false;
    }
    
    private CardRarity RollForRarity(
        CardRarityOdds cardRarityOdds,
        CardRarityOddsType rollMethod,
        CardCreationSource source,
        HashSet<CardRarity> allowedRarities,
        bool forceRarityOddsChange,
        AscensionLevel ascension)
    {
        // var useModifiedOdds =
        //     forceRarityOddsChange ||
        //     (source == CardCreationSource.Encounter &&
        //      (rollMethod == CardRarityOddsType.RegularEncounter ||
        //       rollMethod == CardRarityOddsType.EliteEncounter ||
        //       rollMethod == CardRarityOddsType.BossEncounter));
    
        // CardRarity rarity = useModifiedOdds
        //     ? Roll(rollMethod)
        //     : RollWithBaseOdds(rollMethod);
        var rarity = cardRarityOdds.RollWithBaseOdds(ascension, rollMethod);
    
        return GetNextAllowedRarity(
            rarity,
            allowedRarities.Contains);
    }
    
    private static CardRarity GetNextAllowedRarity(CardRarity rarity, Func<CardRarity, bool> isAllowed)
    {
        var start = rarity;
        while (!isAllowed(rarity) && rarity != CardRarity.None)
        {
            rarity = rarity.GetNextHighestRarityWithWrapping();
            if (rarity == start)
            {
                return CardRarity.None;
            }
        }

        return rarity;
    }
    
    // Lead Paperweight
    // private CardRarity RollWithBaseOdds(CardRarityOdds cardRarityOdds, CardRarityOddsType type, int ascension)
    // {
    //     cardRarityOdds.rol
    //     var num = rng.NextFloat();
    //     if ((double) num < (double) CardRarityOdds.GetBaseOdds(type, CardRarity.Rare))
    //         return CardRarity.Rare;
    //     return (double) num < (double) CardRarityOdds.GetBaseOdds(type, CardRarity.Uncommon) ? CardRarity.Uncommon : CardRarity.Common;
    // }
    
    // public CardRarity Roll(CardRarityOddsType type)
    // {
    //     CardRarity cardRarity = this.RollWithoutChangingFutureOdds(type, type == CardRarityOddsType.BossEncounter ? 0.0f : this.CurrentValue);
    //     if (cardRarity == CardRarity.Rare)
    //         this.CurrentValue = -0.05f;
    //     else
    //         this.CurrentValue = Math.Min(this.CurrentValue + this.RarityGrowth, 0.4f);
    //     return cardRarity;
    // }
}