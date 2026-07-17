using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Relics;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class CapsuleRelicFilterTests
{
    private const string SmallCapsuleAnchorSeed = "4SAA1EW3F179";
    private const string LargeCapsuleAnchorVajraSeed = "0DJUHBN7PSKH";
    private const string BonesSmallAndLargeThreeRelicSeed = "TQ18ZA83KK95"; // Sturdy Clamp, Strike Dummy, Happy Flower

    [TestMethod]
    public void SmallCapsule_WhenRelicMatches_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Anchor],
            generatedRelicCount: 1);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SmallCapsuleAnchorSeed));
    }

    [TestMethod]
    public void SmallCapsule_WhenRelicDoesNotMatch_ReturnsFalse()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Vajra],
            generatedRelicCount: 1);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SmallCapsuleAnchorSeed));
    }

    [TestMethod]
    public void LargeCapsule_WhenBothRelicsMatch_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Anchor, RelicOptions.Vajra],
            generatedRelicCount: 2);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), LargeCapsuleAnchorVajraSeed));
    }

    [TestMethod]
    public void NeowsBonesSmallAndLargeCapsule_WhenThreeRelicsMatch_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch:
            [
                RelicOptions.SturdyClamp,
                RelicOptions.StrikeDummy,
                RelicOptions.HappyFlower
            ],
            generatedRelicCount: 3,
            rngConsumption: RngHelper.GetNeowsBonesBaseConsumption());

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), BonesSmallAndLargeThreeRelicSeed));
    }
    
    [TestMethod]
    public void NeowsBonesSmallAndLargeCapsule_WhenThreeRelicsMatchInDifferentOrder_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch:
            [
                RelicOptions.HappyFlower,
                RelicOptions.SturdyClamp,
                RelicOptions.StrikeDummy
            ],
            generatedRelicCount: 3,
            rngConsumption: RngHelper.GetNeowsBonesBaseConsumption());

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), BonesSmallAndLargeThreeRelicSeed));
    }
}