using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.Code.Acts;
using Godot;
using MegaCrit.Sts2.addons.mega_text;

namespace FilterTheSpire2.Code.Config.Logic;

public static class BossConfigController
{
    private static readonly Dictionary<int, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<int, NConfigDropdown> BossDropdowns = new();

    public static void SetupBossDropdownConfig(Control optionContainer)
    {
        MasterItems.Clear();
        BossDropdowns.Clear();

        for (var act = 1; act <= 3; act++)
        {
            RegisterBossDropdown(optionContainer, act);
            WrapLocationDropdown(optionContainer, act);
            RefreshBossDropdown(act);
        }
    }

    private static void RegisterBossDropdown(Control optionContainer, int act)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, $"Act{act}Boss");

        BossDropdowns[act] = dropdown;
        MasterItems[act] = items.ToList();
    }

    private static void WrapLocationDropdown(Control optionContainer, int act)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, $"Act{act}Locations");

        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in items)
        {
            var originalOnSet = item.OnSet;
            rebuilt.Add(new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
            {
                originalOnSet.Invoke();
                RefreshBossDropdown(act);
            }));
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }

    private static void RefreshBossDropdown(int act)
    {
        if (!BossDropdowns.TryGetValue(act, out var dropdown) || !MasterItems.TryGetValue(act, out var source))
        {
            return;
        }

        var currentLocation = GetLocationForAct(act);
        var validBosses = BossRules.GetValidBosses(act, currentLocation).ToHashSet();

        var filtered = source.Where(item =>
        {
            var value = (BossOptions)item.Value!;
            return value == BossOptions.Any || validBosses.Contains(value);
        }).ToList();

        if (IsCurrentSelectionInvalid(act, filtered))
        {
            ResetBossOption(act, dropdown, filtered);
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

    private static BossOptions GetBossForAct(int act) => act switch
    {
        1 => FilterTheSpire2Config.Act1Boss,
        2 => FilterTheSpire2Config.Act2Boss,
        3 => FilterTheSpire2Config.Act3Boss,
        _ => BossOptions.Any
    };

    private static void SetBossForAct(int act, BossOptions value)
    {
        switch (act)
        {
            case 1: FilterTheSpire2Config.Act1Boss = value; break;
            case 2: FilterTheSpire2Config.Act2Boss = value; break;
            case 3: FilterTheSpire2Config.Act3Boss = value; break;
        }
    }

    private static bool IsCurrentSelectionInvalid(int act, List<NConfigDropdownItem.ItemData> validItems)
    {
        var currentValue = GetBossForAct(act);
        return currentValue != BossOptions.Any && validItems.All(i => !Equals(i.Value, currentValue));
    }

    private static void ResetBossOption(int act, NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> filteredItems)
    {
        SetBossForAct(act, BossOptions.Any);

        var labelField = dropdown.GetType()
            .GetCachedField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
        var label = (MegaLabel?)labelField?.GetValue(dropdown);
        var anyItem = filteredItems.FirstOrDefault(i => Equals(i.Value, BossOptions.Any));
        label?.SetTextAutoSize(anyItem?.Text ?? "Any");

        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
}