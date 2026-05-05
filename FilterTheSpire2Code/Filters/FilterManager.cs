using FilterTheSpire2.FilterTheSpire2Code.DropdownOptions;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public static class FilterManager
{
    public static bool ValidateFilters(string seed)
    {
        // var runRngSet = new RunRngSet(seed);
        return Filters.TrueForAll(f => f.IsSeedValid(seed));;
    }

    public static bool HasFilters() => Filters.Any();
    
    private static readonly List<IFilter> Filters = [];
 
    public static void InitializeFiltersFromSettings()
    {
        CheckAncientRelicConfigs(FilterTheSpire2Config.NeowOptions, NeowOptions.Any);
        CheckAncientRelicConfigs(FilterTheSpire2Config.OrobasOptions, OrobasOptions.Any);
        CheckAncientRelicConfigs(FilterTheSpire2Config.PaelOptions, PaelOptions.Any);
        CheckAncientRelicConfigs(FilterTheSpire2Config.TezcataraOptions, TezcataraOptions.Any);
        CheckAncientRelicConfigs(FilterTheSpire2Config.NonupeipeOptions, NonupeipeOptions.Any);
        CheckAncientRelicConfigs(FilterTheSpire2Config.TanxOptions, TanxOptions.Any);
        CheckAncientRelicConfigs(FilterTheSpire2Config.VakuuOptions, VakuuOptions.Any);
    }

    private static void CheckAncientRelicConfigs<TEnum>(TEnum configVal, TEnum e) where TEnum : Enum
    {
        if (configVal.Equals(e)) return;
        var filter = new AncientRelicFilter(RelicModelFactory.GetRelicModel(configVal)!);
        Filters.Add(filter);
    }
}