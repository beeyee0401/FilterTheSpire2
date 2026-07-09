using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class CapsuleRelicFilterTests
{
    private const string SmallCapsuleAnchorSeed = "SHFRFA087T";
    private const string LargeCapsuleAnchorVajraSeed = "43U6DVB2VZ";
    private const string BonesSmallAndLargeThreeRelicSeed = "KNXJMHDE6Q";

    [TestMethod]
    public void SmallCapsule_WhenFirstRelicMatches_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Anchor],
            generatedRelicCount: 1);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SmallCapsuleAnchorSeed));
    }

    [TestMethod]
    public void SmallCapsule_WhenFirstRelicDoesNotMatch_ReturnsFalse()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Vajra],
            generatedRelicCount: 1);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SmallCapsuleAnchorSeed));
    }

    [TestMethod]
    public void LargeCapsule_WhenBothRelicsMatchInOrder_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Anchor, RelicOptions.Vajra],
            generatedRelicCount: 2);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), LargeCapsuleAnchorVajraSeed));
    }

    [TestMethod]
    public void LargeCapsule_WhenRelicsMatchButWrongOrder_ReturnsFalse()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch: [RelicOptions.Vajra, RelicOptions.Anchor],
            generatedRelicCount: 2);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), LargeCapsuleAnchorVajraSeed));
    }

    [TestMethod]
    public void NeowsBonesSmallAndLargeCapsule_WhenThreeRelicsMatch_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch:
            [
                RelicOptions.AmethystAubergine,
                RelicOptions.Anchor,
                RelicOptions.BloodVial
            ],
            generatedRelicCount: 3,
            rngConsumption: new RngConsumptionSteps(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
                TransformationsRngSteps: 0,
                NicheRngSteps: 0));

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), BonesSmallAndLargeThreeRelicSeed));
    }
}