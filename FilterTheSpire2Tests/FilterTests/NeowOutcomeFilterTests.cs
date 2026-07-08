using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using MegaCrit.Sts2.Core.Entities.Ascension;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class NeowOutcomeFilterTests
{
    private const string Seed_LeadPaperweight_Target = "PRN3PVXDF3"; // Master of Strat
    private const string Seed_NewLeaf_Target = "LU6B4H9EQ8"; // BTrance
    private const string Seed_LeafyPoultice_TwoTargets = "3XQ41HEQN4"; // Shrug and BTrance
    private const string Seed_LostCoffer_Target = "YWMH7BYBFG"; // Btrance
    private const string Seed_Kaleidoscope_TwoTargets = "XVHD11039A"; // Backstab and Ball lightning
    private const string Seed_ArcaneScroll_Target = "L86GUD376W"; // Feed

    // Same seed, different card reward result because rarity odds change after Scarcity.
    // Master of Strat on high asc, Jackpot and Mayhem on low asc
    private const string Seed_LeadPaperweight_AscSensitiveSeed = "PRN3PVXDF3"; 

    // Bones / slot offset scenarios.
    private const string Seed_Bones_LeadPaperweight_Target = "G8PD31ML6X"; // Bones with Paperweight and Master of Strat
    private const string Seed_Bones_KaleidoscopeSlot1_LeafyPoulticeSlot2_Target = "J8P5E8FXG3";
    private const string Seed_Bones_LeafyPoulticeSlot1_KaleidoscopeSlot2_Target = "K3RBVYU86J";

    private const CardOptions LeadPaperweightTarget = CardOptions.MasterOfStrategy;
    private const CardOptions KaleidoscopeTarget1 = CardOptions.Backstab;
    private const CardOptions KaleidoscopeTarget2 = CardOptions.BallLightning;
    private const CardOptions IroncladTarget1 = CardOptions.BattleTrance;
    private const CardOptions IroncladTarget2 = CardOptions.ShrugItOff;
    private const CardOptions IroncladRareTarget = CardOptions.Feed;

    // The card that would be a rare in low Asc but not rare in high Asc
    private const CardOptions AscensionSensitiveCard = CardOptions.Jackpot;

    [TestInitialize]
    public void Setup()
    {
        FilterTestHelpers.ResetConfig();
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
    }

    [TestMethod]
    public void LeadPaperweight_RngConsumption_IsSixRewardSteps()
    {
        var filter = new LeadPaperweightFilter([LeadPaperweightTarget]);

        Assert.AreEqual(new NeowRngConsumption(6, 0, 0), filter.RngConsumption);
    }

    [TestMethod]
    public void LeadPaperweight_WhenRequestedCardAppears_ReturnsTrue()
    {
        var filter = new LeadPaperweightFilter([LeadPaperweightTarget]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            Seed_LeadPaperweight_Target));
    }
    
    [TestMethod]
    public void LeadPaperweight_SameSeed_DifferentRewards()
    {
        var filter = new LeadPaperweightFilter([AscensionSensitiveCard]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.None),
            Seed_LeadPaperweight_AscSensitiveSeed));

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            Seed_LeadPaperweight_AscSensitiveSeed));
    }

    [TestMethod]
    public void LeadPaperweight_WhenTooManyCardsRequested_ReturnsTrueBecauseFilterIsIgnored()
    {
        var filter = new LeadPaperweightFilter([LeadPaperweightTarget, LeadPaperweightTarget]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            "ANY_SEED"));
    }

    [TestMethod]
    public void NewLeaf_RngConsumption_IsOneNicheStep()
    {
        var filter = new NewLeafFilter(IroncladTarget1);

        Assert.AreEqual(new NeowRngConsumption(0, 0, 1), filter.RngConsumption);
    }

    [TestMethod]
    public void NewLeaf_WhenCharacterIsAny_ReturnsTrueBecauseFilterIsIgnored()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Any;

        var filter = new NewLeafFilter(IroncladTarget1);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            "ANY_SEED"));
    }

    [TestMethod]
    public void NewLeaf_WhenRequestedTransformMatches_ReturnsTrue()
    {
        var filter = new NewLeafFilter(IroncladTarget1);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_NewLeaf_Target));
    }

    [TestMethod]
    public void LeafyPoultice_RngConsumption_IsTwoTransformationSteps()
    {
        var filter = new LeafyPoulticeFilter([IroncladTarget1]);

        Assert.AreEqual(new NeowRngConsumption(0, 2, 0), filter.RngConsumption);
    }

    [TestMethod]
    public void LeafyPoultice_WhenBothRequestedTransformsMatch_ReturnsTrue()
    {
        var filter = new LeafyPoulticeFilter([IroncladTarget1, IroncladTarget2]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_LeafyPoultice_TwoTargets));
    }

    [TestMethod]
    public void LeafyPoultice_WhenMoreCardsThanTransformsRequested_ReturnsTrueBecauseFilterIsIgnored()
    {
        var filter = new LeafyPoulticeFilter(
            [IroncladTarget1, IroncladTarget2, IroncladRareTarget]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            "ANY_SEED"));
    }

    [TestMethod]
    public void LostCoffer_RngConsumption_IsNineRewardSteps()
    {
        var filter = new LostCofferFilter([IroncladTarget1]);

        Assert.AreEqual(new NeowRngConsumption(9, 0, 0), filter.RngConsumption);
    }

    [TestMethod]
    public void LostCoffer_WhenCharacterIsAny_ReturnsTrueBecauseFilterIsIgnored()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Any;

        var filter = new LostCofferFilter([IroncladTarget1]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            "ANY_SEED"));
    }

    [TestMethod]
    public void LostCoffer_WhenRequestedCardAppears_ReturnsTrue()
    {
        var filter = new LostCofferFilter([IroncladTarget1]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_LostCoffer_Target));
    }

    [TestMethod]
    public void Kaleidoscope_RngConsumption_UsesRewardsAndNiche()
    {
        var filter = new KaleidoscopeFilter([KaleidoscopeTarget1]);

        Assert.AreEqual(18, filter.RngConsumption.RewardsRngSteps);
        Assert.AreEqual(0, filter.RngConsumption.TransformationsRngSteps);
        Assert.AreEqual(6, filter.RngConsumption.NicheRngSteps);
    }

    [TestMethod]
    public void Kaleidoscope_WhenTargetsAppearInSeparateRewards_ReturnsTrue()
    {
        var filter = new KaleidoscopeFilter([KaleidoscopeTarget1, KaleidoscopeTarget2]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_Kaleidoscope_TwoTargets));
    }

    [TestMethod]
    public void ArcaneScroll_RngConsumption_IsOneRewardStep()
    {
        var filter = new ArcaneScrollFilter([IroncladRareTarget]);

        Assert.AreEqual(new NeowRngConsumption(1, 0, 0), filter.RngConsumption);
    }

    [TestMethod]
    public void ArcaneScroll_WhenRequestedRareTransformMatches_ReturnsTrue()
    {
        var filter = new ArcaneScrollFilter([IroncladRareTarget]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_ArcaneScroll_Target));
    }

    [TestMethod]
    public void LeadPaperweight_WithBonesBaseRewardOffset_ReturnsTrue()
    {
        var bonesBaseConsumption = new NeowRngConsumption(
            RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
            TransformationsRngSteps: 0,
            NicheRngSteps: 0);

        var filter = new LeadPaperweightFilter(
            [LeadPaperweightTarget],
            bonesBaseConsumption);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_Bones_LeadPaperweight_Target));
    }

    [TestMethod]
    public void LeafyPoultice_AsSlot2AfterKaleidoscope_IgnoresKaleidoscopeRewardAndNicheConsumption()
    {
        var bonesBasePlusKaleidoscope = new NeowRngConsumption(
            RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1 + 18,
            TransformationsRngSteps: 0,
            NicheRngSteps: 6);

        var filter = new LeafyPoulticeFilter(
            [IroncladTarget1, IroncladTarget2],
            bonesBasePlusKaleidoscope);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_Bones_KaleidoscopeSlot1_LeafyPoulticeSlot2_Target));
    }

    [TestMethod]
    public void Kaleidoscope_AsSlot2AfterLeafyPoultice_IgnoresLeafyPoulticeTransformationConsumption()
    {
        var bonesBasePlusLeafyPoultice = new NeowRngConsumption(
            RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
            TransformationsRngSteps: 2,
            NicheRngSteps: 0);

        var filter = new KaleidoscopeFilter(
            [KaleidoscopeTarget1, KaleidoscopeTarget2],
            bonesBasePlusLeafyPoultice);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            Seed_Bones_LeafyPoulticeSlot1_KaleidoscopeSlot2_Target));
    }
}