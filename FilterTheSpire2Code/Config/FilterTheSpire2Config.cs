using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.ActLocations;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Config.Logic;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using Godot;

namespace FilterTheSpire2.FilterTheSpire2Code.Config;

public class FilterTheSpire2Config : SimpleModConfig
{
    [ConfigHoverTip]
    public static CharacterOptions Character { get; set; } = CharacterOptions.Any;
    
    [ConfigSection("AncientsSection")] 
    public static NeowOptions NeowOptions { get; set; } = NeowOptions.Any;
    public static Ancient Act2Ancient { get; set; } = Ancient.Any;
    public static Ancient Act3Ancient { get; set; } = Ancient.Any;

    [ConfigSection("AncientRelicsSection")]
    [ConfigVisibleIf(nameof(ShouldShowNewLeafOptions))]
    [ConfigOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions NewLeafOption { get; set; } = CardOptions.Any;
    
    [ConfigVisibleIf(nameof(ShouldShowLeafyPoulticeOptions))]
    [ConfigOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions LeafyPoulticeOption1 { get; set; } = CardOptions.Any;
    
    [ConfigVisibleIf(nameof(ShouldShowLeafyPoulticeOptions))]
    [ConfigOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions LeafyPoulticeOption2 { get; set; } = CardOptions.Any;
    
    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Orobas)]
    public static OrobasOptions OrobasOptions { get; set; } = OrobasOptions.Any;
    
    [ConfigVisibleIf(nameof(ShouldShowSeaGlassCharacters))]
    public static CharacterOptions SeaGlassCharacter { get; set; } = CharacterOptions.Any;

    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Pael)]
    public static PaelOptions PaelOptions { get; set; } = PaelOptions.Any;

    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Tezcatara)]
    public static TezcataraOptions TezcataraOptions { get; set; } = TezcataraOptions.Any;

    [ConfigVisibleIf(nameof(Act3Ancient), Ancient.Nonupeipe)]
    public static NonupeipeOptions NonupeipeOptions { get; set; } = NonupeipeOptions.Any;

    [ConfigVisibleIf(nameof(Act3Ancient), Ancient.Tanx)]
    public static TanxOptions TanxOptions { get; set; } = TanxOptions.Any;

    [ConfigVisibleIf(nameof(Act3Ancient), Ancient.Vakuu)]
    public static VakuuOptions VakuuOptions { get; set; } = VakuuOptions.Any;

    [ConfigVisibleIf(nameof(ShouldShowMultiActOptions))]
    public static DarvOptions DarvOptions { get; set; } = DarvOptions.Any;
    
    [ConfigSection("RelicsSection")]
    public static RelicOptions CommonRelic { get; set; } = RelicOptions.Any;
    public static RelicOptions UncommonRelic { get; set; } = RelicOptions.Any;
    public static RelicOptions RareRelic { get; set; } = RelicOptions.Any;
    public static RelicOptions ShopRelic { get; set; } = RelicOptions.Any;

    [ConfigSection("ActLocationsSection")]
    // [ConfigHideInUI]
    public static ActLocations.ActLocations Act1Locations { get; set; } = ActLocations.ActLocations.Any;

    // [ConfigHideInUI]
    public static ActLocations.ActLocations Act2Locations { get; set; } = ActLocations.ActLocations.Hive;

    // [ConfigHideInUI]
    public static ActLocations.ActLocations Act3Locations { get; set; } = ActLocations.ActLocations.Glory;

    public override void SetupConfigUI(Control optionContainer)
    {
        base.SetupConfigUI(optionContainer);

        for (var i = 1; i <= 3; i++)
        {
            var (dropdown, items) = ConfigDropdownUtilities.GetDropdownListItems(optionContainer, $"%Act{i}Locations");
            var allActLocations = items.ToList();
            
            var newItems = new List<NConfigDropdownItem.ItemData>();
            foreach (var actLocationItem in allActLocations)
            {
                var actLocation = (ActLocations.ActLocations)actLocationItem.Value!;
                if (ActLocationRules.IsValidForAct(i, actLocation))
                {
                    newItems.Add(actLocationItem);
                }
            }
            ConfigDropdownUtilities.RefreshDropdownItems(dropdown, newItems);
        }

        var resetContainer = optionContainer.GetNodeOrNull<Control>("ResetDefaultsButtonContainer");
        if (resetContainer != null)
        {
            optionContainer.MoveChild(resetContainer, -1);
        }

        SetupFocusNeighbors(optionContainer); // Fix controller focus order

        AncientConfigController.SetupAncientDropdownConfig(optionContainer);
        MultiActAncientController.UpdateMultiActAncientActSpecificRelics(optionContainer);
        CharacterConfigController.SetupCharacterDropdownConfig(optionContainer);
    }

    private static bool ShouldShowMultiActOptions()
    {
        return Act2Ancient == Ancient.Darv ||
               Act3Ancient == Ancient.Darv;
    }

    private static bool ShouldShowLeafyPoulticeOptions()
    {
        return Character != CharacterOptions.Any && 
               NeowOptions == NeowOptions.LeafyPoultice;
    }
    
    private static bool ShouldShowNewLeafOptions()
    {
        return Character != CharacterOptions.Any && 
               NeowOptions == NeowOptions.NewLeaf;
    }
    
    private static bool ShouldShowSeaGlassCharacters()
    {
        return Character != CharacterOptions.Any && 
               Act2Ancient == Ancient.Orobas && 
               OrobasOptions == OrobasOptions.SeaGlass;
    }
}