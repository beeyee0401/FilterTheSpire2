using System.Text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

public static class RngHelper
{
    public static Rng GetEventRng(uint seed, string eventId)
    {
        return new Rng((uint) (seed + 1UL + (ulong) StringHelper.GetDeterministicHashCode(eventId)));
    }

    public static string GetRandomSeed(long offset, int length = 10)
    {
        var timestamp = (uint) (DateTime.UtcNow.Ticks * 100 + offset);
        var rng = new Rng(timestamp);
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