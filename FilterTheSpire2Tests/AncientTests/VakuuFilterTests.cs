// FilterTheSpire2Tests/FilterTests/Ancients/VakuuFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class VakuuFilterTests
{
    // TODO: find a seed and record which relic ends up first (index 0) in each of the 3 shuffled lists.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const VakuuOptions MatchingRelic = VakuuOptions.Fiddle;
    private const VakuuOptions NonMatchingRelic = VakuuOptions.MusicBox;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var vakuu = new Vakuu();

        Assert.IsTrue(vakuu.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var vakuu = new Vakuu();

        Assert.IsTrue(vakuu.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), TanxOptions.Claws));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicIsFirstInAnyShuffledList_ReturnsTrue()
    {
        var vakuu = new Vakuu();

        Assert.IsTrue(vakuu.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicIsNotFirstInAnyShuffledList_ReturnsFalse()
    {
        var vakuu = new Vakuu();

        Assert.IsFalse(vakuu.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }
}