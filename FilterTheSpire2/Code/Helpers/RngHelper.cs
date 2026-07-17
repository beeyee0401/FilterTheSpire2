using System.Text;
using FilterTheSpire2.Code.Acts;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Helpers;

public static class RngHelper
{
    public static class RngCounters
    {
        public const int NewLeafNicheCounter = 1;
        public const int KaleidoscopeNicheCounter = 6;
        public const int UncommonRelicPoolCounter = 112;
        public const int CommonRelicPoolCounter = 143;
        public const int RareRelicPoolCounter = 168;
        public const int ShopRelicPoolCounter = 205;
        public const int AncientCounter = 230;
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
    
    public static Rng GetRunRngType(ulong seed, RunRngType runRngType)
    {
        return new Rng(seed, StringHelper.SnakeCase(runRngType.ToString()));
    }

    public static string GetRandomSeed(ulong candidate, int length = 12)
    {
        var rng = new Rng(candidate);
        return SeedHelper.GetRandomSeed(rng, length);
    }
    
    public static ulong GetSeedHash(string seed) => StringHelper.GetDeterministicHashCode(seed);

    public static Rng GetBaseRng(string seed) => new(GetSeedHash(seed));
}