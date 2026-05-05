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

    public static bool HasFilters() => Filters.Count != 0;
    
    private static readonly List<IFilter> Filters = [];
 
    public static void InitializeFiltersFromSettings()
    {
        Filters.Clear();
        CheckAncientRelicConfigs(FilterTheSpire2Config.NeowOptions, NeowOptions.Any, 1);
        switch (FilterTheSpire2Config.Act2Ancients)
        {
            case Ancient.Orobas:
                CheckAncientRelicConfigs(FilterTheSpire2Config.OrobasOptions, OrobasOptions.Any, 2);
                break;
            case Ancient.Pael:
                CheckAncientRelicConfigs(FilterTheSpire2Config.PaelOptions, PaelOptions.Any, 2);
                break;
            case Ancient.Tezcatara:
                CheckAncientRelicConfigs(FilterTheSpire2Config.TezcataraOptions, TezcataraOptions.Any, 2);
                break;
        }

        switch (FilterTheSpire2Config.Act3Ancients)
        {
            
            case Ancient.Nonupeipe:
                CheckAncientRelicConfigs(FilterTheSpire2Config.NonupeipeOptions, NonupeipeOptions.Any, 3);
                break;
            case Ancient.Tanx:
                CheckAncientRelicConfigs(FilterTheSpire2Config.TanxOptions, TanxOptions.Any, 3);
                break;
            case Ancient.Vakuu:
                CheckAncientRelicConfigs(FilterTheSpire2Config.VakuuOptions, VakuuOptions.Any, 3);
                break;
        }

        if (FilterTheSpire2Config.Act2Ancients == Ancient.Darv || FilterTheSpire2Config.Act3Ancients == Ancient.Darv)
        {
            CheckAncientRelicConfigs(FilterTheSpire2Config.DarvOptions, DarvOptions.Any, FilterTheSpire2Config.Act2Ancients == Ancient.Darv ? 2 : 3);
        }
    }

    private static void CheckAncientRelicConfigs<TEnum>(TEnum configVal, TEnum e, int actNum) where TEnum : Enum
    {
        if (configVal.Equals(e)) return;
        var filter = new AncientRelicFilter(RelicModelFactory.GetRelicModel(configVal)!, actNum);
        Filters.Add(filter);
    }
}