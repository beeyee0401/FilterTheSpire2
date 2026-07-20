using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.Code.Acts;
using Godot;
using MegaCrit.Sts2.addons.mega_text;

namespace FilterTheSpire2.Code.Config.Logic;

public static class BossConfigController
{
    private static readonly Dictionary<string, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<string, NConfigDropdown> BossDropdowns = new();
    private static readonly Dictionary<int, NConfigDropdown> LocationDropdowns = new();

    // Every boss dropdown, the act it belongs to, and (if any) the sibling property it can't share
    // a value with.
    private static readonly (string PropName, int Act, string? SiblingPropName)[] BossSlots =
    [
        (nameof(FilterTheSpire2Config.Act1Boss), 1, null),
        (nameof(FilterTheSpire2Config.Act2Boss), 2, null),
        (nameof(FilterTheSpire2Config.Act3FirstBoss), 3, nameof(FilterTheSpire2Config.Act3SecondBoss)),
        (nameof(FilterTheSpire2Config.Act3SecondBoss), 3, nameof(FilterTheSpire2Config.Act3FirstBoss)),
    ];

    public static void SetupBossDropdownConfig(Control optionContainer)
    {
        MasterItems.Clear();
        BossDropdowns.Clear();
        LocationDropdowns.Clear();

        for (var act = 1; act <= 3; act++)
        {
            SetupLocationDropdown(optionContainer, act);
        }

        foreach (var (propName, _, _) in BossSlots)
        {
            RegisterBossDropdown(optionContainer, propName);
        }

        foreach (var (propName, act, siblingPropName) in BossSlots)
        {
            RefreshBossDropdown(propName, act, siblingPropName);
        }
    }

    private static void SetupLocationDropdown(Control optionContainer, int act)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, $"Act{act}Locations");

        LocationDropdowns[act] = dropdown;

        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in items)
        {
            var originalOnSet = item.OnSet;
            rebuilt.Add(new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
            {
                originalOnSet.Invoke();
                RefreshAllBossDropdownsForAct(act);
            }));
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }

    private static void RegisterBossDropdown(Control optionContainer, string propName)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, propName);

        BossDropdowns[propName] = dropdown;
        MasterItems[propName] = items.ToList();
    }

    private static void RefreshAllBossDropdownsForAct(int act)
    {
        foreach (var (propName, slotAct, siblingPropName) in BossSlots)
        {
            if (slotAct == act)
            {
                RefreshBossDropdown(propName, slotAct, siblingPropName);
            }
        }
    }

    private static void RefreshBossDropdown(string propName, int act, string? siblingPropName)
    {
        if (!BossDropdowns.TryGetValue(propName, out var dropdown) || !MasterItems.TryGetValue(propName, out var source))
        {
            return;
        }

        var currentLocation = GetLocationForAct(act);
        var validBosses = BossRules.GetValidBosses(act, currentLocation).ToHashSet();
        var siblingValue = siblingPropName != null ? GetBossValue(siblingPropName) : BossOptions.Any;

        var filtered = source
            .Where(item =>
            {
                var value = (BossOptions)item.Value!;
                if (value == BossOptions.Any) return true;
                if (!validBosses.Contains(value)) return false;
                if (siblingValue != BossOptions.Any && value == siblingValue) return false;
                return true;
            })
            .Select(item => WrapBossItem(item, propName, act, siblingPropName))
            .ToList();

        if (IsCurrentSelectionInvalid(propName, filtered))
        {
            ResetBossOption(propName, dropdown, filtered);
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, filtered);
    }

    private static NConfigDropdownItem.ItemData WrapBossItem(
        NConfigDropdownItem.ItemData item,
        string propName,
        int act,
        string? siblingPropName)
    {
        var originalOnSet = item.OnSet;
        return new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
        {
            originalOnSet.Invoke();

            var selected = (BossOptions)item.Value!;
            SyncLocationToBoss(act, selected);

            // Re-filter the sibling slot so it can no longer target the same boss (e.g. Act 3
            // first/second boss can't both be set to the same value).
            if (siblingPropName != null)
            {
                RefreshBossDropdown(siblingPropName, act, propName);
            }
        });
    }

    private static void SyncLocationToBoss(int act, BossOptions selectedBoss)
    {
        if (selectedBoss == BossOptions.Any)
        {
            return;
        }

        var requiredLocation = BossRules.GetLocationForBoss(selectedBoss);
        if (requiredLocation == ActLocations.Any || GetLocationForAct(act) == requiredLocation)
        {
            return;
        }

        SetLocationForAct(act, requiredLocation);

        if (LocationDropdowns.TryGetValue(act, out var locationDropdown))
        {
            locationDropdown.SetFromProperty();
        }

        RefreshAllBossDropdownsForAct(act);
        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }

    private static ActLocations GetLocationForAct(int act) => act switch
    {
        1 => FilterTheSpire2Config.Act1Locations,
        2 => FilterTheSpire2Config.Act2Locations,
        3 => FilterTheSpire2Config.Act3Locations,
        _ => ActLocations.Any
    };

    private static void SetLocationForAct(int act, ActLocations value)
    {
        switch (act)
        {
            case 1:
            {
                FilterTheSpire2Config.Act1Locations = value;
                break;
            }
            case 2:
            {
                FilterTheSpire2Config.Act2Locations = value;
                break;
            }
            case 3:
            {
                FilterTheSpire2Config.Act3Locations = value;
                break;
            }
        }
    }

    private static BossOptions GetBossValue(string propName)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        return (BossOptions)property!.GetValue(null)!;
    }

    private static void SetBossValue(string propName, BossOptions value)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        property?.SetValue(null, value);
    }

    private static bool IsCurrentSelectionInvalid(string propName, List<NConfigDropdownItem.ItemData> validItems)
    {
        var currentValue = GetBossValue(propName);
        return currentValue != BossOptions.Any && validItems.All(i => !Equals(i.Value, currentValue));
    }

    private static void ResetBossOption(string propName, NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> filteredItems)
    {
        SetBossValue(propName, BossOptions.Any);

        var labelField = dropdown.GetType()
            .GetCachedField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
        var label = (MegaLabel?)labelField?.GetValue(dropdown);
        var anyItem = filteredItems.FirstOrDefault(i => Equals(i.Value, BossOptions.Any));
        label?.SetTextAutoSize(anyItem?.Text ?? "Any");

        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
}