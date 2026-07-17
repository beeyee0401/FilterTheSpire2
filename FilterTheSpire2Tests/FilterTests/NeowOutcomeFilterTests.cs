using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Potions;
using MegaCrit.Sts2.Core.Entities.Ascension;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class NeowOutcomeFilterTests
{
    private const string SeedLeadPaperweightTarget = "HE68L49HK02W"; // Master of Strat
    private const string SeedNewLeafTarget = "86LMS1VBD7Y7"; // BTrance
    private const string SeedLeafyPoulticeTwoTargets = "379JUJCAQ0Q8"; // Shrug and BTrance
    private const string SeedLostCofferTarget = "V2VL1EU5PJ04"; // Btrance
    private const string SeedKaleidoscopeTwoTargets = "VW5LX2R4XT89"; // Backstab and Ball lightning
    private const string SeedArcaneScrollTarget = "KBCDV2VBPUS3"; // Feed
    private const string SeedPhialHolsterAttackAndBlockPotions = "WZ7P8J75YJME";
    private const string SeedLostCofferBeetleJuicePotion = "RQ6EF252FEPZ";
    private const string SeedLostCofferAggressionAndAshwaterPotion = "9AX9GGVT9SR4";

    // Same seed, different card reward result because rarity odds change after Scarcity.
    // Master of Strat on high asc, Jackpot and Mayhem on low asc
    private const string SeedLeadPaperweightAscSensitiveSeed = "PRN3PVXDF3"; 

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

        Assert.AreEqual(new RngConsumptionSteps(6, 0, 0), filter.RngConsumptionSteps);
    }

    [TestMethod]
    public void LeadPaperweight_WhenRequestedCardAppears_ReturnsTrue()
    {
        var filter = new LeadPaperweightFilter([LeadPaperweightTarget]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            SeedLeadPaperweightTarget));
    }
    
    [TestMethod]
    [Ignore("Hard to find one, create something to find it more easily")]
    public void LeadPaperweight_SameSeed_DifferentRewards()
    {
        var filter = new LeadPaperweightFilter([AscensionSensitiveCard]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.None),
            SeedLeadPaperweightAscSensitiveSeed));

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(AscensionLevel.DoubleBoss),
            SeedLeadPaperweightAscSensitiveSeed));
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

        Assert.AreEqual(new RngConsumptionSteps(0, 0, 1), filter.RngConsumptionSteps);
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
            SeedNewLeafTarget));
    }

    [TestMethod]
    public void LeafyPoultice_RngConsumption_IsTwoTransformationSteps()
    {
        var filter = new LeafyPoulticeFilter([IroncladTarget1]);

        Assert.AreEqual(new RngConsumptionSteps(0, 2, 0), filter.RngConsumptionSteps);
    }

    [TestMethod]
    public void LeafyPoultice_WhenBothRequestedTransformsMatch_ReturnsTrue()
    {
        var filter = new LeafyPoulticeFilter([IroncladTarget1, IroncladTarget2]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLeafyPoulticeTwoTargets));
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

    #region Lost Coffer
    
    [TestMethod]
    public void LostCoffer_RngConsumption_IsNineRewardSteps()
    {
        var filter = new LostCofferFilter([IroncladTarget1], PotionOptions.Any);

        Assert.AreEqual(new RngConsumptionSteps(11, 0, 0), filter.RngConsumptionSteps);
    }

    [TestMethod]
    public void LostCoffer_WhenCharacterIsAny_ReturnsTrueBecauseFilterIsIgnored()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Any;

        var filter = new LostCofferFilter([IroncladTarget1], PotionOptions.Any);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            "ANY_SEED"));
    }

    [TestMethod]
    public void LostCoffer_WhenRequestedCardAppears_ReturnsTrue()
    {
        var filter = new LostCofferFilter([IroncladTarget1], PotionOptions.Any);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLostCofferTarget));
    }
    
    [TestMethod]
    public void LostCoffer_WhenRequestedPotionMatches_ReturnsTrue()
    {
        var filter = new LostCofferFilter(
            [],
            PotionOptions.BeetleJuice);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLostCofferBeetleJuicePotion));
    }
    
    [TestMethod]
    public void LostCoffer_WhenRequestedPotionDoesNotMatch_ReturnsFalse()
    {
        var filter = new LostCofferFilter(
            [],
            PotionOptions.AttackPotion);

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLostCofferBeetleJuicePotion));
    }
    
    [TestMethod]
    public void LostCoffer_WhenCardAndPotionMatch_ReturnsTrue()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
        
        var filter = new LostCofferFilter(
            [CardOptions.Aggression],
            PotionOptions.Ashwater);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLostCofferAggressionAndAshwaterPotion));
    }

    [TestMethod]
    public void LostCoffer_WhenCardMatchesButPotionDoesNot_ReturnsFalse()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
        
        var filter = new LostCofferFilter(
            [CardOptions.Aggression],
            PotionOptions.FirePotion);

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLostCofferAggressionAndAshwaterPotion));
    }

    [TestMethod]
    public void LostCoffer_WhenPotionMatchesButCardDoesNot_ReturnsFalse()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
        
        var filter = new LostCofferFilter(
            [CardOptions.Anger],
            PotionOptions.Ashwater);

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedLostCofferAggressionAndAshwaterPotion));
    }

    #endregion
    
    #region Kaleidoscope
    
    [TestMethod]
    public void Kaleidoscope_RngConsumption_UsesRewardsAndNiche()
    {
        var filter = new KaleidoscopeFilter([KaleidoscopeTarget1]);

        Assert.AreEqual(18, filter.RngConsumptionSteps.RewardsRngSteps);
        Assert.AreEqual(0, filter.RngConsumptionSteps.TransformationsRngSteps);
        Assert.AreEqual(6, filter.RngConsumptionSteps.NicheRngSteps);
    }

    [TestMethod]
    public void Kaleidoscope_WhenTargetsAppearInSeparateRewards_ReturnsTrue()
    {
        var filter = new KaleidoscopeFilter([KaleidoscopeTarget1, KaleidoscopeTarget2]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedKaleidoscopeTwoTargets));
    }

    #endregion

    #region Arcane Scroll

    [TestMethod]
    public void ArcaneScroll_RngConsumption_IsOneRewardStep()
    {
        var filter = new ArcaneScrollFilter([IroncladRareTarget]);

        Assert.AreEqual(new RngConsumptionSteps(1, 0, 0), filter.RngConsumptionSteps);
    }

    [TestMethod]
    public void ArcaneScroll_WhenRequestedRareTransformMatches_ReturnsTrue()
    {
        var filter = new ArcaneScrollFilter([IroncladRareTarget]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedArcaneScrollTarget));
    }

    #endregion
    
    #region Phial Holster

    [TestMethod]
    public void PhialHolster_WhenBothRequestedPotionsMatch_ReturnsTrue()
    {
        var filter = new PhialHolsterFilter(
        [
            PotionOptions.AttackPotion,
            PotionOptions.BlockPotion
        ]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedPhialHolsterAttackAndBlockPotions));
    }
    
    [TestMethod]
    public void PhialHolster_WhenRequestedPotionsAreReversed_ReturnsTrue()
    {
        var filter = new PhialHolsterFilter(
        [
            PotionOptions.BlockPotion,
            PotionOptions.AttackPotion
        ]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedPhialHolsterAttackAndBlockPotions));
    }
    
    [TestMethod]
    [DataRow(PotionOptions.AttackPotion)]
    [DataRow(PotionOptions.BlockPotion)]
    public void PhialHolster_WhenOneRequestedPotionAppears_ReturnsTrue(
        PotionOptions potion)
    {
        var filter = new PhialHolsterFilter([potion]);

        Assert.IsTrue(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedPhialHolsterAttackAndBlockPotions));
    }
    
    [TestMethod]
    public void PhialHolster_WhenRequestedPotionDoesNotAppear_ReturnsFalse()
    {
        var filter = new PhialHolsterFilter(
        [
            PotionOptions.AttackPotion,
            PotionOptions.FirePotion
        ]);

        Assert.IsFalse(filter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedPhialHolsterAttackAndBlockPotions));
    }
    
    #endregion
}