// FilterTheSpire2Tests/FilterTests/Ancients/TanxFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class TanxFilterTests
{
    // TODO: find a seed and record which 3 TanxOptions relics end up in the shuffled top-3.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const TanxOptions MatchingRelic = TanxOptions.WarHammer;
    private const TanxOptions NonMatchingRelic = TanxOptions.Sai;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var tanx = new Tanx();

        Assert.IsTrue(tanx.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var tanx = new Tanx();

        Assert.IsTrue(tanx.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), VakuuOptions.Fiddle));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicIsInTopThree_ReturnsTrue()
    {
        var tanx = new Tanx();

        Assert.IsTrue(tanx.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicIsNotInTopThree_ReturnsFalse()
    {
        var tanx = new Tanx();

        Assert.IsFalse(tanx.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }
}