using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class FilterManagerTests
{
    [TestInitialize]
    public void Setup()
    {
        FilterTestHelpers.ResetConfig();
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenAllSettingsAreAny_ReturnsNoFilters()
    {
        var filters = FilterManager.CreateFiltersFromSettings();
        Assert.IsEmpty(filters);
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenDirectLeadPaperweightHasCard_AddsAncientAndOutcomeFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LeadPaperweight;
        FilterTheSpire2Config.LeadPaperweightOption = CardOptions.MasterOfStrategy;
    
        var filters = FilterManager.CreateFiltersFromSettings();
    
        Assert.IsTrue(filters.OfType<AncientRelicFilter>().Any());
        Assert.IsTrue(filters.OfType<LeadPaperweightFilter>().Any());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenDirectLeadPaperweightCardIsAny_AddsOnlyAncientFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LeadPaperweight;
        FilterTheSpire2Config.LeadPaperweightOption = CardOptions.Any;
    
        var filters = FilterManager.CreateFiltersFromSettings();
    
        Assert.IsTrue(filters.OfType<AncientRelicFilter>().Any());
        Assert.IsFalse(filters.OfType<LeadPaperweightFilter>().Any());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenGenericRelicsConfigured_AddsExpectedRelicFilters()
    {
        FilterTheSpire2Config.CommonRelic = RelicOptions.Anchor;
        FilterTheSpire2Config.ShopRelic = RelicOptions.MiniatureTent;
    
        var filters = FilterManager.CreateFiltersFromSettings();
    
        Assert.IsTrue(filters.OfType<CommonRelicFilter>().Any());
        Assert.IsTrue(filters.OfType<ShopRelicFilter>().Any());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenBonesHasOnlySlot2_TreatsSlot2AsEffectiveSlot1()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.Any;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.LeadPaperweight;
        FilterTheSpire2Config.LeadPaperweightOption = CardOptions.MasterOfStrategy;
    
        var filters = FilterManager.CreateFiltersFromSettings();
    
        Assert.IsTrue(filters.OfType<NeowsBonesFilter>().Any());
        Assert.IsTrue(filters.OfType<LeadPaperweightFilter>().Any());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenBonesHasTwoOutcomeOptions_AddsBothOutcomeFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.Kaleidoscope;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.LeafyPoultice;
        FilterTheSpire2Config.KaleidoscopeOption1 = CardOptions.BeatDown;
        FilterTheSpire2Config.LeafyPoulticeOption1 = CardOptions.Finesse;
    
        var filters = FilterManager.CreateFiltersFromSettings();
    
        Assert.IsTrue(filters.OfType<NeowsBonesFilter>().Any());
        Assert.IsTrue(filters.OfType<KaleidoscopeFilter>().Any());
        Assert.IsTrue(filters.OfType<LeafyPoulticeFilter>().Any());
    }
}