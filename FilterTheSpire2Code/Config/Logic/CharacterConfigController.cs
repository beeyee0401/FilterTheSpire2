using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class CharacterConfigController
{
    private static CharacterOptions _currentCharacterSelection;
    private static readonly Dictionary<RelicRarity, NConfigDropdown> Dropdowns = new();
    private static readonly Dictionary<RelicRarity, List<NConfigDropdownItem.ItemData>> MasterItems = new();

    public static void SetupCharacterDropdownConfig(Control optionContainer)
    {
        _currentCharacterSelection = FilterTheSpire2Config.Character;
        RebuildCharacterDropdown(optionContainer);
        SetDropdownsFromCharacter(optionContainer, RelicRarity.Common, nameof(FilterTheSpire2Config.CommonRelic));
        SetDropdownsFromCharacter(optionContainer, RelicRarity.Uncommon, nameof(FilterTheSpire2Config.UncommonRelic));
        SetDropdownsFromCharacter(optionContainer, RelicRarity.Rare, nameof(FilterTheSpire2Config.RareRelic));
        SetDropdownsFromCharacter(optionContainer, RelicRarity.Shop, nameof(FilterTheSpire2Config.ShopRelic));
    }
    
    #region "Dropdowns dependent on character"
    private static void RegisterDropdown(RelicRarity relicRarity, NConfigDropdown dropdown)
    {
        Dropdowns[relicRarity] = dropdown;
    }
    
    private static void SetDropdownsFromCharacter(Control optionContainer, RelicRarity relicRarity, string propName)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, propName);
        
        RegisterDropdown(relicRarity, dropdown);
        
        if (!MasterItems.ContainsKey(relicRarity))
        {
            MasterItems[relicRarity] = items.ToList();
        }
        
        RebuildDropdownForCharacter(relicRarity, dropdown, GetRelicOption(relicRarity), false);
    }
    
    private static void SyncAllDropdowns(bool shouldCheckToReset)
    {
        foreach (var kvp in Dropdowns)
        {
            RebuildDropdownForCharacter(kvp.Key, kvp.Value, GetRelicOption(kvp.Key), shouldCheckToReset);
        }
    }
    
    private static void RebuildDropdownForCharacter(RelicRarity relicRarity, NConfigDropdown dropdown, RelicOptions dropdownSelection, bool shouldCheckToReset)
    {
        var source = MasterItems[relicRarity];

        var relicPool = RelicRules.GetRelicPool(relicRarity).ToList();
        
        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in source)
        {
            var value = (RelicOptions)item.Value!;

            if (value == RelicOptions.Any)
            {
                rebuilt.Add(item);
                continue;
            }

            if (!relicPool.Contains(value))
            {
                continue;
            }

            rebuilt.Add(item);
        }

        if (shouldCheckToReset)
        {
            var characterSpecificRelics = RelicRules.CharacterSpecificRelics.Values
                .SelectMany(c => c.Values)
                .SelectMany(r => r);
        
            // Update the currently selected option to any if it was a character specific relic but that character is not selected
            if (characterSpecificRelics.Contains(dropdownSelection))
            {
                var currentOptionLabelField = dropdown.GetType()
                    .GetField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
                var currentOptionLabel = (MegaLabel)currentOptionLabelField?.GetValue(dropdown)!;
                currentOptionLabel.SetTextAutoSize(nameof(RelicOptions.Any));
                ResetRelicOption(relicRarity);
            }
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }

    private static RelicOptions GetRelicOption(RelicRarity relicRarity)
    {
        switch (relicRarity)
        {
            case RelicRarity.Common:
                return FilterTheSpire2Config.CommonRelic;
            case RelicRarity.Uncommon:
                return FilterTheSpire2Config.UncommonRelic;
            case RelicRarity.Rare:
                return FilterTheSpire2Config.RareRelic;
            case RelicRarity.Shop:
                return FilterTheSpire2Config.ShopRelic;
            default:
                throw new ArgumentOutOfRangeException(nameof(relicRarity), relicRarity, null);
        }
    }
    
    private static void ResetRelicOption(RelicRarity relicRarity)
    {
        switch (relicRarity)
        {
            case RelicRarity.Common:
                FilterTheSpire2Config.CommonRelic = RelicOptions.Any;
                break;
            case RelicRarity.Uncommon:
                FilterTheSpire2Config.UncommonRelic = RelicOptions.Any;
                break;
            case RelicRarity.Rare:
                FilterTheSpire2Config.RareRelic = RelicOptions.Any;
                break;
            case RelicRarity.Shop:
                FilterTheSpire2Config.ShopRelic = RelicOptions.Any;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(relicRarity), relicRarity, null);
        }
        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
    
    #endregion
    
    #region "Character dropdown"
    private static Action WrapOnSet(CharacterOptions character, Action originalOnSet)
    {
        return () =>
        {
            originalOnSet.Invoke();
            
            var shouldCheckToResetRelicDropdowns = character != _currentCharacterSelection;
            _currentCharacterSelection = character;
            SyncAllDropdowns(shouldCheckToResetRelicDropdowns);
        };
    }
    
    private static void RebuildCharacterDropdown(Control optionContainer)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, nameof(FilterTheSpire2Config.Character));
        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in items)
        {
            var value = (CharacterOptions)item.Value!;
            var wrappedOnSet = WrapOnSet(value, item.OnSet);

            rebuilt.Add(new NConfigDropdownItem.ItemData(
                item.Text,
                item.Value,
                wrappedOnSet
            ));
        }
        
        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }
    #endregion
}