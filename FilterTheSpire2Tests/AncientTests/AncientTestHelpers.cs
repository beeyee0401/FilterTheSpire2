using FilterTheSpire2.Code.Helpers;

namespace FilterTheSpire2Tests.AncientTests;

internal static class AncientTestHelpers
{
    /// <summary>
    /// Reproduces the numeric seed AncientRelicFilter passes into AbstractAncient.CheckOptions,
    /// so individual Ancient classes can be tested directly without going through act-rolling.
    /// </summary>
    public static ulong ToNumericSeed(string seed) =>
        RngHelper.GetSeedHash(seed);
}