// FilterTheSpire2Tests/FilterTests/Ancients/NeowFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class NeowFilterTests
{
    // TODO: find a seed and record which NeowOptions relic Neow.CheckOptions actually returns true for.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const NeowOptions MatchingRelic = NeowOptions.LostCoffer;
    private const NeowOptions NonMatchingRelic = NeowOptions.WingedBoots;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var neow = new Neow();

        Assert.IsTrue(neow.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        // relicOption is only meaningful when it's a NeowOptions; any other enum is a no-op pass-through.
        var neow = new Neow();

        Assert.IsTrue(neow.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), OrobasOptions.SandCastle));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicMatchesRolledOutcome_ReturnsTrue()
    {
        var neow = new Neow();

        Assert.IsTrue(neow.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicDoesNotMatchRolledOutcome_ReturnsFalse()
    {
        var neow = new Neow();

        Assert.IsFalse(neow.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }
}