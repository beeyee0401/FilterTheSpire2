// FilterTheSpire2Tests/FilterTests/Ancients/DarvFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[Ignore("Ignored for possible changing RNG")]
public class DarvFilterTests
{
    // TODO: find act2/act3 seeds and record a matching + non-matching DarvOptions relic for each.
    // Note Darv's relic pool differs by act (EctoplasmAct2/SozuAct2 vs PhilosophersStoneAct3/VelvetChokerAct3),
    // so act2 and act3 seeds/relics are intentionally separate.
    private const string SeedAct2KnownOutcome = "REPLACE_WITH_SEED";
    private const DarvOptions Act2MatchingRelic = DarvOptions.SneckoEye;
    private const DarvOptions Act2NonMatchingRelic = DarvOptions.EctoplasmAct2;

    private const string SeedAct3KnownOutcome = "REPLACE_WITH_SEED";
    private const DarvOptions Act3MatchingRelic = DarvOptions.PhilosophersStone;
    private const DarvOptions Act3NonMatchingRelic = DarvOptions.VelvetChoker;

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var darv = new Darv(2);

        Assert.IsTrue(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct2KnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var darv = new Darv(2);

        Assert.IsTrue(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct2KnownOutcome), OrobasOptions.SandCastle));
    }

    [TestMethod]
    public void CheckOptions_Act2_WhenRelicMatchesRolledOutcome_ReturnsTrue()
    {
        var darv = new Darv(2);

        Assert.IsTrue(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct2KnownOutcome), Act2MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_Act2_WhenRelicDoesNotMatchRolledOutcome_ReturnsFalse()
    {
        var darv = new Darv(2);

        Assert.IsFalse(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct2KnownOutcome), Act2NonMatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_Act3_WhenRelicMatchesRolledOutcome_ReturnsTrue()
    {
        var darv = new Darv(3);

        Assert.IsTrue(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct3KnownOutcome), Act3MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_Act3_WhenRelicDoesNotMatchRolledOutcome_ReturnsFalse()
    {
        var darv = new Darv(3);

        Assert.IsFalse(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct3KnownOutcome), Act3NonMatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_Act2Seed_UsingAct3RelicPool_NeverMatches()
    {
        // Sanity check that actNum actually gates the relic pool: an act3-only relic
        // (e.g. PhilosophersStoneAct3) should never be reachable when constructed for act 2.
        var darv = new Darv(2);

        Assert.IsFalse(darv.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedAct2KnownOutcome), DarvOptions.PhilosophersStone));
    }
}