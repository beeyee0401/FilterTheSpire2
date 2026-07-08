// RelicFilterTests.cs
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class RelicFilterTests
{
    private const string Seed_CommonRelic_Target = "FBHQ4E6JJ4";
    private const string Seed_UncommonRelic_Target = "EXEGYHCM0T";
    private const string Seed_RareRelic_Target = "RLKAU60YWS";
    private const string Seed_ShopRelic_Target = "XSWBZ6TKZ3";

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

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), Seed_CommonRelic_Target));
    }

    [TestMethod]
    public void UncommonRelic_WhenFirstShuffledUncommonRelicMatches_ReturnsTrue()
    {
        var filter = new UncommonRelicFilter(UncommonTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), Seed_UncommonRelic_Target));
    }

    [TestMethod]
    public void RareRelic_WhenFirstShuffledRareRelicMatches_ReturnsTrue()
    {
        var filter = new RareRelicFilter(RareTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), Seed_RareRelic_Target));
    }

    [TestMethod]
    public void ShopRelic_WhenLastShuffledShopRelicMatches_ReturnsTrue()
    {
        var filter = new ShopRelicFilter(ShopTarget);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), Seed_ShopRelic_Target));
    }
}