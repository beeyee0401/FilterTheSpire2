// FilterTheSpire2Tests/FilterTests/Ancients/OrobasFilteringTests.cs

using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2Tests.FilterTests;

namespace FilterTheSpire2Tests.AncientTests;

[TestClass]
[DoNotParallelize] // reads FilterTheSpire2Config.Character / SeaGlassCharacter / OrobasOptions
[Ignore("Ignored for possible changing RNG")]
public class OrobasFilterTests
{
    // TODO: find a seed, with Character = Any (default), and record what relic Orobas rolls.
    private const string SeedKnownOutcome = "REPLACE_WITH_SEED";
    private const OrobasOptions MatchingRelic = OrobasOptions.TouchOfOrobas;
    private const OrobasOptions NonMatchingRelic = OrobasOptions.SandCastle;

    // TODO: find a seed where SeaGlass is rolled in list1 (i.e. NextFloat >= 1/3).
    private const string SeedRollsSeaGlass = "REPLACE_WITH_SEED";

    [TestInitialize]
    public void Setup()
    {
        FilterTestHelpers.ResetConfig();
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsNull_ReturnsTrue()
    {
        var orobas = new Orobas();

        Assert.IsTrue(orobas.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), null));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicOptionIsWrongEnumType_ReturnsTrue()
    {
        var orobas = new Orobas();

        Assert.IsTrue(orobas.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NeowOptions.LostCoffer));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicMatchesRolledOutcome_ReturnsTrue()
    {
        var orobas = new Orobas();

        Assert.IsTrue(orobas.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), MatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenRelicDoesNotMatchRolledOutcome_ReturnsFalse()
    {
        var orobas = new Orobas();

        Assert.IsFalse(orobas.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedKnownOutcome), NonMatchingRelic));
    }

    [TestMethod]
    public void CheckOptions_WhenSeaGlassRolledButCharacterMismatchRequired_ReturnsFalse()
    {
        // SeaGlass is character-gated: even if the item itself rolls, CheckOptions should only
        // return true if the rolled seaGlassChar equals the configured SeaGlassCharacter.
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
        FilterTheSpire2Config.SeaGlassCharacter = CharacterOptions.Silent;
        FilterTheSpire2Config.OrobasOptions = OrobasOptions.SeaGlass;

        var orobas = new Orobas();

        // TODO: confirm Seed_RollsSeaGlass actually rolls a character other than Silent here.
        Assert.IsFalse(orobas.CheckOptions(AncientTestHelpers.ToNumericSeed(SeedRollsSeaGlass), OrobasOptions.SeaGlass));
    }
}