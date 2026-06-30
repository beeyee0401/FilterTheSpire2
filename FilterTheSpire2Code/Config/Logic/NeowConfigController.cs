using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using Godot;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class NeowConfigController
{
    private static readonly Dictionary<string, NConfigOptionRow> OptionRows = new();
    private static readonly Dictionary<string, ColorRect> Dividers = new();
    private static Control? _optionContainer;
 
    private static readonly (string PropName, NeowOptions RequiredOption)[] NeowSubOptions =
    [
        (nameof(FilterTheSpire2Config.LeadPaperweightOption), NeowOptions.LeadPaperweight),
        (nameof(FilterTheSpire2Config.NeowsBonesRelicOption1), NeowOptions.NeowsBones),
        (nameof(FilterTheSpire2Config.NeowsBonesRelicOption2), NeowOptions.NeowsBones),
        (nameof(FilterTheSpire2Config.NeowsBonesCurseOption), NeowOptions.NeowsBones),
    ];
    
    // Propnames for the bones relic selectors that affect card outcome row visibility
    private static readonly HashSet<string> BonesRelicOptionPropNames =
    [
        nameof(FilterTheSpire2Config.NeowsBonesRelicOption1),
        nameof(FilterTheSpire2Config.NeowsBonesRelicOption2),
    ];
 
    public static void SetupNeowDropdownConfig(Control optionContainer)
    {
        _optionContainer = optionContainer;
        OptionRows.Clear();
        Dividers.Clear();
 
        WrapNeowOptionsDropdown(optionContainer);
        EnsureSubOptionRows(optionContainer);
    }
 
    private static void WrapNeowOptionsDropdown(Control optionContainer)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, nameof(FilterTheSpire2Config.NeowOptions));
        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in items)
        {
            var originalOnSet = item.OnSet;
            rebuilt.Add(new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
            {
                originalOnSet.Invoke();
                EnsureSubOptionRows(optionContainer);
            }));
        }
        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }
 
    private static void EnsureSubOptionRows(Control optionContainer)
    {
        var currentNeow = FilterTheSpire2Config.NeowOptions;
 
        foreach (var (propName, requiredOption) in NeowSubOptions)
        {
            var isRelevant = currentNeow == requiredOption;
 
            if (!OptionRows.TryGetValue(propName, out var row))
            {
                if (isRelevant)
                {
                    BuildSubOptionRow(optionContainer, propName);
                }
                continue;
            }
 
            row.Visible = isRelevant;
            if (Dividers.TryGetValue(propName, out var divider))
            {
                divider.Visible = isRelevant;
            }
        }
 
        SimpleModConfig.SetupFocusNeighbors(optionContainer);
    }

    private static Control? GetNeowSectionContainer(Control optionContainer)
    {
        var siblingRow = optionContainer.GetNodeOrNull<NConfigOptionRow>("%" + nameof(FilterTheSpire2Config.NeowOptions));
        return siblingRow?.GetParent() as Control;
    }

    private static void BuildSubOptionRow(Control optionContainer, string propName)
    {
        var container = GetNeowSectionContainer(optionContainer);
        if (container == null)
        {
            return;
        }

        var configInstance = ModConfigRegistry.Get<FilterTheSpire2Config>();
        if (configInstance == null)
        {
            return;
        }

        var row = configInstance.CreateHiddenOptionRow(propName, out var masterItems);

        var dropdown = ConfigDropdownUtilities.GetDropdownFromRow(row);
        if (dropdown == null)
        {
            return;
        }

        var filtered = FilterItems(propName, masterItems);

        if (!filtered.Any(i => Equals(i.Value, GetCurrentValue(propName))))
        {
            ResetOption(propName);
        }

        // If this is a bones relic option, wrap it to refresh card outcome rows when changed
        if (BonesRelicOptionPropNames.Contains(propName))
        {
            filtered = WrapBonesRelicItems(filtered, optionContainer);
        }
        
        ConfigDropdownUtilities.SeedItemsBeforeReady(dropdown, filtered);
        
        var divider = FilterTheSpire2Config.CreateCardDivider();
        container.AddChild(divider);
        container.AddChild(row);

        // Move the divider+row to just after the last known Neow bones row,
        // or just after the NeowOptions row if none exist yet — so bones rows
        // always sit above card outcome rows.
        InsertAfterBonesAnchor(container, divider, row);
        
        Dividers[propName] = divider;
        OptionRows[propName] = row;
    }
    
    private static List<NConfigDropdownItem.ItemData> WrapBonesRelicItems(
        List<NConfigDropdownItem.ItemData> items,
        Control optionContainer)
    {
        return items.Select(item =>
        {
            var originalOnSet = item.OnSet;
            return new NConfigDropdownItem.ItemData(item.Text, item.Value, () =>
            {
                originalOnSet.Invoke();
                CharacterConfigController.RefreshCardRows(optionContainer);
            });
        }).ToList();
    }
    
    /// <summary>
    /// Moves the divider and row to appear after the last already-placed Neow bones row
    /// (or after the NeowOptions row itself if no bones rows exist yet), ensuring bones
    /// rows always sit above any card outcome rows appended later.
    /// </summary>
    private static void InsertAfterBonesAnchor(Control container, ColorRect divider, NConfigOptionRow row)
    {
        // Find the last bones row already in the container, falling back to the NeowOptions row
        Node? anchor = null;
        foreach (var (_, existingRow) in OptionRows)
        {
            if (existingRow.GetParent() == container)
            {
                anchor = existingRow;
            }
        }

        if (anchor == null)
        {
            // No bones rows yet — find the NeowOptions row as the anchor
            anchor = container.GetChildren()
                .OfType<NConfigOptionRow>()
                .FirstOrDefault(r => r.Name == nameof(FilterTheSpire2Config.NeowOptions));
        }

        if (anchor == null) { return; }

        var anchorIndex = anchor.GetIndex();
        container.MoveChild(divider, anchorIndex + 1);
        container.MoveChild(row, anchorIndex + 2);
    }
 
    private static List<NConfigDropdownItem.ItemData> FilterItems(string propName, List<NConfigDropdownItem.ItemData> source)
    {
        return propName switch
        {
            nameof(FilterTheSpire2Config.LeadPaperweightOption) => FilterColorlessCards(source),
            nameof(FilterTheSpire2Config.NeowsBonesRelicOption1) or
            nameof(FilterTheSpire2Config.NeowsBonesRelicOption2) => FilterNeowsBoneOptions(source),
            nameof(FilterTheSpire2Config.NeowsBonesCurseOption) => FilterCurseCards(source),
            _ => source
        };
    }
 
    private static List<NConfigDropdownItem.ItemData> FilterColorlessCards(List<NConfigDropdownItem.ItemData> source)
    {
        var cardPool = CardRules.AvailableCardPools[CharacterOptions.Any];
        return source.Where(item =>
        {
            var value = (CardOptions)item.Value!;
            return value == CardOptions.Any || cardPool.Contains(value);
        }).ToList();
    }
 
    private static List<NConfigDropdownItem.ItemData> FilterNeowsBoneOptions(List<NConfigDropdownItem.ItemData> source)
    {
        return source.Where(item =>
        {
            var value = (NeowOptions)item.Value!;
            return value == NeowOptions.Any || AncientRules.NeowsBonesOptions.Contains(value);
        }).ToList();
    }
 
    private static List<NConfigDropdownItem.ItemData> FilterCurseCards(List<NConfigDropdownItem.ItemData> source)
    {
        var cursePool = CardRules.CursePool;
        return source.Where(item =>
        {
            var value = (CardOptions)item.Value!;
            return value == CardOptions.Any || cursePool.Contains(value);
        }).ToList();
    }
 
    private static object? GetCurrentValue(string propName)
    {
        var property = typeof(FilterTheSpire2Config)
            .GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        return property?.GetValue(null);
    }
 
    private static void ResetOption(string propName)
    {
        var property = typeof(FilterTheSpire2Config)
            .GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        if (property == null) return;
 
        var defaultValue = property.PropertyType == typeof(CardOptions)
            ? CardOptions.Any
            : (object)NeowOptions.Any;
 
        property.SetValue(null, defaultValue);
        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
}