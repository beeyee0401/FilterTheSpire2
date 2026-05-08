using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

public static class RngHelper
{
    public static Rng GetEventRng(uint seed, string eventId)
    {
        return new Rng((uint) (seed + 1UL + (ulong) StringHelper.GetDeterministicHashCode(eventId)));
    }
}