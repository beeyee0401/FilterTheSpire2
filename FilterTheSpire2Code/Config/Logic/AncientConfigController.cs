using System.Reflection;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using Godot;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class AncientConfigController
{
    private static readonly Dictionary<int, Ancient> CurrentSelection = new();
    private static readonly Dictionary<int, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<int, NConfigDropdown> Dropdowns = new();

    public static void SetupAncientDropdownConfig(Control optionContainer)
    {
        CurrentSelection[2] = FilterTheSpire2Config.Act2Ancient;
        CurrentSelection[3] = FilterTheSpire2Config.Act3Ancient;
        
        SetActSpecificAncients(optionContainer, 2, nameof(FilterTheSpire2Config.Act2Ancient));
        SetActSpecificAncients(optionContainer, 3, nameof(FilterTheSpire2Config.Act3Ancient));
    }
    
    private static void RegisterDropdown(int act, NConfigDropdown dropdown)
    {
        Dropdowns[act] = dropdown;
    }
    
    private static void SetActSpecificAncients(Control optionContainer, int act, string propName)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, propName);
        
        RegisterDropdown(act, dropdown);

        // store original master list once
        if (!MasterItems.ContainsKey(act))
        {
            MasterItems[act] = items.ToList();
        }

        RebuildDropdownForAct(act, dropdown, optionContainer);
    }
    
    private static Action WrapOnSet(int act, Ancient value, Action originalOnSet, Control optionContainer)
    {
        return () =>
        {
            originalOnSet.Invoke();

            // We do not need to refresh if a dropdown was not updated to or from a multi-act ancient
            var shouldUpdateAncientDropdowns = AncientRules.MultiActAncientsAndRelics.ContainsKey(CurrentSelection[act]);
            var shouldUpdateMultiActOptionsDropdown = AncientRules.MultiActAncientsAndRelics.ContainsKey(value);
            CurrentSelection[act] = value;

            if (shouldUpdateAncientDropdowns || shouldUpdateMultiActOptionsDropdown)
            {
                SyncAllDropdowns(optionContainer, shouldUpdateMultiActOptionsDropdown);
            }
        };
    }

    private static void SyncAllDropdowns(Control optionContainer, bool shouldUpdateMultiActOptionsDropdown)
    {
        foreach (var kvp in Dropdowns)
        {
            RebuildDropdownForAct(kvp.Key, kvp.Value, optionContainer);
        }

        if (shouldUpdateMultiActOptionsDropdown)
        {
            MultiActAncientController.UpdateMultiActAncientActSpecificRelics(optionContainer);
        }
    }
    
    private static void RebuildDropdownForAct(int act, NConfigDropdown dropdown, Control optionContainer)
    {
        var source = MasterItems[act];

        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in source)
        {
            var value = (Ancient)item.Value!;

            if (!AncientRules.IsValidForAct(act, value))
            {
                continue;
            }

            var takenElsewhere =
                AncientRules.MultiActAncientsAndRelics.ContainsKey(value)
                && CurrentSelection.Any(x => x.Key != act && x.Value == value);

            if (takenElsewhere)
            {
                continue;
            }

            var wrappedOnSet = WrapOnSet(act, value, item.OnSet, optionContainer);

            rebuilt.Add(new NConfigDropdownItem.ItemData(
                item.Text,
                item.Value,
                wrappedOnSet
            ));
        }
        
        // Update the currently selected index since the list items have shifted
        var currentDisplayIndexField = dropdown.GetType()
            .GetCachedField("_currentDisplayIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        var currentDisplayIndex = (int)currentDisplayIndexField?.GetValue(dropdown)!;
        if (currentDisplayIndex >= 0)
        {
            var selectedAncient = source[currentDisplayIndex].Value;
            var newIndex = rebuilt.FindIndex(i => i.Value == selectedAncient);
            currentDisplayIndexField.SetValue(dropdown, newIndex);
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }
}