// FilterTheSpire2Tests/FilterTests/Ancients/PaelFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class PaelFilterTests
{
    // TODO: find a seed and record which PaelOptions relic each of the 3 lists rolls.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const PaelOptions MatchingRelic = PaelOptions.PaelsHorn; // any relic rolled from list1/2/3
    private const PaelOptions NonMatchingRelic = PaelOptions.PaelsWing;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var pael = new Pael();

        Assert.IsTrue(pael.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var pael = new Pael();

        Assert.IsTrue(pael.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), TezcataraOptions.ToyBox));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicMatchesRolledOutcome_ReturnsTrue()
    {
        var pael = new Pael();

        Assert.IsTrue(pael.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicDoesNotMatchRolledOutcome_ReturnsFalse()
    {
        var pael = new Pael();

        Assert.IsFalse(pael.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }
}