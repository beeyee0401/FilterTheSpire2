using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.Code.Relics;

public static class RelicRules
{
    private static readonly Dictionary<CharacterOptions, Dictionary<RelicRarity, List<RelicOptions>>> CachedPools = new();
    
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

    private static readonly HashSet<RelicOptions> UncommonRelicPool =
    [
        RelicOptions.Akabeko,
        RelicOptions.BowlerHat,
        RelicOptions.Candelabra,
        RelicOptions.EternalFeather,
        RelicOptions.GremlinHorn,
        RelicOptions.HornCleat,
        RelicOptions.JossPaper,
        RelicOptions.Kusarigama,
        RelicOptions.LastingCandy,
        RelicOptions.LetterOpener,
        RelicOptions.LuckyFysh,
        RelicOptions.MercuryHourglass,
        RelicOptions.MiniatureCannon,
        RelicOptions.Nunchaku,
        RelicOptions.Orichalcum,
        RelicOptions.OrnamentalFan,
        RelicOptions.Pantograph,
        RelicOptions.ParryingShield,
        RelicOptions.Pear,
        RelicOptions.PenNib,
        RelicOptions.Permafrost,
        RelicOptions.PetrifiedToad,
        RelicOptions.Planisphere,
        RelicOptions.ReptileTrinket,
        RelicOptions.RippleBasin,
        RelicOptions.SparklingRouge,
        RelicOptions.StoneCracker,
        RelicOptions.TinyMailbox,
        RelicOptions.TuningFork,
        RelicOptions.Vambrace
    ];
    
    private static readonly HashSet<RelicOptions> RareRelicPool =
    [
        RelicOptions.ArtOfWar,
        RelicOptions.BeatingRemnant,
        RelicOptions.Bellows,
        RelicOptions.CaptainsWheel,
        RelicOptions.Chandelier,
        RelicOptions.CloakClasp,
        RelicOptions.FrozenEgg,
        RelicOptions.GamblingChip,
        RelicOptions.GamePiece,
        RelicOptions.Girya,
        RelicOptions.IceCream,
        RelicOptions.IntimidatingHelmet,
        RelicOptions.Kunai,
        RelicOptions.LizardTail,
        RelicOptions.Mango,
        RelicOptions.MeatOnTheBone,
        RelicOptions.MoltenEgg,
        RelicOptions.MummifiedHand,
        RelicOptions.OldCoin,
        RelicOptions.Pocketwatch,
        RelicOptions.PrayerWheel,
        RelicOptions.RainbowRing,
        RelicOptions.RazorTooth,
        RelicOptions.Shovel,
        RelicOptions.Shuriken,
        RelicOptions.StoneCalendar,
        RelicOptions.SturdyClamp,
        RelicOptions.TheCourier,
        RelicOptions.ToxicEgg,
        RelicOptions.TungstenRod,
        RelicOptions.UnceasingTop,
        RelicOptions.UnsettlingLamp,
        RelicOptions.VexingPuzzlebox,
        RelicOptions.WhiteBeastStatue,
        RelicOptions.WhiteStar,
    ];

    private static readonly HashSet<RelicOptions> ShopRelicPool =
    [
        RelicOptions.BeltBuckle,
        RelicOptions.Bread,
        RelicOptions.BurningSticks,
        RelicOptions.Cauldron,
        RelicOptions.ChemicalX,
        RelicOptions.DingyRug,
        RelicOptions.DollysMirror,
        RelicOptions.DragonFruit,
        RelicOptions.GhostSeed,
        RelicOptions.GnarledHammer,
        RelicOptions.Kifuda,
        RelicOptions.LavaLamp,
        RelicOptions.LeesWaffle,
        RelicOptions.MembershipCard,
        RelicOptions.MiniatureTent,
        RelicOptions.MysticLighter,
        RelicOptions.Orrery,
        RelicOptions.PunchDagger,
        RelicOptions.RingingTriangle,
        RelicOptions.RoyalStamp,
        RelicOptions.ScreamingFlagon,
        RelicOptions.SlingOfCourage,
        RelicOptions.TheAbacus,
        RelicOptions.Toolbox,
        RelicOptions.WingCharm,
    ];

    public static readonly Dictionary<CharacterOptions, Dictionary<RelicRarity, HashSet<RelicOptions>>> CharacterSpecificRelics = new()
    {
        {CharacterOptions.Ironclad, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.RedSkull] },
            { RelicRarity.Uncommon, [RelicOptions.PaperPhrog, RelicOptions.SelfFormingClay] },
            { RelicRarity.Rare, [RelicOptions.CharonsAshes, RelicOptions.DemonTongue, RelicOptions.RuinedHelmet] },
            { RelicRarity.Shop, [RelicOptions.Brimstone] },
        }},
        {CharacterOptions.Silent, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.SneckoSkull] },
            { RelicRarity.Uncommon, [RelicOptions.Tingsha, RelicOptions.TwistedFunnel] },
            { RelicRarity.Rare, [RelicOptions.HelicalDart, RelicOptions.PaperKrane, RelicOptions.ToughBandages] },
            { RelicRarity.Shop, [RelicOptions.NinjaScroll] },
        }},
        {CharacterOptions.Regent, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.FencingManual] },
            { RelicRarity.Uncommon, [RelicOptions.GalacticDust, RelicOptions.Regalite] },
            { RelicRarity.Rare, [RelicOptions.LunarPastry, RelicOptions.MiniRegent, RelicOptions.OrangeDough] },
            { RelicRarity.Shop, [RelicOptions.VitruvianMinion] },
        }},
        {CharacterOptions.Necrobinder, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.BoneFlute] },
            { RelicRarity.Uncommon, [RelicOptions.BookRepairKnife, RelicOptions.FuneraryMask] },
            { RelicRarity.Rare, [RelicOptions.BigHat, RelicOptions.Bookmark, RelicOptions.IvoryTile] },
            { RelicRarity.Shop, [RelicOptions.UndyingSigil] },
        }},
        {CharacterOptions.Defect, new Dictionary<RelicRarity, HashSet<RelicOptions>>()
        {
            { RelicRarity.Common, [RelicOptions.DataDisk] },
            { RelicRarity.Uncommon, [RelicOptions.GoldPlatedCables, RelicOptions.SymbioticVirus] },
            { RelicRarity.Rare, [RelicOptions.EmotionChip, RelicOptions.Metronome, RelicOptions.PowerCell] },
            { RelicRarity.Shop, [RelicOptions.RunicCapacitor] },
        }}
    };

    private static List<RelicOptions> CreateRelicPool(RelicRarity rarity, bool includeCharacterRelics)
    {
        var relicPool = GetBaseRelicPool(rarity).ToList();

        if (!includeCharacterRelics)
        {
            return relicPool;
        }

        relicPool.AddRange(FilterTheSpire2Config.Character != CharacterOptions.Any
            ? CharacterSpecificRelics[FilterTheSpire2Config.Character][rarity]
            // Even if the character is Any, we need to add relics to the pool so it's the correct length for RNG
            : CharacterSpecificRelics[CharacterOptions.Ironclad][rarity]);

        return relicPool;
    }

    public static IEnumerable<RelicOptions> GetRelicPool(RelicRarity rarity)
    {
        var character = FilterTheSpire2Config.Character;

        if (!CachedPools.TryGetValue(character, out var poolsByRarity))
        {
            poolsByRarity = new Dictionary<RelicRarity, List<RelicOptions>>();
            CachedPools[character] = poolsByRarity;
        }

        if (!poolsByRarity.TryGetValue(rarity, out var relicPool))
        {
            relicPool = CreateRelicPool(rarity, includeCharacterRelics: true);
            poolsByRarity[rarity] = relicPool;
        }

        return relicPool;
    }

    public static IEnumerable<RelicOptions> GetRelicDisplayPool(RelicRarity rarity)
    {
        return CreateRelicPool(rarity, includeCharacterRelics: FilterTheSpire2Config.Character != CharacterOptions.Any);
    }

    private static IEnumerable<RelicOptions> GetBaseRelicPool(RelicRarity rarity)
    {
        return rarity switch
        {
            RelicRarity.Common => CommonRelicPool,
            RelicRarity.Uncommon => UncommonRelicPool,
            RelicRarity.Rare => RareRelicPool,
            RelicRarity.Shop => ShopRelicPool,
            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
        };
    }
}