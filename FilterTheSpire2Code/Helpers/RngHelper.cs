using System.Text;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

public static class RngHelper
{
    public static class RngCounters
    {
        public const int UncommonRelicPoolCounter = 112;
        public const int CommonRelicPoolCounter = 143;
        public const int RareRelicPoolCounter = 168;
        public const int ShopRelicPoolCounter = 205;
        public const int AncientCounter = 230;
    }

    public static Rng GetActSelectionRng(string seed) =>
        new((uint)StringHelper.GetDeterministicHashCode(seed), "act_selection");
    
    public static Rng GetEventRng(uint seed, string eventId)
    {
        return new Rng((uint) (seed + 0UL + (ulong) StringHelper.GetDeterministicHashCode(eventId)));
    }

    public static Rng GetPlayerRngType(uint seed, PlayerRngType playerRngType)
    {
        return new Rng(seed + 1, StringHelper.SnakeCase(playerRngType.ToString()));
    }
    
    public static Rng GetRunRngType(uint seed, RunRngType runRngType)
    {
        return new Rng(seed, StringHelper.SnakeCase(runRngType.ToString()));
    }

    public static string GetRandomSeed(long candidate, int length = 10)
    {
        var rng = new Rng((uint)candidate);
        string text;
        do
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var index = 0; index < length; ++index)
            {
                stringBuilder.Append(rng.NextItem("0123456789ABCDEFGHJKLMNPQRSTUVWXYZ"));
            }
            text = stringBuilder.ToString();
        }
        while (BadWordChecker.ContainsBadWord(text));
        return text;
    }
}