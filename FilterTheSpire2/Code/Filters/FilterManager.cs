using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Relics;
using FilterTheSpire2.Code.SeedSearcher;

namespace FilterTheSpire2.Code.Filters;

public static class FilterManager
{
    public static bool ValidateFilters(SeedSearchRequest request, string seed)
    {
        return request.Filters
            .All(f => f.IsSeedValid(request, seed));
    }

    public static List<IFilter> CreateFiltersFromSettings()
    {
        var filters = new List<IFilter>();

        HandleAncientFilters(filters);
        AddNeowRelicOutcomeFilters(filters);
        AddCapsuleRelicOutcomeFilter(filters);
        AddGenericRelicFilters(filters);
        AddActLocationFilters(filters);

        return filters;
    }

    private static void HandleAncientFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.Any)
            AddAncientRelicFilterIfNeeded(
                filters,
                FilterTheSpire2Config.NeowOptions,
                NeowOptions.Any,
                1);

        switch (FilterTheSpire2Config.Act2Ancient)
        {
            case Ancient.Orobas:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.OrobasOptions,
                    OrobasOptions.Any,
                    2);
                break;

            case Ancient.Pael:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.PaelOptions,
                    PaelOptions.Any,
                    2);
                break;

            case Ancient.Tezcatara:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.TezcataraOptions,
                    TezcataraOptions.Any,
                    2);
                break;
        }

        switch (FilterTheSpire2Config.Act3Ancient)
        {
            case Ancient.Nonupeipe:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.NonupeipeOptions,
                    NonupeipeOptions.Any,
                    3);
                break;

            case Ancient.Tanx:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.TanxOptions,
                    TanxOptions.Any,
                    3);
                break;

            case Ancient.Vakuu:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.VakuuOptions,
                    VakuuOptions.Any,
                    3);
                break;
        }

        if (FilterTheSpire2Config.Act2Ancient == Ancient.Darv || FilterTheSpire2Config.Act3Ancient == Ancient.Darv)
        {
            AddAncientRelicFilterIfNeeded(
                filters,
                FilterTheSpire2Config.DarvOptions,
                DarvOptions.Any,
                FilterTheSpire2Config.Act2Ancient == Ancient.Darv ? 2 : 3);
        }
    }

    private static void AddAncientRelicFilterIfNeeded<TEnum>(
        List<IFilter> filters,
        TEnum configVal,
        TEnum anyValue,
        int actNum) where TEnum : Enum
    {
        var isAnyVal = configVal.Equals(anyValue);

        filters.Add(new AncientRelicFilter(
            actNum == 2
                ? FilterTheSpire2Config.Act2Ancient
                : FilterTheSpire2Config.Act3Ancient,
            isAnyVal ? null : configVal,
            actNum));
    }

    private static void AddGenericRelicFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.ShopRelic != RelicOptions.Any)
        {
            filters.Add(new ShopRelicFilter(FilterTheSpire2Config.ShopRelic));
        }

        if (ShouldSuppressGenericRelicFilters())
        {
            return;
        }

        if (FilterTheSpire2Config.CommonRelic != RelicOptions.Any)
        {
            filters.Add(new CommonRelicFilter(FilterTheSpire2Config.CommonRelic));
        }

        if (FilterTheSpire2Config.UncommonRelic != RelicOptions.Any)
        {
            filters.Add(new UncommonRelicFilter(FilterTheSpire2Config.UncommonRelic));
        }

        if (FilterTheSpire2Config.RareRelic != RelicOptions.Any)
        {
            filters.Add(new RareRelicFilter(FilterTheSpire2Config.RareRelic));
        }
    }

    private static bool ShouldSuppressGenericRelicFilters()
    {
        return GetTotalCapsuleRelicCount() > 0 &&
               (
                   FilterTheSpire2Config.CapsuleRelicOption1 != RelicOptions.Any ||
                   FilterTheSpire2Config.CapsuleRelicOption2 != RelicOptions.Any ||
                   FilterTheSpire2Config.CapsuleRelicOption3 != RelicOptions.Any
               );
    }

    // private static void AddCapsuleRelicOutcomeFilter(List<IFilter> filters)
    // {
    //     var generatedRelicCount = GetTotalCapsuleRelicCount();
    //
    //     if (generatedRelicCount == 0)
    //     {
    //         return;
    //     }
    //
    //     var selectedRelics = new List<RelicOptions>
    //     {
    //         FilterTheSpire2Config.CapsuleRelicOption1,
    //         FilterTheSpire2Config.CapsuleRelicOption2,
    //         FilterTheSpire2Config.CapsuleRelicOption3,
    //     };
    //
    //     selectedRelics = selectedRelics
    //         .Take(generatedRelicCount)
    //         .Where(relic => relic != RelicOptions.Any)
    //         .Distinct()
    //         .ToList();
    //
    //     if (selectedRelics.Count == 0)
    //     {
    //         return;
    //     }
    //
    //     filters.Add(new CapsuleRelicFilter(
    //         selectedRelics,
    //         generatedRelicCount));
    // }

    /// <summary>
    ///     For specifically the outcome of what Neow relic is chosen. Such as card transforms or specific relics
    /// </summary>
    /// <param name="filters"></param>
    private static void AddNeowRelicOutcomeFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.NeowOptions == NeowOptions.NeowsBones)
        {
            var bonesOption1 = FilterTheSpire2Config.NeowsBonesRelicOption1;
            var bonesOption2 = FilterTheSpire2Config.NeowsBonesRelicOption2;

            var neowOptions = new List<NeowOptions>();
            if (bonesOption1 != NeowOptions.Any)
            {
                neowOptions.Add(bonesOption1);
            }

            if (bonesOption2 != NeowOptions.Any)
            {
                neowOptions.Add(bonesOption2);
            }

            CardOptions? curseOption = FilterTheSpire2Config.NeowsBonesCurseOption != CardOptions.Any
                ? FilterTheSpire2Config.NeowsBonesCurseOption
                : null;

            var bonesBaseConsumption = new RngConsumptionSteps(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
                TransformationsRngSteps: 0,
                NicheRngSteps: 0);

            var option1Filter = BuildNeowOutcomeFilter(bonesOption1, bonesBaseConsumption);
            var option2Filter = BuildNeowOutcomeFilter(bonesOption2, bonesBaseConsumption);

            var requireSequence =
                bonesOption1 != NeowOptions.Any &&
                bonesOption2 != NeowOptions.Any &&
                (
                    RequiresScrollBoxesSecond(neowOptions) ||
                    option1Filter != null &&
                    option2Filter != null &&
                    DoConsumptionsOverlap(option1Filter.RngConsumptionSteps, option2Filter.RngConsumptionSteps)
                );

            if (neowOptions.Count != 0 || curseOption != null)
            {
                filters.Add(new NeowsBonesFilter([..neowOptions], curseOption, requireSequence));
            }

            if (!requireSequence)
            {
                if (option1Filter != null)
                {
                    filters.Add(option1Filter);
                }

                if (option2Filter != null)
                {
                    filters.Add(option2Filter);
                }

                return;
            }

            if (option1Filter != null)
            {
                filters.Add(option1Filter);
            }

            if (bonesOption2 != NeowOptions.Any)
            {
                var slot2Consumption = AddConsumption(
                    bonesBaseConsumption,
                    option1Filter?.RngConsumptionSteps ?? RngConsumptionSteps.None);

                var slot2Filter = BuildNeowOutcomeFilter(bonesOption2, slot2Consumption);
                if (slot2Filter != null)
                {
                    filters.Add(slot2Filter);
                }
            }

            return;
        }

        var directFilter = BuildNeowOutcomeFilter(FilterTheSpire2Config.NeowOptions, null);
        if (directFilter != null)
        {
            filters.Add(directFilter);
        }
    }

    private static INeowOutcomeFilter? BuildNeowOutcomeFilter(
        NeowOptions option,
        RngConsumptionSteps? slot1Consumption)
    {
        return option switch
        {
            NeowOptions.NewLeaf when FilterTheSpire2Config.NewLeafOption != CardOptions.Any =>
                new NewLeafFilter(FilterTheSpire2Config.NewLeafOption, slot1Consumption),

            NeowOptions.LeafyPoultice => BuildLeafyPoulticeFilter(slot1Consumption),

            NeowOptions.LeadPaperweight when FilterTheSpire2Config.LeadPaperweightOption != CardOptions.Any =>
                new LeadPaperweightFilter([FilterTheSpire2Config.LeadPaperweightOption], slot1Consumption),

            NeowOptions.LostCoffer when FilterTheSpire2Config.LostCofferOption != CardOptions.Any =>
                new LostCofferFilter([FilterTheSpire2Config.LostCofferOption], slot1Consumption),

            NeowOptions.Kaleidoscope => BuildKaleidoscopeFilter(slot1Consumption),

            NeowOptions.ArcaneScroll when FilterTheSpire2Config.ArcaneScrollOption != CardOptions.Any =>
                new ArcaneScrollFilter([FilterTheSpire2Config.ArcaneScrollOption], slot1Consumption),

            _ => null
        };
    }

    private static INeowOutcomeFilter? BuildLeafyPoulticeFilter(RngConsumptionSteps? slot1Consumption)
    {
        var cardOptions = new List<CardOptions>();
        if (FilterTheSpire2Config.LeafyPoulticeOption1 != CardOptions.Any)
            cardOptions.Add(FilterTheSpire2Config.LeafyPoulticeOption1);
        if (FilterTheSpire2Config.LeafyPoulticeOption2 != CardOptions.Any)
            cardOptions.Add(FilterTheSpire2Config.LeafyPoulticeOption2);

        return cardOptions.Count > 0
            ? new LeafyPoulticeFilter(cardOptions, slot1Consumption)
            : null;
    }

    private static INeowOutcomeFilter? BuildKaleidoscopeFilter(RngConsumptionSteps? slot1Consumption)
    {
        var cardOptions = new List<CardOptions>();
        if (FilterTheSpire2Config.KaleidoscopeOption1 != CardOptions.Any)
        {
            cardOptions.Add(FilterTheSpire2Config.KaleidoscopeOption1);
        }

        if (FilterTheSpire2Config.KaleidoscopeOption2 != CardOptions.Any)
        {
            cardOptions.Add(FilterTheSpire2Config.KaleidoscopeOption2);
        }

        return cardOptions.Count > 0
            ? new KaleidoscopeFilter(cardOptions, slot1Consumption)
            : null;
    }

    private static void AddActLocationFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.Act1Locations != ActLocations.Any)
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act1Locations, 1));

        if (FilterTheSpire2Config.Act2Locations != ActLocations.Any)
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act2Locations, 2));

        if (FilterTheSpire2Config.Act3Locations != ActLocations.Any)
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act3Locations, 3));
    }

    private static void AddCapsuleRelicOutcomeFilter(List<IFilter> filters)
    {
        var generatedRelicCount = GetTotalCapsuleRelicCount();

        if (generatedRelicCount == 0)
        {
            return;
        }

        var selectedRelics = new List<RelicOptions>
            {
                FilterTheSpire2Config.CapsuleRelicOption1,
                FilterTheSpire2Config.CapsuleRelicOption2,
                FilterTheSpire2Config.CapsuleRelicOption3,
            }
            .Take(generatedRelicCount)
            // Intentionally collapse Any slots. Capsule relic filters are treated as
            // "match these selected relics in order from the first generated relic",
            // not as positional slot constraints.
            .Where(relic => relic != RelicOptions.Any)
            .Distinct()
            .ToList();

        if (selectedRelics.Count == 0)
        {
            return;
        }

        filters.Add(new CapsuleRelicFilter(
            selectedRelics,
            generatedRelicCount,
            GetCapsulePriorConsumption()));
    }

    #region helpers

    private static bool DoConsumptionsOverlap(RngConsumptionSteps a, RngConsumptionSteps b)
    {
        return (a.RewardsRngSteps > 0 && b.RewardsRngSteps > 0) ||
               (a.TransformationsRngSteps > 0 && b.TransformationsRngSteps > 0) ||
               (a.NicheRngSteps > 0 && b.NicheRngSteps > 0);
    }

    private static RngConsumptionSteps GetCapsulePriorConsumption()
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.NeowsBones)
        {
            return RngConsumptionSteps.None;
        }

        var bonesOption1 = FilterTheSpire2Config.NeowsBonesRelicOption1;
        var bonesOption2 = FilterTheSpire2Config.NeowsBonesRelicOption2;

        var bonesBaseConsumption = new RngConsumptionSteps(
            RewardsRngSteps: AncientRules.NeowsBonesOptions.Length - 1,
            TransformationsRngSteps: 0,
            NicheRngSteps: 0);

        var option1Filter = BuildNeowOutcomeFilter(bonesOption1, bonesBaseConsumption);

        // If ScrollBoxes is paired with a capsule, NeowsBonesFilter forces ScrollBoxes second.
        // This is so ScrollBoxes does not consume Rewards RNG before the capsule relic rolls.
        if (bonesOption1 == NeowOptions.ScrollBoxes || bonesOption2 == NeowOptions.ScrollBoxes)
        {
            if (GetCapsuleRelicCount(bonesOption1) > 0 || GetCapsuleRelicCount(bonesOption2) > 0)
            {
                return bonesBaseConsumption;
            }
        }

        if (GetCapsuleRelicCount(bonesOption1) > 0)
        {
            return bonesBaseConsumption;
        }

        return AddConsumption(
            bonesBaseConsumption,
            option1Filter?.RngConsumptionSteps ?? RngConsumptionSteps.None);
    }

    private static int GetTotalCapsuleRelicCount()
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.NeowsBones)
        {
            return GetCapsuleRelicCount(FilterTheSpire2Config.NeowOptions);
        }

        return GetCapsuleRelicCount(FilterTheSpire2Config.NeowsBonesRelicOption1) +
               GetCapsuleRelicCount(FilterTheSpire2Config.NeowsBonesRelicOption2);
    }

    public static int GetCapsuleRelicCount(NeowOptions option)
    {
        return option switch
        {
            NeowOptions.SmallCapsule => 1,
            NeowOptions.LargeCapsule => 2,
            _ => 0
        };
    }

    private static bool RequiresScrollBoxesSecond(IReadOnlyList<NeowOptions> requested)
    {
        return requested.Contains(NeowOptions.ScrollBoxes) &&
               requested.Any(option => GetCapsuleRelicCount(option) > 0);
    }

    private static RngConsumptionSteps AddConsumption(
        RngConsumptionSteps a,
        RngConsumptionSteps b)
    {
        return new RngConsumptionSteps(
            RewardsRngSteps: a.RewardsRngSteps + b.RewardsRngSteps,
            TransformationsRngSteps: a.TransformationsRngSteps + b.TransformationsRngSteps,
            NicheRngSteps: a.NicheRngSteps + b.NicheRngSteps);
    }

    #endregion
}