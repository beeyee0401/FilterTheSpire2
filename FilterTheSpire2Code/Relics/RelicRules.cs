using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.FilterTheSpire2Code.Relics;

public class RelicRules
{
    private static readonly HashSet<RelicOptions> CommonRelicPool =
    [
        RelicOptions.AmethystAubergine,
        RelicOptions.Anchor,
        RelicOptions.BagOfMarbles,
        RelicOptions.BagOfPreparation,
        RelicOptions.BloodVial,
        RelicOptions.BookOfFiveRings,
        RelicOptions.BronzeScales,
        RelicOptions.CentennialPuzzle,
        RelicOptions.FestivePopper,
        RelicOptions.Gorget,
        RelicOptions.HappyFlower,
        RelicOptions.JuzuBracelet,
        RelicOptions.Lantern,
        RelicOptions.MealTicket,
        RelicOptions.OddlySmoothStone,
        RelicOptions.Pendulum,
        RelicOptions.PotionBelt,
        RelicOptions.RedMask,
        RelicOptions.RegalPillow,
        RelicOptions.Strawberry,
        RelicOptions.StrikeDummy,
        RelicOptions.Vajra,
        RelicOptions.VenerableTeaSet,
        RelicOptions.WarPaint,
        RelicOptions.WhetStone,
    ];

    private static readonly Dictionary<CharacterOptions, Dictionary<RelicRarity, HashSet<RelicOptions>>> CharacterSpecificRelics = new()
    {
        {CharacterOptions.Ironclad, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.RedSkull] },
            // { RelicRarity.Uncommon, [RelicOptions.PaperPhrog] },
            // { RelicRarity.Rare, [RelicOptions.RedSkull] },
            // { RelicRarity.Shop, [RelicOptions.Brimstone] },
        }},
        {CharacterOptions.Silent, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.SneckoSkull] },
            // { RelicRarity.Uncommon, [RelicOptions.PaperPhrog] },
            // { RelicRarity.Rare, [RelicOptions.RedSkull] },
            // { RelicRarity.Shop, [RelicOptions.Brimstone] },
        }},
        {CharacterOptions.Regent, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.FencingManual] },
            // { RelicRarity.Uncommon, [RelicOptions.PaperPhrog] },
            // { RelicRarity.Rare, [RelicOptions.RedSkull] },
            // { RelicRarity.Shop, [RelicOptions.Brimstone] },
        }},
        {CharacterOptions.Necrobinder, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.BoneFlute] },
            // { RelicRarity.Uncommon, [RelicOptions.PaperPhrog] },
            // { RelicRarity.Rare, [RelicOptions.RedSkull] },
            // { RelicRarity.Shop, [RelicOptions.Brimstone] },
        }},
        {CharacterOptions.Defect, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.DataDisk] },
            // { RelicRarity.Uncommon, [RelicOptions.PaperPhrog] },
            // { RelicRarity.Rare, [RelicOptions.RedSkull] },
            // { RelicRarity.Shop, [RelicOptions.Brimstone] },
        }}
    };

    public static IEnumerable<RelicOptions> GetRelicPool(RelicRarity rarity)
    {
        var relicPool = CommonRelicPool.ToList();
        // if (FilterTheSpire2Config.Character != CharacterOptions.Any)
        // {
        //     relicPool.Add(CharacterSpecificRelics[FilterTheSpire2Config.Character][rarity]);
        // }
        return relicPool;
    }
}