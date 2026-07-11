// FilterTheSpire2Tests/FilterTests/Ancients/NonupeipeFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class NonupeipeFilterTests
{
    // TODO: find a seed and record which 3 NonupeipeOptions relics end up in the shuffled top-3.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const NonupeipeOptions MatchingRelic = NonupeipeOptions.DiamondDiadem;
    private const NonupeipeOptions NonMatchingRelic = NonupeipeOptions.Glitter;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var nonupeipe = new Nonupeipe();

        Assert.IsTrue(nonupeipe.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var nonupeipe = new Nonupeipe();

        Assert.IsTrue(nonupeipe.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), TanxOptions.Claws));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicIsInTopThree_ReturnsTrue()
    {
        var nonupeipe = new Nonupeipe();

        Assert.IsTrue(nonupeipe.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicIsNotInTopThree_ReturnsFalse()
    {
        var nonupeipe = new Nonupeipe();

        Assert.IsFalse(nonupeipe.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }
}