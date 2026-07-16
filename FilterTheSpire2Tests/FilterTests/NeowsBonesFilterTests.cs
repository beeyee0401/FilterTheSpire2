using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Potions;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class NeowsBonesFilterTests
{
    private const string SeedBonesNewLeafKaleidoscopeRegret = "79U2UR9CSX";
    
    // Bones / slot offset scenarios.
    private const string SeedBonesLeadPaperweightTarget = "G8PD31ML6X"; // Bones with Paperweight and Master of Strat
    private const string SeedBonesKaleidoscopeSlot1LeafyPoulticeSlot2Target = "J8P5E8FXG3";
    private const string SeedBonesLeafyPoulticeSlot1KaleidoscopeSlot2Target = "K3RBVYU86J";
    
    private const CardOptions LeadPaperweightTarget = CardOptions.MasterOfStrategy;
    private const CardOptions KaleidoscopeTarget1 = CardOptions.Backstab;
    private const CardOptions KaleidoscopeTarget2 = CardOptions.BallLightning;
    private const CardOptions IroncladTarget1 = CardOptions.BattleTrance;
    private const CardOptions IroncladTarget2 = CardOptions.ShrugItOff;

    #region General logic

    [TestMethod]
        public void IsSeedValid_WithNoRestrictions_ReturnsTrue()
        {
            var filter = new NeowsBonesFilter(
                ImmutableArray<NeowOptions>.Empty,
                null);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsTrue(filter.IsSeedValid(request, "Hello"));
            Assert.IsTrue(filter.IsSeedValid(request, "ABC123"));
            Assert.IsTrue(filter.IsSeedValid(request, "999999"));
            Assert.IsTrue(filter.IsSeedValid(request, ""));
        }
        
        [TestMethod]
        public void IsSeedValid_WhenSeedContainsRequestedOption_ReturnsTrue()
        {
            var filter = new NeowsBonesFilter(
                [NeowOptions.NewLeaf],
                null);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsTrue(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_WhenSeedDoesNotContainRequestedOption_ReturnsFalse()
        {
            var filter = new NeowsBonesFilter(
                [NeowOptions.LeadPaperweight],
                null);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_WhenBothRequestedOptionsMatch_ReturnsTrue()
        {
            var filter = new NeowsBonesFilter(
                [
                    NeowOptions.NewLeaf,
                    NeowOptions.Kaleidoscope
                ],
                null);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsTrue(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_WhenCurseMatches_ReturnsTrue()
        {
            var filter = new NeowsBonesFilter(
                ImmutableArray<NeowOptions>.Empty,
                CardOptions.Regret);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsTrue(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_WhenCurseDoesNotMatch_ReturnsFalse()
        {
            var filter = new NeowsBonesFilter(
                ImmutableArray<NeowOptions>.Empty,
                CardOptions.Shame);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_DuplicateConfigOptions_TreatedAsSingleOption()
        {
            var filter = new NeowsBonesFilter(
                [
                    NeowOptions.NewLeaf,
                    NeowOptions.NewLeaf // duplicate from UI not preventing it in config
                ],
                null);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsTrue(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_FirstOptionMismatch_FailsEvenIfSecondMatches()
        {
            var filter = new NeowsBonesFilter(
                [NeowOptions.LeadPaperweight, NeowOptions.NewLeaf],
                null);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_OptionsMatchButCurseMismatch_ReturnsFalse()
        {
            var filter = new NeowsBonesFilter(
                [NeowOptions.NewLeaf],
                CardOptions.Shame);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }
        
        [TestMethod]
        public void IsSeedValid_CurseMatchesButOptionsMismatch_ReturnsFalse()
        {
            var filter = new NeowsBonesFilter(
                [NeowOptions.LeadPaperweight],
                CardOptions.Regret);
    
            var request = FilterTestHelpers.Request();
    
            Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        }

    #endregion
    
    #region Order checks
    
    [TestMethod]
    public void IsSeedValid_UnorderedMode_AllowsSwappedTwoOptions()
    {
        var request = FilterTestHelpers.Request();

        var normal = new NeowsBonesFilter(
            [NeowOptions.NewLeaf, NeowOptions.Kaleidoscope],
            null,
            requireSequenceForTwoOptions: false);

        var swapped = new NeowsBonesFilter(
            [NeowOptions.Kaleidoscope, NeowOptions.NewLeaf],
            null,
            requireSequenceForTwoOptions: false);

        Assert.IsTrue(normal.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        Assert.IsTrue(swapped.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
    }

    [TestMethod]
    public void IsSeedValid_SequenceMode_RejectsSwappedTwoOptions()
    {
        var request = FilterTestHelpers.Request();

        var normal = new NeowsBonesFilter(
            [NeowOptions.NewLeaf, NeowOptions.Kaleidoscope],
            null,
            requireSequenceForTwoOptions: true);

        var swapped = new NeowsBonesFilter(
            [NeowOptions.Kaleidoscope, NeowOptions.NewLeaf],
            null,
            requireSequenceForTwoOptions: true);

        Assert.IsTrue(normal.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
        Assert.IsFalse(swapped.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
    }

    [TestMethod]
    public void IsSeedValid_UnorderedMode_StillRejectsMissingOption()
    {
        var request = FilterTestHelpers.Request();

        var filter = new NeowsBonesFilter(
            [NeowOptions.NewLeaf, NeowOptions.LeadPaperweight],
            null,
            requireSequenceForTwoOptions: false);

        Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
    }

    [TestMethod]
    public void IsSeedValid_UnorderedMode_StillRequiresCurse()
    {
        var request = FilterTestHelpers.Request();

        var filter = new NeowsBonesFilter(
            [NeowOptions.Kaleidoscope, NeowOptions.NewLeaf],
            CardOptions.Shame,
            requireSequenceForTwoOptions: false);

        Assert.IsFalse(filter.IsSeedValid(request, SeedBonesNewLeafKaleidoscopeRegret));
    }
    
    #endregion

    #region RNG Fast forward

    [TestMethod]
        public void LeadPaperweight_WithBonesBaseRewardOffset_ReturnsTrue()
        {
            var bonesBaseConsumption = new RngConsumptionSteps(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
                TransformationsRngSteps: 0,
                NicheRngSteps: 0);
    
            var filter = new LeadPaperweightFilter(
                [LeadPaperweightTarget],
                bonesBaseConsumption);
    
            Assert.IsTrue(filter.IsSeedValid(
                FilterTestHelpers.Request(),
                SeedBonesLeadPaperweightTarget));
        }
    
        [TestMethod]
        public void LeafyPoultice_AsSlot2AfterKaleidoscope_IgnoresKaleidoscopeRewardAndNicheConsumption()
        {
            var bonesBasePlusKaleidoscope = new RngConsumptionSteps(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1 + 18,
                TransformationsRngSteps: 0,
                NicheRngSteps: 6);
    
            var filter = new LeafyPoulticeFilter(
                [IroncladTarget1, IroncladTarget2],
                bonesBasePlusKaleidoscope);
    
            Assert.IsTrue(filter.IsSeedValid(
                FilterTestHelpers.Request(),
                SeedBonesKaleidoscopeSlot1LeafyPoulticeSlot2Target));
        }
    
        [TestMethod]
        public void Kaleidoscope_AsSlot2AfterLeafyPoultice_IgnoresLeafyPoulticeTransformationConsumption()
        {
            var bonesBasePlusLeafyPoultice = new RngConsumptionSteps(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
                TransformationsRngSteps: 2,
                NicheRngSteps: 0);
    
            var filter = new KaleidoscopeFilter(
                [KaleidoscopeTarget1, KaleidoscopeTarget2],
                bonesBasePlusLeafyPoultice);
    
            Assert.IsTrue(filter.IsSeedValid(
                FilterTestHelpers.Request(),
                SeedBonesLeafyPoulticeSlot1KaleidoscopeSlot2Target));
        }

    #endregion
}