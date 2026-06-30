using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

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
            isAnyVal ? null : RelicModelFactory.GetRelicModel(configVal)!,
            actNum));
    }

    private static void AddGenericRelicFilters(
        List<IFilter> filters)
    {
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

        if (FilterTheSpire2Config.ShopRelic != RelicOptions.Any)
        {
            filters.Add(new ShopRelicFilter(FilterTheSpire2Config.ShopRelic));
        }
    }

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

            if (neowOptions.Count != 0 || curseOption != null)
                filters.Add(new NeowsBonesFilter(neowOptions, curseOption));

            var bonesBaseConsumption = new NeowRngConsumption(
                RewardsRngSteps: AncientRules.NeowsBonesOptions.Length-1, TransformationsRngSteps: 0, NicheRngSteps: 0);

            // If only option 2 is set and option 1 is Any, treat option 2 as slot 1
            // for outcome filtering — we just need it to appear somewhere in the chosen pair.
            var effectiveOption1 = bonesOption1 != NeowOptions.Any
                ? bonesOption1
                : bonesOption2;
            var effectiveOption2 = bonesOption1 != NeowOptions.Any
                ? bonesOption2
                : NeowOptions.Any;

            var slot1Filter = BuildNeowOutcomeFilter(effectiveOption1, bonesBaseConsumption);
            if (slot1Filter != null)
            {
                filters.Add(slot1Filter);
            }

            if (effectiveOption2 != NeowOptions.Any)
            {
                var slot2Consumption = new NeowRngConsumption(
                    RewardsRngSteps: bonesBaseConsumption.RewardsRngSteps + (slot1Filter?.RngConsumption.RewardsRngSteps ?? 0), 
                    TransformationsRngSteps: bonesBaseConsumption.TransformationsRngSteps + (slot1Filter?.RngConsumption.TransformationsRngSteps ?? 0), 
                    NicheRngSteps: bonesBaseConsumption.NicheRngSteps + (slot1Filter?.RngConsumption.NicheRngSteps ?? 0));
                var slot2Filter = BuildNeowOutcomeFilter(effectiveOption2, slot2Consumption);
                if (slot2Filter != null)
                {
                    filters.Add(slot2Filter);
                }
            }

            return;
        }

        // Non-bones path — no base consumption, no slot 2
        var directFilter = BuildNeowOutcomeFilter(FilterTheSpire2Config.NeowOptions, null);
        if (directFilter != null)
        {
            filters.Add(directFilter);
        }
    }

    private static INeowOutcomeFilter? BuildNeowOutcomeFilter(
        NeowOptions option,
        NeowRngConsumption? slot1Consumption)
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

    private static INeowOutcomeFilter? BuildLeafyPoulticeFilter(NeowRngConsumption? slot1Consumption)
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

    private static INeowOutcomeFilter? BuildKaleidoscopeFilter(NeowRngConsumption? slot1Consumption)
    {
        var cardOptions = new List<CardOptions>();
        if (FilterTheSpire2Config.KaleidoscopeOption1 != CardOptions.Any)
            cardOptions.Add(FilterTheSpire2Config.KaleidoscopeOption1);
        if (FilterTheSpire2Config.KaleidoscopeOption2 != CardOptions.Any)
            cardOptions.Add(FilterTheSpire2Config.KaleidoscopeOption2);

        return cardOptions.Count > 0
            ? new KaleidoscopeFilter(cardOptions, slot1Consumption)
            : null;
    }

    private static void AddActLocationFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.Act1Locations != ActLocations.ActLocations.Any)
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act1Locations, 1));

        if (FilterTheSpire2Config.Act2Locations != ActLocations.ActLocations.Any)
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act2Locations, 2));

        if (FilterTheSpire2Config.Act3Locations != ActLocations.ActLocations.Any)
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act3Locations, 3));
    }
}