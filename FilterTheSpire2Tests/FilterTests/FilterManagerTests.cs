using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.Relics;

namespace FilterTheSpire2Tests.FilterTests;

[TestClass]
[DoNotParallelize]
public class FilterManagerTests
{
    private const string SeedBonesNewLeafKaleidoscope = "15P9SBV7MFZA";
    
    // Block/Attack from Holster, Colorless from Coffer
    private const string SeedBonesLostCofferPhialHolster = "1H13GMKSMYHM";
    
    // Attack and block pots from Phial, attack pot from Coffer
    private const string SeedBonesPhialCofferDupePotions = "X0Y73RTVGBQ6";
    
    [TestInitialize]
    public void Setup()
    {
        FilterTestHelpers.ResetConfig();
        FilterTheSpire2Config.Character = CharacterOptions.Ironclad;
    }

    #region General logic

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
    public void CreateFiltersFromSettings_WhenDirectPhialHolsterHasPotions_AddsPhialFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.PhialHolster;
        FilterTheSpire2Config.PhialHolsterPotionOption1 =
            PotionOptions.AttackPotion;
        FilterTheSpire2Config.PhialHolsterPotionOption2 =
            PotionOptions.BlockPotion;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.AreEqual(1, filters.OfType<PhialHolsterFilter>().Count());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenPhialPotionsAreAny_DoesNotAddPhialFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.PhialHolster;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.IsFalse(filters.OfType<PhialHolsterFilter>().Any());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenLostCofferHasOnlyPotion_AddsLostCofferFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LostCoffer;
        FilterTheSpire2Config.LostCofferCardOption = CardOptions.Any;
        FilterTheSpire2Config.LostCofferPotionOption =
            PotionOptions.BloodPotion;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.AreEqual(1, filters.OfType<LostCofferFilter>().Count());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenLostCofferHasOnlyCard_AddsLostCofferFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LostCoffer;
        FilterTheSpire2Config.LostCofferCardOption =
            CardOptions.BattleTrance;
        FilterTheSpire2Config.LostCofferPotionOption =
            PotionOptions.Any;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.AreEqual(1, filters.OfType<LostCofferFilter>().Count());
    }

    #endregion

    #region Neow's Bones tests

    [TestMethod]
    public void CreateFiltersFromSettings_WhenOnlyBonesSlot2Configured_AddsItsOutcomeFilterWithBaseOffset()
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
    

    [TestMethod]
    public void CreateFiltersFromSettings_WhenOptionsOverlap_RequiresConfiguredOrderWithoutOutcomeFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.NewLeaf;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.Kaleidoscope;

        // Leave NewLeaf and Kaleidoscope card selections as Any so no outcome
        // filters are created. Ordering should still come from option metadata.
        var filters = FilterManager.CreateFiltersFromSettings();
        var bonesFilter = filters.OfType<NeowsBonesFilter>().Single();

        Assert.IsTrue(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedBonesNewLeafKaleidoscope));
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenOverlappingOptionsConfiguredInWrongOrder_RejectsSeed()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.Kaleidoscope;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.NewLeaf;

        // No outcome filters are configured. The manager must still recognize
        // that both options consume Niche RNG and require sequence.
        var filters = FilterManager.CreateFiltersFromSettings();
        var bonesFilter = filters.OfType<NeowsBonesFilter>().Single();

        Assert.IsFalse(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            SeedBonesNewLeafKaleidoscope));
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenOptionsDoNotOverlap_AllowsEitherConfiguredOrder()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.LeafyPoultice;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.Kaleidoscope;

        var filters = FilterManager.CreateFiltersFromSettings();
        var bonesFilter = filters.OfType<NeowsBonesFilter>().Single();

        var seed = FilterTestHelpers.FindSeedWithBonesOptions(
            NeowOptions.Kaleidoscope,
            NeowOptions.LeafyPoultice);

        Assert.IsTrue(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            seed));
    }
    
    [TestMethod]
    [DataRow(NeowOptions.SmallCapsule)]
    [DataRow(NeowOptions.LargeCapsule)]
    [DataRow(NeowOptions.ArcaneScroll)]
    [DataRow(NeowOptions.LeadPaperweight)]
    [DataRow(NeowOptions.LostCoffer)]
    [DataRow(NeowOptions.Kaleidoscope)]
    public void CreateFiltersFromSettings_WhenScrollBoxesOverlapsRewards_ForcesScrollBoxesSecond(
        NeowOptions otherOption)
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;

        // Deliberately configure Scroll Boxes first. FilterManager should resolve
        // the required order as the deterministic option first, Scroll Boxes second.
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.ScrollBoxes;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = otherOption;

        var filters = FilterManager.CreateFiltersFromSettings();
        var bonesFilter = filters.OfType<NeowsBonesFilter>().Single();

        var correctOrderSeed = FilterTestHelpers.FindSeedWithBonesOptions(
            otherOption,
            NeowOptions.ScrollBoxes);

        var incorrectOrderSeed = FilterTestHelpers.FindSeedWithBonesOptions(
            NeowOptions.ScrollBoxes,
            otherOption);

        Assert.IsTrue(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            correctOrderSeed));

        Assert.IsFalse(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            incorrectOrderSeed));
    }
    
    [TestMethod]
    [DataRow(NeowOptions.NewLeaf)]
    [DataRow(NeowOptions.LeafyPoultice)]
    public void CreateFiltersFromSettings_WhenScrollBoxesDoesNotOverlap_DoesNotForceSequence(
        NeowOptions otherOption)
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.ScrollBoxes;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = otherOption;

        var filters = FilterManager.CreateFiltersFromSettings();
        var bonesFilter = filters.OfType<NeowsBonesFilter>().Single();

        var scrollBoxesFirstSeed = FilterTestHelpers.FindSeedWithBonesOptions(
            NeowOptions.ScrollBoxes,
            otherOption);

        var scrollBoxesSecondSeed = FilterTestHelpers.FindSeedWithBonesOptions(
            otherOption,
            NeowOptions.ScrollBoxes);

        Assert.IsTrue(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            scrollBoxesFirstSeed));

        Assert.IsTrue(bonesFilter.IsSeedValid(
            FilterTestHelpers.Request(),
            scrollBoxesSecondSeed));
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenBonesHasPhialAndLostCoffer_AddsSeparatePotionFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 =
            NeowOptions.PhialHolster;
        FilterTheSpire2Config.NeowsBonesRelicOption2 =
            NeowOptions.LostCoffer;

        FilterTheSpire2Config.PhialHolsterPotionOption1 =
            PotionOptions.AttackPotion;
        FilterTheSpire2Config.PhialHolsterPotionOption2 =
            PotionOptions.BlockPotion;
        FilterTheSpire2Config.LostCofferPotionOption =
            PotionOptions.BloodPotion;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.AreEqual(1, filters.OfType<PhialHolsterFilter>().Count());
        Assert.AreEqual(1, filters.OfType<LostCofferFilter>().Count());
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenBonesPotionOutcomesMatch_ValidatesSeed()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 =
            NeowOptions.PhialHolster;
        FilterTheSpire2Config.NeowsBonesRelicOption2 =
            NeowOptions.LostCoffer;

        FilterTheSpire2Config.PhialHolsterPotionOption1 =
            PotionOptions.AttackPotion;
        FilterTheSpire2Config.PhialHolsterPotionOption2 =
            PotionOptions.BlockPotion;
        FilterTheSpire2Config.LostCofferPotionOption =
            PotionOptions.ColorlessPotion;

        var filters = FilterManager.CreateFiltersFromSettings();
        var request = FilterTestHelpers.Request(filters: filters);

        Assert.IsTrue(FilterManager.ValidateFilters(
            request,
            SeedBonesLostCofferPhialHolster));
    }
    
    [TestMethod]
    public void CreateFiltersFromSettings_WhenBonesAndPhialPotionSelectionsAreReversed_ValidatesSeed()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 =
            NeowOptions.LostCoffer;
        FilterTheSpire2Config.NeowsBonesRelicOption2 =
            NeowOptions.PhialHolster;

        FilterTheSpire2Config.PhialHolsterPotionOption1 =
            PotionOptions.BlockPotion;
        FilterTheSpire2Config.PhialHolsterPotionOption2 =
            PotionOptions.AttackPotion;
        FilterTheSpire2Config.LostCofferPotionOption =
            PotionOptions.ColorlessPotion;

        var filters = FilterManager.CreateFiltersFromSettings();
        var request = FilterTestHelpers.Request(filters: filters);

        Assert.IsTrue(FilterManager.ValidateFilters(
            request,
            SeedBonesLostCofferPhialHolster));
    }
    
    [TestMethod]
    public void LostCoffer_CanMatchPotionAlsoGeneratedByPhialHolster()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 =
            NeowOptions.PhialHolster;
        FilterTheSpire2Config.NeowsBonesRelicOption2 =
            NeowOptions.LostCoffer;

        FilterTheSpire2Config.PhialHolsterPotionOption1 =
            PotionOptions.AttackPotion;
        FilterTheSpire2Config.PhialHolsterPotionOption2 =
            PotionOptions.BlockPotion;

        // Lost Coffer duplicates one of Phial Holster's potions.
        FilterTheSpire2Config.LostCofferPotionOption =
            PotionOptions.AttackPotion;

        var filters = FilterManager.CreateFiltersFromSettings();
        var request = FilterTestHelpers.Request(filters: filters);

        Assert.IsTrue(FilterManager.ValidateFilters(
            request,
            SeedBonesPhialCofferDupePotions));
    }

    #endregion

    #region Capsule relic tests

    [TestMethod]
    public void CreateFiltersFromSettings_WhenSmallCapsuleHasRelic_AddsAncientAndCapsuleFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.SmallCapsule;
        FilterTheSpire2Config.CapsuleRelicOption1 = RelicOptions.Anchor;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.IsTrue(filters.OfType<AncientRelicFilter>().Any());
        Assert.IsTrue(filters.OfType<CapsuleRelicFilter>().Any());
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenLargeCapsuleHasTwoRelics_AddsOneCapsuleFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LargeCapsule;
        FilterTheSpire2Config.CapsuleRelicOption1 = RelicOptions.Anchor;
        FilterTheSpire2Config.CapsuleRelicOption2 = RelicOptions.Vajra;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.AreEqual(1, filters.OfType<CapsuleRelicFilter>().Count());
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenCapsuleHasAllAny_DoesNotAddCapsuleFilter()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LargeCapsule;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.IsFalse(filters.OfType<CapsuleRelicFilter>().Any());
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenBonesHasSmallAndLargeCapsuleWithRelics_AddsNeowsBonesAndCapsuleFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.NeowsBones;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.SmallCapsule;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.LargeCapsule;
        FilterTheSpire2Config.CapsuleRelicOption1 = RelicOptions.AmethystAubergine;
        FilterTheSpire2Config.CapsuleRelicOption2 = RelicOptions.Anchor;
        FilterTheSpire2Config.CapsuleRelicOption3 = RelicOptions.BloodVial;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.IsTrue(filters.OfType<NeowsBonesFilter>().Any());
        Assert.IsTrue(filters.OfType<CapsuleRelicFilter>().Any());
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenCapsuleRelicSelected_SuppressesCommonUncommonRareButNotShop()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.SmallCapsule;
        FilterTheSpire2Config.CapsuleRelicOption1 = RelicOptions.Anchor;

        FilterTheSpire2Config.CommonRelic = RelicOptions.Anchor;
        FilterTheSpire2Config.UncommonRelic = RelicOptions.Akabeko;
        FilterTheSpire2Config.RareRelic = RelicOptions.ArtOfWar;
        FilterTheSpire2Config.ShopRelic = RelicOptions.MiniatureTent;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.IsFalse(filters.OfType<CommonRelicFilter>().Any());
        Assert.IsFalse(filters.OfType<UncommonRelicFilter>().Any());
        Assert.IsFalse(filters.OfType<RareRelicFilter>().Any());
        Assert.IsTrue(filters.OfType<ShopRelicFilter>().Any());
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenCapsuleSelectedButNoCapsuleRelicSelected_DoesNotSuppressGenericRelicFilters()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.SmallCapsule;
        FilterTheSpire2Config.CommonRelic = RelicOptions.Anchor;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.IsTrue(filters.OfType<CommonRelicFilter>().Any());
        Assert.IsFalse(filters.OfType<CapsuleRelicFilter>().Any());
    }

    [TestMethod]
    public void CreateFiltersFromSettings_WhenLargeCapsuleOnlySecondRelicSelected_TreatsSecondAsEffectiveFirstSelection()
    {
        FilterTheSpire2Config.NeowOptions = NeowOptions.LargeCapsule;
        FilterTheSpire2Config.CapsuleRelicOption1 = RelicOptions.Any;
        FilterTheSpire2Config.CapsuleRelicOption2 = RelicOptions.Vajra;

        var filters = FilterManager.CreateFiltersFromSettings();

        Assert.AreEqual(1, filters.OfType<CapsuleRelicFilter>().Count());
    }

    #endregion
}