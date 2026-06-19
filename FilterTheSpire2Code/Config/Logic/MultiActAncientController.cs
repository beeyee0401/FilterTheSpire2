using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using Godot;
using MegaCrit.Sts2.addons.mega_text;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class MultiActAncientController
{
    private record AncientDefinition(
        string PropName,
        Func<int, HashSet<object>> GetOptionsForAct
    );

    // Add new multi-act ancients here — everything else is driven from this
    private static readonly Dictionary<Ancient, AncientDefinition> AncientDefinitions = new()
    {
        {
            Ancient.Darv,
            new AncientDefinition(
                nameof(FilterTheSpire2Config.DarvOptions),
                act => AncientRules.MultiActAncientsAndRelics[Ancient.Darv][act]
                    .Cast<object>().ToHashSet()
            )
        }
    };

    private static readonly Dictionary<Ancient, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<Ancient, NConfigDropdown> Dropdowns = new();
    private static readonly Dictionary<Ancient, NConfigOptionRow> OptionRows = new();
    private static readonly Dictionary<Ancient, ColorRect> OptionDividers = new();

    private static Control? _act2Container;
    private static Control? _act3Container;

    public static void SetupMultiActAncientConfig(Control optionContainer)
    {
        _act2Container = GetActContainer(optionContainer, nameof(FilterTheSpire2Config.Act2Ancient));
        _act3Container = GetActContainer(optionContainer, nameof(FilterTheSpire2Config.Act3Ancient));
        MasterItems.Clear();
        Dropdowns.Clear();
        OptionRows.Clear();
        OptionDividers.Clear();

        foreach (var (ancient, definition) in AncientDefinitions)
        {
            var act = GetActForAncient(ancient);
            if (act != 0)
            {
                BuildAndPlaceRow(optionContainer, ancient, definition, act);
            }
        }

        WrapActAncientDropdown(optionContainer, nameof(FilterTheSpire2Config.Act2Ancient));
        WrapActAncientDropdown(optionContainer, nameof(FilterTheSpire2Config.Act3Ancient));
    }

    private static Control? GetActContainer(Control optionContainer, string propName)
    {
        var row = optionContainer.GetNodeOrNull<NConfigOptionRow>("%" + propName);
        return row?.GetParent() as Control;
    }

    private static int GetActForAncient(Ancient ancient)
    {
        if (FilterTheSpire2Config.Act2Ancient == ancient)
        {
            return 2;
        }
        if (FilterTheSpire2Config.Act3Ancient == ancient)
        {
            return 3;
        }
        return 0;
    }

    private static Control? GetContainerForAct(int act) => act switch
    {
        2 => _act2Container,
        3 => _act3Container,
        _ => null
    };

    private static void WrapActAncientDropdown(Control optionContainer, string propName)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, propName);
        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in items)
        {
            var originalOnSet = item.OnSet;
            rebuilt.Add(new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
            {
                originalOnSet.Invoke();
                SyncAllAncientRows(optionContainer);
            }));
        }
        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }

    private static void SyncAllAncientRows(Control optionContainer)
    {
        foreach (var (ancient, definition) in AncientDefinitions)
        {
            var act = GetActForAncient(ancient);

            if (!OptionRows.TryGetValue(ancient, out var row))
            {
                if (act != 0)
                {
                    BuildAndPlaceRow(optionContainer, ancient, definition, act);
                }
                continue;
            }

            if (act == 0)
            {
                row.Visible = false;
                OptionDividers[ancient].Visible = false;
                continue;
            }

            PlaceRowInContainer(ancient, act);
            row.Visible = true;
            OptionDividers[ancient].Visible = true;
            RefreshDropdownForAct(ancient, definition, act);
        }
    }

    private static void BuildAndPlaceRow(Control _, Ancient ancient, AncientDefinition definition, int act)
    {
        var container = GetContainerForAct(act);
        if (container == null)
        {
            return;
        }

        var configInstance = ModConfigRegistry.Get<FilterTheSpire2Config>();
        if (configInstance == null)
        {
            return;
        }

        var row = configInstance.CreateHiddenOptionRow(definition.PropName, out var masterItems);
        MasterItems[ancient] = masterItems;

        var dropdown = ConfigDropdownUtilities.GetDropdownFromRow(row);
        if (dropdown == null)
        {
            return;
        }

        var filtered = FilterItems(definition, masterItems, act);

        if (IsCurrentValueInvalid(definition.PropName, filtered))
        {
            ResetOption(dropdown, definition.PropName, filtered);
        }

        ConfigDropdownUtilities.SeedItemsBeforeReady(dropdown, filtered);

        var divider = FilterTheSpire2Config.CreateCardDivider();
        container.AddChild(divider);
        container.AddChild(row);

        Dropdowns[ancient] = dropdown;
        OptionRows[ancient] = row;
        OptionDividers[ancient] = divider;
    }

    private static void PlaceRowInContainer(Ancient ancient, int act)
    {
        var targetContainer = GetContainerForAct(act);
        if (targetContainer == null)
        {
            return;
        }

        var row = OptionRows[ancient];
        var divider = OptionDividers[ancient];
        if (row.GetParent() == targetContainer)
        {
            return;
        }

        row.GetParent()?.RemoveChild(divider);
        row.GetParent()?.RemoveChild(row);
        targetContainer.AddChild(divider);
        targetContainer.AddChild(row);
    }

    private static void RefreshDropdownForAct(Ancient ancient, AncientDefinition definition, int act)
    {
        if (!Dropdowns.TryGetValue(ancient, out var dropdown)) return;

        var filtered = FilterItems(definition, MasterItems[ancient], act);

        if (IsCurrentValueInvalid(definition.PropName, filtered))
        {
            ResetOption(dropdown, definition.PropName, filtered);
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, filtered);
    }

    private static List<NConfigDropdownItem.ItemData> FilterItems(
        AncientDefinition definition,
        List<NConfigDropdownItem.ItemData> source,
        int act)
    {
        var validOptions = definition.GetOptionsForAct(act);
        return source.Where(item => validOptions.Contains(item.Value!)).ToList();
    }

    private static bool IsCurrentValueInvalid(string propName, List<NConfigDropdownItem.ItemData> validItems)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        var currentValue = property?.GetValue(null);
        return currentValue != null && validItems.All(i => !Equals(i.Value, currentValue));
    }

    private static void ResetOption(NConfigDropdown dropdown, string propName, List<NConfigDropdownItem.ItemData> filteredItems)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        if (property == null)
        {
            return;
        }

        var anyValue = Enum.GetNames(property.PropertyType)
            .Where(n => n == "Any")
            .Select(n => Enum.Parse(property.PropertyType, n))
            .FirstOrDefault();
        property.SetValue(null, anyValue ?? Enum.GetValues(property.PropertyType).GetValue(0));

        var labelField = dropdown.GetType()
            .GetCachedField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
        var label = (MegaLabel?)labelField?.GetValue(dropdown);
        var anyItem = filteredItems.FirstOrDefault(i => Equals(i.Value, anyValue));
        label?.SetTextAutoSize(anyItem?.Text ?? filteredItems.FirstOrDefault()?.Text ?? "Any");

        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
}