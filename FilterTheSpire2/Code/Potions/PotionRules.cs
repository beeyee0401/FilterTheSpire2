using System.Collections.Immutable;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using MegaCrit.Sts2.Core.Entities.Potions;

namespace FilterTheSpire2.Code.Potions;

public sealed record PotionDefinition(
    PotionOptions Potion,
    CharacterOptions Character, // CharacterOptions.Any = shared
    PotionRarity Rarity);

public static class PotionRules
{
    private static readonly ImmutableArray<PotionDefinition> AllPotionDefinitions =
    [
        #region Shared pool
        new(PotionOptions.AttackPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.BeetleJuice, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.BlessingOfTheForge, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.BlockPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.BottledPotential, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.Clarity, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.ColorlessPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.CureAll, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.DexterityPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.DistilledChaos, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.DropletOfPrecognition, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.Duplicator, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.EnergyPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.EntropicBrew, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.ExplosiveAmpoule, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.FairyInABottle, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.FirePotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.FlexPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.Fortifier, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.FruitJuice, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.FyshOil, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.GamblersBrew, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.GigantificationPotion, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.HeartOfIron, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.LiquidBronze, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.LiquidMemories, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.LuckyTonic, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.MazalethsGift, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.OrobicAcid, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.PotionOfBinding, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.PowderedDemise, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.PowerPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.RadiantTincture, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.RegenPotion, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.ShacklingPotion, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.ShipInABottle, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.SkillPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.SneckoOil, CharacterOptions.Any, PotionRarity.Rare),
        new(PotionOptions.SpeedPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.StableSerum, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.StrengthPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.SwiftPotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.TouchOfInsanity, CharacterOptions.Any, PotionRarity.Uncommon),
        new(PotionOptions.VulnerablePotion, CharacterOptions.Any, PotionRarity.Common),
        new(PotionOptions.WeakPotion, CharacterOptions.Any, PotionRarity.Common),
        #endregion

        #region Ironclad pool
        new(PotionOptions.BloodPotion, CharacterOptions.Ironclad, PotionRarity.Common),
        new(PotionOptions.SoldiersStew, CharacterOptions.Ironclad, PotionRarity.Rare),
        new(PotionOptions.Ashwater, CharacterOptions.Ironclad, PotionRarity.Uncommon),
        #endregion
        
        #region Silent pool
        new(PotionOptions.PoisonPotion, CharacterOptions.Silent, PotionRarity.Common),
        new(PotionOptions.GhostInAJar, CharacterOptions.Silent, PotionRarity.Rare),
        new(PotionOptions.CunningPotion, CharacterOptions.Silent, PotionRarity.Uncommon),
        #endregion
        
        #region Regent pool
        new(PotionOptions.StarPotion, CharacterOptions.Regent, PotionRarity.Common),
        new(PotionOptions.CosmicConcoction, CharacterOptions.Regent, PotionRarity.Rare),
        new(PotionOptions.KingsCourage, CharacterOptions.Regent, PotionRarity.Uncommon),
        #endregion
        
        #region Necrobinder pool
        new(PotionOptions.PotionOfDoom, CharacterOptions.Necrobinder, PotionRarity.Common),
        new(PotionOptions.PotOfGhouls, CharacterOptions.Necrobinder, PotionRarity.Rare),
        new(PotionOptions.BoneBrew, CharacterOptions.Necrobinder, PotionRarity.Uncommon),
        #endregion
        
        #region Defect pool
        new(PotionOptions.FocusPotion, CharacterOptions.Defect, PotionRarity.Common),
        new(PotionOptions.EssenceOfDarkness, CharacterOptions.Defect, PotionRarity.Rare),
        new(PotionOptions.PotionOfCapacity, CharacterOptions.Defect, PotionRarity.Uncommon),
        #endregion
    ];

    private static readonly ImmutableArray<PotionDefinition> SharedPotions =
        [..AllPotionDefinitions.Where(p => p.Character == CharacterOptions.Any)];

    private static readonly ImmutableDictionary<CharacterOptions, ImmutableArray<PotionDefinition>>
        CharacterSpecificPotions =
            AllPotionDefinitions
                .Where(p => p.Character != CharacterOptions.Any)
                .GroupBy(p => p.Character)
                .ToImmutableDictionary(g => g.Key, g => g.ToImmutableArray());

    private static readonly Dictionary<CharacterOptions, List<PotionDefinition>> CachedPools = new();

    
    /// <summary>
    /// The pool actually used for RNG matching/filtering. Always includes a character-specific set of
    /// potions — the real character's if selected, otherwise a placeholder's — so the pool length (and
    /// therefore index-based selection within it) stays consistent regardless of whether Character is
    /// known. Mirrors RelicRules.GetRelicPool.
    /// </summary>
    public static List<PotionDefinition> GetPotionPool()
    {
        var character = FilterTheSpire2Config.Character;

        if (!CachedPools.TryGetValue(character, out var pool))
        {
            pool = CreatePotionPool(includeCharacterPotions: true);
            CachedPools[character] = pool;
        }

        return pool;
    }

    /// <summary>
    /// The pool used to populate the config UI dropdown. Only includes character-specific potions when
    /// an actual character is selected, so users can't pick a character-specific potion while Character
    /// is Any (matching RelicRules.GetRelicDisplayPool).
    /// </summary>
    public static List<PotionDefinition> GetPotionDisplayPool()
    {
        return CreatePotionPool(includeCharacterPotions: FilterTheSpire2Config.Character != CharacterOptions.Any);
    }

    private static List<PotionDefinition> CreatePotionPool(bool includeCharacterPotions)
    {
        var pool = new List<PotionDefinition>();

        if (includeCharacterPotions)
        {
            var character = FilterTheSpire2Config.Character != CharacterOptions.Any
                ? FilterTheSpire2Config.Character
                : CharacterOptions.Ironclad; // placeholder to keep length

            if (CharacterSpecificPotions.TryGetValue(character, out var specificPotions))
            {
                pool.AddRange(specificPotions);
            }
        }

        pool.AddRange(SharedPotions);

        return pool;
    }
}