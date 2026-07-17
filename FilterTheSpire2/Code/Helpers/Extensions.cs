using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Helpers;

public static class Extensions
{
    public static bool In<T>(this T @this, params T[] values)
    {
        return values.Contains(@this);
    }
    
    public static void FastForwardCounter(this Rng rng, int counter)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(counter);

        for (var i = 0; i < counter; i++)
        {
            rng.NextUnsignedLong();
        }
    }
}