using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2Tests.AncientTests;

internal static class AncientTestHelpers
{
    /// <summary>
    /// Reproduces the numeric seed AncientRelicFilter passes into AbstractAncient.CheckOptions,
    /// so individual Ancient classes can be tested directly without going through act-rolling.
    /// </summary>
    public static uint ToNumericSeed(string seed) =>
        new Rng((uint)StringHelper.GetDeterministicHashCode(seed)).Seed;
}