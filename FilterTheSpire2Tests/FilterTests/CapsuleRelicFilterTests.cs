using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class CapsuleRelicFilterTests
{
    private const string SmallCapsuleAnchorSeed = "LCVY5YC9ZQ3Q";
    private const string LargeCapsuleAnchorVajraSeed = "3CRJ4Q0P9J4F";
    private const string BonesSmallAndLargeThreeRelicSeed = "5KQDRY6RR21T"; // Sturdy Clamp, Strike Dummy, Pen nib 

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
                RelicOptions.PenNib
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
                RelicOptions.PenNib,
                RelicOptions.SturdyClamp,
                RelicOptions.StrikeDummy
            ],
            generatedRelicCount: 3,
            rngConsumption: RngHelper.GetNeowsBonesBaseConsumption());

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), BonesSmallAndLargeThreeRelicSeed));
    }
}