using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using CardRarityOdds = FilterTheSpire2.Code.Cards.CardRarityOdds;

namespace FilterTheSpire2.Code.Filters;

public readonly record struct RewardPool(IReadOnlyList<CardDefinition> Cards, HashSet<CardRarity> Rarities);

public abstract class BaseCardRewardFilter(
    CardRarityOddsType cardRarityOddsType,
    List<CardOptions> requestedCards,
    int cardRewardCount,
    int cardsPerReward = 3,
    PriorRngConsumption? slot1Consumption = null) : IFilter
{
    protected int CardsPerReward => cardsPerReward;
    protected abstract bool IsCharacterRequired { get; }
    protected abstract Rng GetRewardRng(ulong seed);
    protected abstract Rng? GetCardPoolRng(ulong seed);
    protected abstract List<RewardPool> GetRewardPools(Rng rng);

    public virtual bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var requestedCardList = requestedCards
            .Where(c => c != CardOptions.Any)
            .ToList();
        if (requestedCardList.Count == 0 || requestedCardList.Count > cardRewardCount)
        {
            return true;
        }

        if (IsCharacterRequired && FilterTheSpire2Config.Character == CharacterOptions.Any)
        {
            return true;
        }

        var seedLong = RngHelper.GetSeedHash(seed);
        var rng = GetRewardRng(seedLong);
        var poolRng = GetCardPoolRng(seedLong);

        if (slot1Consumption != null)
        {
            rng.FastForwardCounter(slot1Consumption.RewardsRngSteps);
            poolRng?.FastForwardCounter(slot1Consumption.NonCombatGenerationRngSteps);
        }

        var remaining = new Dictionary<CardOptions, int>();
        foreach (var card in requestedCardList)
        {
            remaining[card] = remaining.GetValueOrDefault(card) + 1;
        }

        var cardRarityOdds = new CardRarityOdds(rng);
        var matchedRewardIndices = new List<int>();
        var blacklist = new HashSet<CardDefinition>();
        var candidates = new List<CardDefinition>();

        for (var i = 0; i < cardRewardCount; i++)
        {
            blacklist.Clear();
            var rewardPools = GetRewardPools(poolRng ?? rng);

            for (var j = 0; j < cardsPerReward; j++)
            {
                var (cardPool, cardPoolRarities) = rewardPools[j];
                var cardRarity = RollForRarity(cardRarityOdds, cardRarityOddsType, CardCreationSource.Other,
                    cardPoolRarities, false, request.AscensionLevel);

                candidates.Clear();
                foreach (var c in cardPool)
                {
                    if (c.Rarity == cardRarity && !blacklist.Contains(c))
                    {
                        candidates.Add(c);
                    }
                }

                var card = rng.NextItem(candidates)!;

                if (remaining.TryGetValue(card.Card, out var count) && count > 0)
                {
                    remaining[card.Card]--;
                    matchedRewardIndices.Add(i);
                }

                blacklist.Add(card);
                rng.NextFloat();
            }
        }

        var allFound = remaining.Values.All(v => v == 0);
        var allInSeparateRewards =
            matchedRewardIndices.Count == requestedCardList.Count &&
            matchedRewardIndices.Distinct().Count() == matchedRewardIndices.Count;

        return allFound && allInSeparateRewards;
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
}