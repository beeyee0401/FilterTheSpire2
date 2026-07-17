using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Potions;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class NeowsBonesFilterTests
{
    private const string SeedBonesNewLeafKaleidoscopeRegret = "AU0RDDGRZ135";
    
    // Bones / slot offset scenarios.
    private const string SeedBonesLeadPaperweightTarget = "L0YE2F8NDMK0"; // Bones with Paperweight and Master of Strat
    private const string SeedBonesKaleidoscopeSlot1LeafyPoulticeSlot2Target = "F4A16TV9U6SH";
    private const string SeedBonesLeafyPoulticeSlot1KaleidoscopeSlot2Target = "K3KEHWAE0G5Y";
    
    private const CardOptions LeadPaperweightTarget = CardOptions.MasterOfStrategy;
    private const CardOptions KaleidoscopeTarget1 = CardOptions.Finisher;
    private const CardOptions KaleidoscopeTarget2 = CardOptions.SpectrumShift;
    private const CardOptions IroncladTarget1 = CardOptions.NotYet;
    private const CardOptions IroncladTarget2 = CardOptions.HowlFromBeyond;

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
            var bonesBaseConsumption = RngHelper.GetNeowsBonesBaseConsumption();
    
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
            var bonesBasePlusKaleidoscope = RngHelper.GetNeowsBonesBaseConsumption(
                extraRewardsRngSteps: 18, 
                extraNicheRngSteps: 6
            );
    
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
            var bonesBasePlusLeafyPoultice = RngHelper.GetNeowsBonesBaseConsumption(extraTransformationRngSteps: 2);
    
            var filter = new KaleidoscopeFilter(
                [KaleidoscopeTarget1, KaleidoscopeTarget2],
                bonesBasePlusLeafyPoultice);
    
            Assert.IsTrue(filter.IsSeedValid(
                FilterTestHelpers.Request(),
                SeedBonesLeafyPoulticeSlot1KaleidoscopeSlot2Target));
        }

    #endregion
}