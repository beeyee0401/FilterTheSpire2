using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2Tests.FilterTests;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[DoNotParallelize]
[Ignore("Ignored for possible changing RNG")]
public class AncientRelicFilterTests
{
    // --- Act 1 (Neow) ---
    // TODO: find a seed and confirm which NeowOptions relic Neow actually rolls for it.
    private const string SeedAct1NeowKnown = "REPLACE_WITH_SEED";
    private const NeowOptions Act1NeowMatchingRelic = NeowOptions.LostCoffer; // whichever the seed above rolls
    private const NeowOptions Act1NeowNonMatchingRelic = NeowOptions.WingedBoots; // any relic NOT rolled by the seed above

    // --- Act 2 ---
    // TODO: find a seed where Act2Ancient rolls to Orobas specifically.
    private const string SeedAct2RollsOrobas = "REPLACE_WITH_SEED";
    private const OrobasOptions Act2OrobasMatchingRelic = OrobasOptions.TouchOfOrobas; // whichever relic that seed rolls
    private const OrobasOptions Act2OrobasNonMatchingRelic = OrobasOptions.SandCastle;

    // --- Act 3 ---
    // TODO: find a seed where Act3Ancient rolls to Vakuu specifically.
    private const string SeedAct3RollsVakuu = "REPLACE_WITH_SEED";
    private const VakuuOptions Act3VakuuMatchingRelic = VakuuOptions.Fiddle;
    private const VakuuOptions Act3VakuuNonMatchingRelic = VakuuOptions.MusicBox;

    // --- Darv (multi-act) ---
    // TODO: find seeds where Darv is the shared ancient rolled for act2/act3 respectively,
    // along with a relic that Darv's roll includes for that seed.
    private const string SeedAct2RollsDarv = "REPLACE_WITH_SEED";
    private const DarvOptions Act2DarvMatchingRelic = DarvOptions.SneckoEye;

    private const string SeedAct3RollsDarv = "REPLACE_WITH_SEED";
    private const DarvOptions Act3DarvMatchingRelic = DarvOptions.PhilosophersStone;

    [TestInitialize]
    public void Setup()
    {
        FilterTestHelpers.ResetConfig();
    }

    #region Act 1 (Neow)

    [TestMethod]
    public void Act1_WhenRelicOptionIsNull_ReturnsFalse()
    {
        // AncientRelicFilter's act-1 branch is `relicOption != null && ...`, so a null
        // relic option always fails, regardless of seed.
        var filter = new AncientRelicFilter(Ancient.Neow, null, 1);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct1NeowKnown));
    }

    [TestMethod]
    public void Act1_WhenRelicOptionMatchesNeowRoll_ReturnsTrue()
    {
        var filter = new AncientRelicFilter(Ancient.Neow, Act1NeowMatchingRelic, 1);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct1NeowKnown));
    }

    [TestMethod]
    public void Act1_WhenRelicOptionDoesNotMatchNeowRoll_ReturnsFalse()
    {
        var filter = new AncientRelicFilter(Ancient.Neow, Act1NeowNonMatchingRelic, 1);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct1NeowKnown));
    }

    #endregion

    #region Act 2

    [TestMethod]
    public void Act2_WhenSelectedAncientDoesNotMatchRolledAncient_ReturnsFalse()
    {
        // The seed rolls Orobas for act 2; asking for Pael should fail before relic checks even run.
        var filter = new AncientRelicFilter(Ancient.Pael, null, 2);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsOrobas));
    }

    [TestMethod]
    public void Act2_WhenSelectedAncientMatchesAndRelicOptionIsNull_ReturnsTrue()
    {
        // No relic constraint: only the act-ancient match matters.
        var filter = new AncientRelicFilter(Ancient.Orobas, null, 2);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsOrobas));
    }

    [TestMethod]
    public void Act2_WhenSelectedAncientMatchesAndRelicMatches_ReturnsTrue()
    {
        var filter = new AncientRelicFilter(Ancient.Orobas, Act2OrobasMatchingRelic, 2);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsOrobas));
    }

    [TestMethod]
    public void Act2_WhenSelectedAncientMatchesButRelicDoesNotMatch_ReturnsFalse()
    {
        var filter = new AncientRelicFilter(Ancient.Orobas, Act2OrobasNonMatchingRelic, 2);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsOrobas));
    }

    #endregion

    #region Act 3

    [TestMethod]
    public void Act3_WhenSelectedAncientDoesNotMatchRolledAncient_ReturnsFalse()
    {
        var filter = new AncientRelicFilter(Ancient.Tanx, null, 3);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct3RollsVakuu));
    }

    [TestMethod]
    public void Act3_WhenSelectedAncientMatchesAndRelicMatches_ReturnsTrue()
    {
        var filter = new AncientRelicFilter(Ancient.Vakuu, Act3VakuuMatchingRelic, 3);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct3RollsVakuu));
    }

    [TestMethod]
    public void Act3_WhenSelectedAncientMatchesButRelicDoesNotMatch_ReturnsFalse()
    {
        var filter = new AncientRelicFilter(Ancient.Vakuu, Act3VakuuNonMatchingRelic, 3);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct3RollsVakuu));
    }

    #endregion

    #region Darv (multi-act)

    [TestMethod]
    public void Darv_WhenRolledForAct2AndRelicMatches_ReturnsTrue()
    {
        var filter = new AncientRelicFilter(Ancient.Darv, Act2DarvMatchingRelic, 2);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsDarv));
    }

    [TestMethod]
    public void Darv_WhenRolledForAct3AndRelicMatches_ReturnsTrue()
    {
        // Confirms actNum is threaded through to Darv correctly (act-specific relic pool:
        // Ectoplasm/Sozu for act2 vs Philosopher's Stone/Velvet Choker for act3).
        var filter = new AncientRelicFilter(Ancient.Darv, Act3DarvMatchingRelic, 3);

        Assert.IsTrue(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct3RollsDarv));
    }

    [TestMethod]
    public void Darv_WhenRolledForAct2ButQueriedAsAct3_ReturnsFalse()
    {
        // Same seed/relic, but Darv isn't the act-3 ancient for this seed, so it should fail
        // even though it *is* the act-2 ancient.
        var filter = new AncientRelicFilter(Ancient.Darv, Act2DarvMatchingRelic, 3);

        Assert.IsFalse(filter.IsSeedValid(FilterTestHelpers.Request(), SeedAct2RollsDarv));
    }

    #endregion
}