using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class CapsuleRelicFilterTests
{
    private const string SmallCapsuleAnchorSeed = "SHFRFA087T";
    private const string LargeCapsuleAnchorVajraSeed = "43U6DVB2VZ";
    private const string BonesSmallAndLargeThreeRelicSeed = "KNXJMHDE6Q"; // AmethystAubergine, Anchor, BloodVial

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
    
    [TestMethod]
    public void NeowsBonesSmallAndLargeCapsule_WhenThreeRelicsMatchInDifferentOrder_ReturnsTrue()
    {
        var filter = new CapsuleRelicFilter(
            relicsToMatch:
            [
                RelicOptions.Anchor,
                RelicOptions.BloodVial,
                RelicOptions.AmethystAubergine
            ],
            generatedRelicCount: 3,
            rngConsumption: new RngConsumptionSteps(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
                TransformationsRngSteps: 0,
                NicheRngSteps: 0));

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), BonesSmallAndLargeThreeRelicSeed));
    }
}