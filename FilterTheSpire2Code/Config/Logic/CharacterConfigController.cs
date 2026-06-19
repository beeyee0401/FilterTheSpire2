using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class CharacterConfigController
{
    private static CharacterOptions _currentCharacterSelection;
    private static readonly Dictionary<RelicRarity, NConfigDropdown> RelicDropdowns = new();
    private static readonly Dictionary<RelicRarity, List<NConfigDropdownItem.ItemData>> RelicMasterItems = new();
    private static readonly Dictionary<string, ColorRect> CardDividers = new();
    
    private static Control? _optionContainer;
    private static Control? _cardSectionContainer;
    private static readonly Dictionary<string, NConfigDropdown> CardDropdowns = new();
    private static readonly Dictionary<string, NConfigOptionRow> CardRows = new();
    private static readonly Dictionary<string, List<NConfigDropdownItem.ItemData>> CardMasterItems = new();
    private static readonly Dictionary<string, bool> CardRowVisibility = new();
    
    private readonly struct CharacterOnSetHandler(
        CharacterOptions character,
        Action originalOnSet)
    {
        public void Invoke()
        {
            originalOnSet.Invoke();
            var characterChanged = character != _currentCharacterSelection;
            _currentCharacterSelection = character;
            SyncAllDropdowns(characterChanged);
        }
    }
    
    public static void SetupCharacterDropdownConfig(Control optionContainer)
    {
        _optionContainer = optionContainer;
        _cardSectionContainer = null;
        CardDropdowns.Clear();
        CardRows.Clear();
        CardRowVisibility.Clear();
        CardDividers.Clear();
        
        _currentCharacterSelection = FilterTheSpire2Config.Character;
        RebuildCharacterDropdown(optionContainer);
        WrapNeowOptionsDropdown(optionContainer);

        SetRelicDropdownsFromCharacter(optionContainer, RelicRarity.Common, nameof(FilterTheSpire2Config.CommonRelic));
        SetRelicDropdownsFromCharacter(optionContainer, RelicRarity.Uncommon, nameof(FilterTheSpire2Config.UncommonRelic));
        SetRelicDropdownsFromCharacter(optionContainer, RelicRarity.Rare, nameof(FilterTheSpire2Config.RareRelic));
        SetRelicDropdownsFromCharacter(optionContainer, RelicRarity.Shop, nameof(FilterTheSpire2Config.ShopRelic));

        EnsureCardRows(optionContainer, characterChanged: false);
    }
    
    #region Relic dropdowns
    private static void RegisterDropdown(RelicRarity relicRarity, NConfigDropdown dropdown)
    {
        RelicDropdowns[relicRarity] = dropdown;
    }
    
    private static void SetRelicDropdownsFromCharacter(Control optionContainer, RelicRarity relicRarity, string propName)
    {
        var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, propName);
        
        RegisterDropdown(relicRarity, dropdown);
        
        if (!RelicMasterItems.ContainsKey(relicRarity))
        {
            RelicMasterItems[relicRarity] = items.ToList();
        }
        
        RebuildDropdownForCharacter(relicRarity, dropdown, GetRelicOption(relicRarity), false);
    }
    
    private static void RebuildDropdownForCharacter(RelicRarity relicRarity, NConfigDropdown dropdown, RelicOptions dropdownSelection, bool shouldCheckToReset)
    {
        var source = RelicMasterItems[relicRarity];

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
                    .GetCachedField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
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
    
    #region Dependency dropdowns
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
                EnsureCardRows(optionContainer, characterChanged: false);
            }));
        }
        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
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
    
    private static Action WrapOnSet(CharacterOptions character, Action originalOnSet)
    {
        var handler = new CharacterOnSetHandler(character, originalOnSet);
        return handler.Invoke;
    }
    #endregion
    
    #region Card dropdowns
    private static readonly (string PropName, Func<bool> ShouldShow)[] CardSlots =
    [
        (nameof(FilterTheSpire2Config.LeafyPoulticeOption1), FilterTheSpire2Config.ShouldShowLeafyPoulticeOptions),
        (nameof(FilterTheSpire2Config.LeafyPoulticeOption2), FilterTheSpire2Config.ShouldShowLeafyPoulticeOptions),
        (nameof(FilterTheSpire2Config.NewLeafOption), FilterTheSpire2Config.ShouldShowNewLeafOptions),
        (nameof(FilterTheSpire2Config.LostCofferOption), FilterTheSpire2Config.ShouldShowLostCofferOptions),
        (nameof(FilterTheSpire2Config.KaleidoscopeOption1), FilterTheSpire2Config.ShouldShowKaleidoscopeOptions),
        (nameof(FilterTheSpire2Config.KaleidoscopeOption2), FilterTheSpire2Config.ShouldShowKaleidoscopeOptions)
    ];

    private static void SyncAllDropdowns(bool shouldCheckToReset)
    {
        foreach (var kvp in RelicDropdowns)
        {
            RebuildDropdownForCharacter(kvp.Key, kvp.Value, GetRelicOption(kvp.Key), shouldCheckToReset);
        }

        EnsureCardRows(_optionContainer!, shouldCheckToReset);
    }
    
    private static void ResetCardOptions(string propName)
    {
        var property = typeof(FilterTheSpire2Config)
            .GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);

        if (property == null)
        {
            throw new InvalidOperationException($"Property '{propName}' not found.");
        }

        property.SetValue(null, CardOptions.Any);
        ModConfig.SaveDebounced<FilterTheSpire2Config>();
    }
    
    private static List<CardOptions> GetCardPoolForProperty(string propName)
    {
        if (propName is nameof(FilterTheSpire2Config.KaleidoscopeOption1) or 
            nameof(FilterTheSpire2Config.KaleidoscopeOption2))
        {
            return CardRules.AvailableCardPools
                .Where(kvp => kvp.Key != _currentCharacterSelection && kvp.Key != CharacterOptions.Any)
                .SelectMany(kvp => kvp.Value)
                .ToList();
        }

        return CardRules.AvailableCardPools[_currentCharacterSelection].ToList();
    }
    
    private static void EnsureCardRows(Control optionContainer, bool characterChanged)
    {
        var container = GetCardSectionContainer(optionContainer);
        if (container == null)
        {
            return;
        }

        foreach (var (propName, shouldShow) in CardSlots)
        {
            var isRelevant = shouldShow();
            var wasRelevant = CardRowVisibility.GetValueOrDefault(propName);
            CardRowVisibility[propName] = isRelevant;

            if (!CardDropdowns.TryGetValue(propName, out var dropdown))
            {
                if (isRelevant)
                {
                    BuildCardRow(container, propName);
                }
                continue;
            }

            CardRows[propName].Visible = isRelevant;
            if (CardDividers.TryGetValue(propName, out var divider))
            {
                divider.Visible = isRelevant;
            }

            if (isRelevant && (characterChanged || !wasRelevant))
            {
                RebuildCardDropdownForCharacter(propName, dropdown, shouldCheckToReset: true);
            }
        }
    }

    private static Control? GetCardSectionContainer(Control optionContainer)
    {
        if (_cardSectionContainer != null)
        {
            return _cardSectionContainer;
        }
        var siblingRow = optionContainer.GetNodeOrNull<NConfigOptionRow>("%" + nameof(FilterTheSpire2Config.LeadPaperweightOption));
        _cardSectionContainer = siblingRow?.GetParent() as Control;
        return _cardSectionContainer;
    }

    private static void BuildCardRow(Control container, string propName)
    {
        var configInstance = ModConfigRegistry.Get<FilterTheSpire2Config>();
        if (configInstance == null)
        {
            return;
        }

        var row = configInstance.CreateHiddenOptionRow(propName, out var masterItems);
        CardMasterItems.TryAdd(propName, masterItems);

        var dropdown = ConfigDropdownUtilities.GetDropdownFromRow(row);
        if (dropdown == null)
        {
            return;
        }

        var filtered = FilterCardItems(propName, CardMasterItems[propName]);

        if (!filtered.Any(i => Equals((CardOptions)i.Value!, GetCardOptionValue(propName))))
        {
            ResetCardOptions(propName); // clears a stale value from an earlier session/character before it's ever displayed
        }

        ConfigDropdownUtilities.SeedItemsBeforeReady(dropdown, filtered);
        var divider = FilterTheSpire2Config.CreateCardDivider();
        container.AddChild(divider);
        container.AddChild(row);

        CardDividers[propName] = divider;
        CardDropdowns[propName] = dropdown;
        CardRows[propName] = row;
    }

    private static CardOptions GetCardOptionValue(string propName)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propName, BindingFlags.Public | BindingFlags.Static);
        return (CardOptions)property!.GetValue(null)!;
    }

    private static List<NConfigDropdownItem.ItemData> FilterCardItems(string propName, List<NConfigDropdownItem.ItemData> source)
    {
        var cardPool = GetCardPoolForProperty(propName);
        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in source)
        {
            var value = (CardOptions)item.Value!;
            if (value == CardOptions.Any || cardPool.Contains(value))
            {
                rebuilt.Add(item);
            }
        }
        return rebuilt;
    }

    private static void RebuildCardDropdownForCharacter(string propName, NConfigDropdown dropdown, bool shouldCheckToReset)
    {
        var rebuilt = FilterCardItems(propName, CardMasterItems[propName]);

        if (shouldCheckToReset)
        {
            var currentOptionLabelField = dropdown.GetType()
                .GetCachedField("_currentOptionLabel", BindingFlags.NonPublic | BindingFlags.Instance);
            var currentOptionLabel = (MegaLabel)currentOptionLabelField?.GetValue(dropdown)!;
            currentOptionLabel.SetTextAutoSize(nameof(CardOptions.Any));
            ResetCardOptions(propName);
        }

        ConfigDropdownUtilities.RefreshDropdownItems(dropdown, rebuilt);
    }
    #endregion
}