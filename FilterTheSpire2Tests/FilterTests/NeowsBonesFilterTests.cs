using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
public class NeowsBonesFilterTests
{
    private const string SeedBonesNewLeafKaleidoscopeRegret = "79U2UR9CSX";
    
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
}