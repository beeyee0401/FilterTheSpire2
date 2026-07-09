using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Filters;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class ActLocationFilterTests
{
    private const string OVERGROWTH_SEED = "TZ5D4WGQHK";
    private const string UNDERDOCKS_SEED = "LPTVQ1LBB0";
    private const string HIVE_SEED = "NJPKY5DCED";
    private const string GLORY_SEED = "Z1M0PNW8DB";
    
    [TestMethod]
    public void Act1Overgrowth_WhenSeedHasOvergrowthInAct1_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Overgrowth, 1);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), OVERGROWTH_SEED));
    }

    [TestMethod]
    public void Act1Underdocks_WhenSeedHasOvergrowthInAct1_ReturnsFalse()
    {
        var filter = new ActLocationFilter(ActLocations.Underdocks, 1);
        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), OVERGROWTH_SEED));
    }

    [TestMethod]
    public void Act1Underdocks_WhenSeedHasUnderdocksInAct1_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Underdocks, 1);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), UNDERDOCKS_SEED));
    }

    [TestMethod]
    public void Act2Hive_WhenAnyValidSeed_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Hive, 2);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), HIVE_SEED));
    }

    [TestMethod]
    public void Act3Glory_WhenAnyValidSeed_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Glory, 3);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), GLORY_SEED));
    }

    [TestMethod]
    public void InvalidLocationForAct_WhenAct1Glory_ReturnsTrueBecauseFilterIsIgnored()
    {
        const string seed = "";
        var filter = new ActLocationFilter(ActLocations.Glory, 1);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), seed));
    }

    [TestMethod]
    public void InvalidLocationForAct_WhenAct2Overgrowth_ReturnsTrueBecauseFilterIsIgnored()
    {
        const string seed = "";
        var filter = new ActLocationFilter(ActLocations.Overgrowth, 2);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), seed));
    }
}