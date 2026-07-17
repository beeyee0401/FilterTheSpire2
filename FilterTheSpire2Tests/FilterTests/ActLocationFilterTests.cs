using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Filters;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class ActLocationFilterTests
{
    private const string OvergrowthSeed = "4WAB3Q10D8BC";
    private const string UnderdocksSeed = "EK74PT6SQ6VX";
    private const string HiveSeed = "";
    private const string GlorySeed = "";
    
    [TestMethod]
    public void Act1Overgrowth_WhenSeedHasOvergrowthInAct1_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Overgrowth, 1);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), OvergrowthSeed));
    }

    [TestMethod]
    public void Act1Underdocks_WhenSeedHasOvergrowthInAct1_ReturnsFalse()
    {
        var filter = new ActLocationFilter(ActLocations.Underdocks, 1);
        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), OvergrowthSeed));
    }

    [TestMethod]
    public void Act1Underdocks_WhenSeedHasUnderdocksInAct1_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Underdocks, 1);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), UnderdocksSeed));
    }

    [TestMethod]
    [Ignore("Ignored until alternate Act 2 added")]
    public void Act2Hive_WhenAnyValidSeed_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Hive, 2);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), HiveSeed));
    }
    
    [TestMethod]
    [Ignore("Ignored until alternate Act 3 added")]
    public void Act3Glory_WhenAnyValidSeed_ReturnsTrue()
    {
        var filter = new ActLocationFilter(ActLocations.Glory, 3);
        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), GlorySeed));
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