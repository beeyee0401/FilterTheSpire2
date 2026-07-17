// RelicFilterTests.cs
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class RelicFilterTests
{
    private const string SeedCommonRelicTarget = "4C60TQRCPWCS";
    private const string SeedUncommonRelicTarget = "H455G3QTGMLF";
    private const string SeedRareRelicTarget = "1P4ZMV156RML";
    private const string SeedShopRelicTarget = "G3BVT9NR7BME";

    private const RelicOptions CommonTarget = RelicOptions.Anchor;
    private const RelicOptions UncommonTarget = RelicOptions.HornCleat;
    private const RelicOptions RareTarget = RelicOptions.CaptainsWheel;
    private const RelicOptions ShopTarget = RelicOptions.MiniatureTent;

    [TestInitialize]
    public void Setup()
    {
        FilterTestHelpers.ResetConfig();
    }

    [TestMethod]
    public void CommonRelic_WhenFirstShuffledCommonRelicMatches_ReturnsTrue()
    {
        var filter = new CommonRelicFilter(CommonTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedCommonRelicTarget));
    }

    [TestMethod]
    public void UncommonRelic_WhenFirstShuffledUncommonRelicMatches_ReturnsTrue()
    {
        var filter = new UncommonRelicFilter(UncommonTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedUncommonRelicTarget));
    }

    [TestMethod]
    public void RareRelic_WhenFirstShuffledRareRelicMatches_ReturnsTrue()
    {
        var filter = new RareRelicFilter(RareTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedRareRelicTarget));
    }

    [TestMethod]
    public void ShopRelic_WhenLastShuffledShopRelicMatches_ReturnsTrue()
    {
        var filter = new ShopRelicFilter(ShopTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedShopRelicTarget));
    }
}