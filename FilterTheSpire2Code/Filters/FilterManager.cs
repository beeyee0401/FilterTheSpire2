using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public static class FilterManager
{
    public static bool ValidateFilters(SeedSearchRequest request, string seed)
    {
        return request.Filters
            .All(f => f.IsSeedValid(request, seed));;
    }

    public static List<IFilter> CreateFiltersFromSettings()
    {
        var filters = new List<IFilter>();

        HandleAncientFilters(filters);
        AddNeowRelicOutcomeFilters(filters);
        AddGenericRelicFilters(filters);

        return filters;
    }

    private static void HandleAncientFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.Any)
        {
            AddAncientRelicFilterIfNeeded(
                filters,
                FilterTheSpire2Config.NeowOptions,
                NeowOptions.Any,
                1);   
        }

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
    /// For specifically the outcome of what Neow relic is chosen. Such as card transforms or specific relics
    /// </summary>
    /// <param name="filters"></param>
    private static void AddNeowRelicOutcomeFilters(List<IFilter> filters)
    {
        switch (FilterTheSpire2Config.NeowOptions)
        {
            case NeowOptions.NewLeaf:
                if (FilterTheSpire2Config.NewLeafOption != CardOptions.Any)
                {
                    filters.Add(new NewLeafFilter(FilterTheSpire2Config.NewLeafOption));
                }
                break;
            case NeowOptions.LeafyPoultice:
            {
                var cardOptions = new List<CardOptions>();
                if (FilterTheSpire2Config.LeafyPoulticeOption1 != CardOptions.Any)
                {
                    cardOptions.Add(FilterTheSpire2Config.LeafyPoulticeOption1);
                }

                if (FilterTheSpire2Config.LeafyPoulticeOption2 != CardOptions.Any)
                {
                    cardOptions.Add(FilterTheSpire2Config.LeafyPoulticeOption2);
                }

                if (cardOptions.Count != 0)
                {
                    filters.Add(new LeafyPoulticeFilter(cardOptions));
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}