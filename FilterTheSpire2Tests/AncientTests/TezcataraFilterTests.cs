// FilterTheSpire2Tests/FilterTests/Ancients/TezcataraFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class TezcataraFilterTests
{
    // TODO: find a seed and record which TezcataraOptions relic each of the 3 lists rolls.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const TezcataraOptions MatchingRelic = TezcataraOptions.ToastyMittens;
    private const TezcataraOptions NonMatchingRelic = TezcataraOptions.YummyCookie;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var tezcatara = new Tezcatara();

        Assert.IsTrue(tezcatara.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var tezcatara = new Tezcatara();

        Assert.IsTrue(tezcatara.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), PaelOptions.PaelsHorn));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicMatchesRolledOutcome_ReturnsTrue()
    {
        var tezcatara = new Tezcatara();

        Assert.IsTrue(tezcatara.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicDoesNotMatchRolledOutcome_ReturnsFalse()
    {
        var tezcatara = new Tezcatara();

        Assert.IsFalse(tezcatara.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }
}