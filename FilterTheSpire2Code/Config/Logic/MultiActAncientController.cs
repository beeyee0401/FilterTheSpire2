using System.Reflection;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.Ancients;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class MultiActAncientController
{
    private static readonly Dictionary<Ancient, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    
    public static void UpdateMultiActAncientActSpecificRelics(Control optionContainer)
    {
        var newRelicList = new List<DarvOptions>();
        if (FilterTheSpire2Config.Act2Ancient == Ancient.Darv)
        {
            newRelicList = AncientRules.MultiActAncientsAndRelics[Ancient.Darv][2];
        } 
        else if (FilterTheSpire2Config.Act3Ancient == Ancient.Darv)
        {
            newRelicList = AncientRules.MultiActAncientsAndRelics[Ancient.Darv][3];
        }
        
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, nameof(FilterTheSpire2Config.DarvOptions));
        if (!MasterItems.ContainsKey(Ancient.Darv))
            MasterItems[Ancient.Darv] = items.ToList();
        
        var source = MasterItems[Ancient.Darv];
        var newItems =  new List<NConfigDropdownItem.ItemData>();
        foreach (var item in source)
        {
            var option =  (DarvOptions)item.Value!;
            if (newRelicList.Contains(option))
            {
                newItems.Add(item);
            }
        }
        
        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, newItems);
    }
}