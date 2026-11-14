using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Helpers;

public static class RngHelper
{
    [ThreadStatic] private static string? _cachedSeedString;
    [ThreadStatic] private static ulong _cachedSeedHash;
    
    // These can change at any time between patches
    public static class RngCounters
    {
        /// <summary>
        /// One for each full card reward
        /// </summary>
        public const int KaleidoscopeNonCombatGenerationCounter = 2;
        public const int UncommonRelicPoolCounter = 6; // or 0
        public const int CommonRelicPoolCounter = 7; // or 1
        public const int RareRelicPoolCounter = 8; // or 2
        public const int ShopRelicPoolCounter = 9; // or 3
        
        /// <summary>
        /// Whatever Upfront RNG counter is at the START of RunManager.GenerateRooms
        /// </summary>
        public const int AncientCounter = 10;
    }

    public static Rng GetActSelectionRng(string seed) =>
        new(GetSeedHash(seed), "act_selection");
    
    /// <summary>
    /// Logic starts in StartRunLobby.BeginRunLocally, ignores multiplayer and unlock logic
    /// </summary>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static IReadOnlyList<ActLocations> GetRandomActs(string seed)
    {
        var rng = GetActSelectionRng(seed);
        var result = new List<ActLocations>();

        foreach (var acts in ActLocationRules.ActsByIndex)
        {
            result.Add(rng.NextItem(acts.ToList()));
        }

        return result;
    }
    
    public static Rng GetEventRng(ulong seed, string eventId)
    {
        return new Rng(seed + 0UL + GetSeedHash(eventId));
    }

    public static Rng GetPlayerRngType(ulong seed, PlayerRngType playerRngType)
    {
        return new Rng(seed, StringHelper.SnakeCase(playerRngType.ToString()));
    }
    
    public static Rng GetRunRngType(string seed, RunRngType runRngType)
    {
        var seedHash = GetSeedHash(seed);
        return new Rng(seedHash, StringHelper.SnakeCase(runRngType.ToString()));
    }
    
    public static Rng GetRunRngType(ulong seed, RunRngType runRngType)
    {
        return new Rng(seed, StringHelper.SnakeCase(runRngType.ToString()));
    }

    public static string GetRandomSeed(ulong candidate, int length = 12)
    {
        var rng = new Rng(candidate);
        return SeedHelper.GetRandomSeed(rng, length);
    }
    
    public static ulong GetSeedHash(string seed)
    {
        if (_cachedSeedString == seed)
        {
            return _cachedSeedHash;
        }

        var hash = StringHelper.GetDeterministicHashCode(seed);
        _cachedSeedString = seed;
        _cachedSeedHash = hash;
        return hash;
    }

    public static Rng GetBaseRng(string seed) => new(GetSeedHash(seed));
    
    public static PriorRngConsumption GetNeowsBonesBaseConsumption(
        int extraRewardsRngSteps = 0, 
        int extraTransformationRngSteps = 0)
    {
        // There is one NonCombatGenerationRngSteps to roll the relics,
        // and one to roll the curse. So use the correct number accordingly
        return new PriorRngConsumption(
            RewardsRngSteps: 0 + extraRewardsRngSteps,
            TransformationsRngSteps: 0 + extraTransformationRngSteps,
            NonCombatGenerationRngSteps: 1);
    }
}