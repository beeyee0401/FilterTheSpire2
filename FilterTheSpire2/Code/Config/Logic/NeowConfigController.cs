using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.Code.Config.Logic;

public static class NeowConfigController
{
    private static readonly Dictionary<string, NConfigDropdown> Dropdowns = new();
    private static readonly Dictionary<string, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<string, NConfigOptionRow> OptionRows = new();
    private static readonly Dictionary<string, ColorRect> Dividers = new();
    private static Control? _optionContainer;
 
    private static readonly (string PropName, NeowOptions RequiredOption)[] NeowSubOptions =
    [
        (nameof(FilterTheSpire2Config.NeowsBonesRelicOption1), NeowOptions.NeowsBones),
        (nameof(FilterTheSpire2Config.NeowsBonesRelicOption2), NeowOptions.NeowsBones),
        (nameof(FilterTheSpire2Config.NeowsBonesCurseOption), NeowOptions.NeowsBones),

        (nameof(FilterTheSpire2Config.CapsuleRelicOption1), NeowOptions.SmallCapsule),
        (nameof(FilterTheSpire2Config.CapsuleRelicOption2), NeowOptions.LargeCapsule),
        (nameof(FilterTheSpire2Config.CapsuleRelicOption3), NeowOptions.LargeCapsule),

        (nameof(FilterTheSpire2Config.LeadPaperweightOption), NeowOptions.LeadPaperweight),
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
        Dropdowns.Clear();
        MasterItems.Clear();
 
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
        foreach (var (propName, requiredOption) in NeowSubOptions)
        {
            var isRelevant = IsSubOptionRelevant(propName, requiredOption);

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

            if (isRelevant && Dropdowns.TryGetValue(propName, out var dropdown))
            {
                var filtered = FilterItems(propName, MasterItems[propName]);

                if (BonesRelicOptionPropNames.Contains(propName))
                {
                    filtered = WrapBonesRelicItems(filtered, optionContainer);
                }

                if (!filtered.Any(i => Equals(i.Value, GetCurrentValue(propName))))
                {
                    ResetOption(propName);
                }

                ConfigDropdownUtilities.RefreshDropdownItems(dropdown, filtered);
            }
        }

        SimpleModConfig.SetupFocusNeighbors(optionContainer);
    }
    
    private static bool IsSubOptionRelevant(string propName, NeowOptions requiredOption)
    {
        var currentNeow = FilterTheSpire2Config.NeowOptions;
        var capsuleRelicCount = GetVisibleCapsuleRelicCount();

        if (propName == nameof(FilterTheSpire2Config.CapsuleRelicOption1))
        {
            return capsuleRelicCount >= 1;
        }

        if (propName == nameof(FilterTheSpire2Config.CapsuleRelicOption2))
        {
            return capsuleRelicCount >= 2;
        }

        if (propName == nameof(FilterTheSpire2Config.CapsuleRelicOption3))
        {
            return capsuleRelicCount >= 3;
        }

        if (currentNeow == requiredOption)
        {
            return true;
        }

        if (propName == nameof(FilterTheSpire2Config.LeadPaperweightOption) &&
            currentNeow == NeowOptions.NeowsBones)
        {
            return FilterTheSpire2Config.NeowsBonesRelicOption1 == NeowOptions.LeadPaperweight ||
                   FilterTheSpire2Config.NeowsBonesRelicOption2 == NeowOptions.LeadPaperweight;
        }

        return false;
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
        
        Dropdowns[propName] = dropdown;
        MasterItems[propName] = masterItems;

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
        InsertNeowSubOptionRow(container, propName, divider, row);
        
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

                EnsureSubOptionRows(optionContainer);
                CharacterConfigController.RefreshRelicRows(optionContainer);
                CharacterConfigController.RefreshCardRows(optionContainer);
            });
        }).ToList();
    }
    
    private static int GetNeowRowOrder(string propName)
    {
        return propName switch
        {
            nameof(FilterTheSpire2Config.NeowsBonesRelicOption1) => 0,
            nameof(FilterTheSpire2Config.NeowsBonesRelicOption2) => 1,
            nameof(FilterTheSpire2Config.NeowsBonesCurseOption) => 2,
            nameof(FilterTheSpire2Config.CapsuleRelicOption1) => 3,
            nameof(FilterTheSpire2Config.CapsuleRelicOption2) => 4,
            nameof(FilterTheSpire2Config.CapsuleRelicOption3) => 5,
            nameof(FilterTheSpire2Config.LeadPaperweightOption) => 6,
            _ => 100,
        };
    }

    private static void InsertNeowSubOptionRow(
        Control container,
        string propName,
        ColorRect divider,
        NConfigOptionRow row)
    {
        var myOrder = GetNeowRowOrder(propName);

        Node? anchor = OptionRows
            .Where(kvp =>
                kvp.Value.GetParent() == container &&
                kvp.Value.Visible &&
                GetNeowRowOrder(kvp.Key) < myOrder)
            .OrderBy(kvp => GetNeowRowOrder(kvp.Key))
            .LastOrDefault()
            .Value;

        anchor ??= container.GetChildren()
            .OfType<NConfigOptionRow>()
            .FirstOrDefault(r => r.Name == nameof(FilterTheSpire2Config.NeowOptions));

        if (anchor == null)
        {
            return;
        }

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
                nameof(FilterTheSpire2Config.NeowsBonesRelicOption2) => FilterNeowsBoneOptions(propName, source),

            nameof(FilterTheSpire2Config.NeowsBonesCurseOption) => FilterCurseCards(source),

            nameof(FilterTheSpire2Config.CapsuleRelicOption1) or 
                nameof(FilterTheSpire2Config.CapsuleRelicOption2) or
                nameof(FilterTheSpire2Config.CapsuleRelicOption3) => FilterCapsuleRelics(source),

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
 
    private static List<NConfigDropdownItem.ItemData> FilterNeowsBoneOptions(
        string propName,
        List<NConfigDropdownItem.ItemData> source)
    {
        var otherSelected = propName == nameof(FilterTheSpire2Config.NeowsBonesRelicOption1)
            ? FilterTheSpire2Config.NeowsBonesRelicOption2
            : FilterTheSpire2Config.NeowsBonesRelicOption1;

        return source.Where(item =>
        {
            var value = (NeowOptions)item.Value!;

            if (value == NeowOptions.Any)
            {
                return true;
            }

            if (!AncientRules.NeowsBonesOptions.Contains(value))
            {
                return false;
            }

            return otherSelected == NeowOptions.Any || value != otherSelected;
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
        if (property == null)
        {
            return;
        }

        object defaultValue =
            property.PropertyType == typeof(CardOptions) ? CardOptions.Any :
            property.PropertyType == typeof(RelicOptions) ? RelicOptions.Any :
            NeowOptions.Any;

        property.SetValue(null, defaultValue);
        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
    
    private static List<NConfigDropdownItem.ItemData> FilterCapsuleRelics(List<NConfigDropdownItem.ItemData> source)
    {
        var capsulePool = RelicRules.GetRelicDisplayPool(RelicRarity.Common)
            .Concat(RelicRules.GetRelicDisplayPool(RelicRarity.Uncommon))
            .Concat(RelicRules.GetRelicDisplayPool(RelicRarity.Rare))
            .ToHashSet();

        return source.Where(item =>
        {
            var value = (RelicOptions)item.Value!;
            return value == RelicOptions.Any || capsulePool.Contains(value);
        }).ToList();
    }
    
    private static int GetVisibleCapsuleRelicCount()
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.NeowsBones)
        {
            return FilterTheSpire2Config.NeowOptions switch
            {
                NeowOptions.SmallCapsule => 1,
                NeowOptions.LargeCapsule => 2,
                _ => 0
            };
        }

        return FilterManager.GetCapsuleRelicCount(FilterTheSpire2Config.NeowsBonesRelicOption1) +
               FilterManager.GetCapsuleRelicCount(FilterTheSpire2Config.NeowsBonesRelicOption2);
    }
    
    public static void RefreshSubOptionRows(Control optionContainer)
    {
        EnsureSubOptionRows(optionContainer);
    }
}