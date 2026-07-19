using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Filters;
using MegaCrit.Sts2.Core.Entities.Ascension;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class BossFilterTests
{
    private const string SeedAct1RollsVantom = "1A96AXBPJZ5L";
    private const string SeedAct2RollsKaiserCrab = "1A96AXBPJZ5L";
    private const string SeedAct3RollsQueen = "1A96AXBPJZ5L";

    private const string SeedAct3DoubleBoss = "1A96AXBPJZ5L";
    private const BossOptions Act3DoubleBossFirstBoss = BossOptions.Queen;
    private const BossOptions Act3DoubleBossSecondBoss = BossOptions.TestSubject;
    private const BossOptions Act3DoubleBossNonMatchingBoss = BossOptions.Aeonglass;

    [TestMethod]
    public void Act1_WhenBossMatchesRoll_ReturnsTrue()
    {
        var filter = new BossFilter(BossOptions.Vantom, 1);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct1RollsVantom));
    }

    [TestMethod]
    public void Act1_WhenBossDoesNotMatchRoll_ReturnsFalse()
    {
        var filter = new BossFilter(BossOptions.TheKin, 1);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct1RollsVantom));
    }

    [TestMethod]
    public void Act2_WhenBossMatchesRoll_ReturnsTrue()
    {
        var filter = new BossFilter(BossOptions.KaiserCrab, 2);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsKaiserCrab));
    }

    [TestMethod]
    public void Act3_WhenBossMatchesRoll_ReturnsTrue()
    {
        var filter = new BossFilter(BossOptions.Queen, 3);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct3RollsQueen));
    }

    #region Second boss (Double Boss ascension)

    [TestMethod]
    public void SecondBoss_WhenAscensionIsBelowDoubleBoss_ReturnsTrueBecauseFilterIsIgnored()
    {
        var filter = new BossFilter(BossOptions.Queen, 3, isSecondBoss: true);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.None),
            "ANY_SEED"));
    }

    [TestMethod]
    public void SecondBoss_AtDoubleBossAscension_WhenSecondBossMatchesRoll_ReturnsTrue()
    {
        var filter = new BossFilter(Act3DoubleBossSecondBoss, 3, isSecondBoss: true);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            SeedAct3DoubleBoss));
    }

    [TestMethod]
    public void SecondBoss_AtDoubleBossAscension_WhenSecondBossDoesNotMatchRoll_ReturnsFalse()
    {
        var filter = new BossFilter(Act3DoubleBossNonMatchingBoss, 3, isSecondBoss: true);

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            SeedAct3DoubleBoss));
    }

    [TestMethod]
    public void FirstBoss_AtDoubleBossAscension_StillMatchesFirstRoll_RegardlessOfSecondBoss()
    {
        // Confirms the non-second-boss BossFilter isn't affected by Double Boss ascension — it
        // should only ever check the act's first-rolled boss.
        var filter = new BossFilter(Act3DoubleBossFirstBoss, 3);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            SeedAct3DoubleBoss));
    }

    #endregion
}