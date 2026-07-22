using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Events;
using Godot;
using MegaCrit.Sts2.addons.mega_text;

namespace FilterTheSpire2.Code.Config.Logic;

public static class EventConfigController
{
    private static readonly Dictionary<string, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<string, NConfigDropdown> EventDropdowns = new();

    private static readonly (string PropName, int Act)[] EventSlots =
    [
        (nameof(FilterTheSpire2Config.Act1FirstEvent), 1),
        (nameof(FilterTheSpire2Config.Act2FirstEvent), 2),
        (nameof(FilterTheSpire2Config.Act3FirstEvent), 3),
    ];

    public static void SetupEventDropdownConfig(Control optionContainer)
    {
        MasterItems.Clear();
        EventDropdowns.Clear();

        // Must run AFTER BossConfigController.SetupBossDropdownConfig. This re-wraps each act's
        // location dropdown items on top of Boss's existing wrap (calling its onSet first), so
        // changing location refreshes both boss and event dropdowns.
        for (var act = 1; act <= 3; act++)
        {
            WrapLocationDropdownForEvents(optionContainer, act);
        }

        foreach (var (propName, _) in EventSlots)
        {
            RegisterEventDropdown(optionContainer, propName);
        }

        foreach (var (propName, act) in EventSlots)
        {
            RefreshEventDropdown(propName, act);
        }
    }

    private static void WrapLocationDropdownForEvents(Control optionContainer, int act)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, $"Act{act}Locations");

        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in items)
        {
            var originalOnSet = item.OnSet; // already boss-wrapped by BossConfigController
            rebuilt.Add(new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
            {
                originalOnSet.Invoke();
                RefreshAllEventDropdownsForAct(act);
            }));
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }

    private static void RegisterEventDropdown(Control optionContainer, string propName)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, propName);

        EventDropdowns[propName] = dropdown;
        MasterItems[propName] = items.ToList();
    }

    private static void RefreshAllEventDropdownsForAct(int act)
    {
        foreach (var (propName, slotAct) in EventSlots)
        {
            if (slotAct == act)
            {
                RefreshEventDropdown(propName, slotAct);
            }
        }
    }

    private static void RefreshEventDropdown(string propName, int act)
    {
        if (!EventDropdowns.TryGetValue(propName, out var dropdown) || !MasterItems.TryGetValue(propName, out var source))
        {
            return;
        }

        var currentLocation = GetLocationForAct(act);
        var validEvents = EventRules.GetValidEvents(act, currentLocation).ToHashSet();

        var filtered = source
            .Where(item =>
            {
                var value = (EventOptions)item.Value!;
                return value == EventOptions.Any || validEvents.Contains(value);
            }).OrderBy(item => (EventOptions)item.Value! == EventOptions.Any ? 0 : 1)
            .ThenBy(item => ((EventOptions)item.Value!).ToString()).ToList();

        if (IsCurrentSelectionInvalid(propName, filtered))
        {
            ResetEventOption(propName, dropdown, filtered);
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, filtered);
    }

    private static ActLocations GetLocationForAct(int act) => act switch
    {
        1 => FilterTheSpire2Config.Act1Locations,
        2 => FilterTheSpire2Config.Act2Locations,
        3 => FilterTheSpire2Config.Act3Locations,
        _ => ActLocations.Any
    };

    private static EventOptions GetEventValue(string propName)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        return (EventOptions)property!.GetValue(null)!;
    }

    private static void SetEventValue(string propName, EventOptions value)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        property?.SetValue(null, value);
    }

    private static bool IsCurrentSelectionInvalid(string propName, List<NConfigDropdownItem.ItemData> validItems)
    {
        var currentValue = GetEventValue(propName);
        return currentValue != EventOptions.Any && validItems.All(i => !Equals(i.Value, currentValue));
    }

    private static void ResetEventOption(string propName, NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> filteredItems)
    {
        SetEventValue(propName, EventOptions.Any);

        var labelField = dropdown.GetType()
            .GetCachedField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
        var label = (MegaLabel?)labelField?.GetValue(dropdown);
        var anyItem = filteredItems.FirstOrDefault(i => Equals(i.Value, EventOptions.Any));
        label?.SetTextAutoSize(anyItem?.Text ?? "Any");

        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
}