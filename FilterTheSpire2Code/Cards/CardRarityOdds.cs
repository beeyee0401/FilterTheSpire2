using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Cards;

public class CardRarityOdds(Rng rng, float initialOffset = -0.05f)
{
    private const float regularUncommonOdds = 0.37f;
    private const float eliteUncommonOdds = 0.4f;
    private const float bossCommonOdds = 0.0f;
    private const float bossUncommonOdds = 0.0f;
    private const float bossRareOdds = 1f;
    private const float shopUncommonOdds = 0.37f;
    private const float _baseRarityOffset = -0.05f;
    private const float _maxRarityOffset = 0.4f;

    private float CurrentValue { get; set; } = initialOffset;

    private static float GetValueForAscension(
        AscensionLevel level,
        float ascensionValue,
        float fallbackValue)
    {
        return (int)level > (int)AscensionLevel.Scarcity ? fallbackValue : ascensionValue;
    }

    private static float GetRegularCommonOdds(AscensionLevel level) => GetValueForAscension(level, 0.615f, 0.6f);
    private static float GetRarityGrowth(AscensionLevel level) => GetValueForAscension(level, 0.005f, 0.01f);
    private static float GetRegularRareOdds(AscensionLevel level) => GetValueForAscension(level, 0.0149f, 0.03f);
    private static float GetEliteCommonOdds(AscensionLevel level) => GetValueForAscension(level, 0.549f, 0.5f);
    private static float GetEliteRareOdds(AscensionLevel level) => GetValueForAscension(level, 0.05f, 0.1f);
    private static float GetShopCommonOdds(AscensionLevel level) => GetValueForAscension(level, 0.585f, 0.54f);
    private static float GetShopRareOdds(AscensionLevel level) => GetValueForAscension(level, 0.045f, 0.09f);

    // public CardRarity Roll(CardRarityOddsType type)
    // {
    //   CardRarity cardRarity = RollWithoutChangingFutureOdds(type, type == CardRarityOddsType.BossEncounter ? 0.0f : CurrentValue);
    //   if (cardRarity == CardRarity.Rare)
    //     CurrentValue = _baseRarityOffset;
    //   else
    //     CurrentValue = Math.Min(CurrentValue + GetRarityGrowth, _maxRarityOffset);
    //   return cardRarity;
    // }

    // public CardRarity RollWithoutChangingFutureOdds(CardRarityOddsType oddsType)
    // {
    //   return RollWithoutChangingFutureOdds(oddsType, CurrentValue);
    // }
    //
    // public CardRarity RollWithoutChangingFutureOdds(CardRarityOddsType type, float offset)
    // {
    //   float num1 = rng.NextFloat();
    //   float num2 = GetBaseOdds(type, CardRarity.Rare) + offset;
    //   Log.Info($"Card rarity: Rolled {num1}, need < {num2} for rare (offset = {offset})");
    //   if ((double)num1 < (double)num2)
    //     return CardRarity.Rare;
    //   return (double)num1 < (double)GetBaseOdds(type, CardRarity.Uncommon) + (double)num2
    //     ? CardRarity.Uncommon
    //     : CardRarity.Common;
    // }

    public CardRarity RollWithBaseOdds(AscensionLevel ascensionLevel, CardRarityOddsType type)
    {
        var num = rng.NextFloat();
        if ((double)num < GetBaseOdds(ascensionLevel, type, CardRarity.Rare))
        {
            return CardRarity.Rare;
        }

        return (double)num < GetBaseOdds(ascensionLevel, type, CardRarity.Uncommon)
            ? CardRarity.Uncommon
            : CardRarity.Common;
    }

    private float GetBaseOdds(AscensionLevel ascensionLevel, CardRarityOddsType type, CardRarity rarity)
    {
        return type switch
        {
            CardRarityOddsType.RegularEncounter => rarity switch
            {
                CardRarity.Common => GetRegularCommonOdds(ascensionLevel),
                CardRarity.Uncommon => regularUncommonOdds,
                CardRarity.Rare => GetRegularRareOdds(ascensionLevel),
                _ => throw new ArgumentOutOfRangeException(nameof(rarity))
            },
            CardRarityOddsType.EliteEncounter => rarity switch
            {
                CardRarity.Common => GetEliteCommonOdds(ascensionLevel),
                CardRarity.Uncommon => eliteUncommonOdds,
                CardRarity.Rare => GetEliteRareOdds(ascensionLevel),
                _ => throw new ArgumentOutOfRangeException(nameof(rarity))
            },
            CardRarityOddsType.BossEncounter => rarity switch
            {
                CardRarity.Common => bossCommonOdds,
                CardRarity.Uncommon => bossUncommonOdds,
                CardRarity.Rare => bossRareOdds,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity))
            },
            CardRarityOddsType.Shop => rarity switch
            {
                CardRarity.Common => GetShopCommonOdds(ascensionLevel),
                CardRarity.Uncommon => shopUncommonOdds,
                CardRarity.Rare => GetShopRareOdds(ascensionLevel),
                _ => throw new ArgumentOutOfRangeException(nameof(rarity))
            },
            CardRarityOddsType.Uniform => rarity switch
            {
                CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare => 0.33f,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity))
            },
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}