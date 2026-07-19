using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config.Logic;
using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.Relics;
using Godot;

namespace FilterTheSpire2.Code.Config;

public class FilterTheSpire2Config : SimpleModConfig
{
    [ConfigHoverTip]
    public static CharacterOptions Character { get; set; } = CharacterOptions.Any;
    
    [ConfigSection("Act1Section")] 
    public static ActLocations Act1Locations { get; set; } = ActLocations.Any;
    
    [ConfigDropdownOverrideLocalization("BOSS_OPTIONS")]
    public static BossOptions Act1Boss { get; set; } = BossOptions.Any;
    
    public static NeowOptions NeowOptions { get; set; } = NeowOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("NEOW_OPTIONS")]
    [ConfigHoverTip]
    public static NeowOptions NeowsBonesRelicOption1 { get; set; } = NeowOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("NEOW_OPTIONS")]
    [ConfigHoverTip]
    public static NeowOptions NeowsBonesRelicOption2 { get; set; } = NeowOptions.Any;
    
    [ConfigHideInUI]
    public static CardOptions NeowsBonesCurseOption { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions CapsuleRelicOption1 { get; set; } = RelicOptions.Any;

    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions CapsuleRelicOption2 { get; set; } = RelicOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions CapsuleRelicOption3 { get; set; } = RelicOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions LeadPaperweightOption { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions NewLeafOption { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions LeafyPoulticeOption1 { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions LeafyPoulticeOption2 { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions LostCofferCardOption { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("POTION_OPTIONS")]
    public static PotionOptions LostCofferPotionOption { get; set; } = PotionOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions KaleidoscopeOption1 { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions KaleidoscopeOption2 { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("CARD_OPTIONS")]
    public static CardOptions ArcaneScrollOption { get; set; } = CardOptions.Any;
    
    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("POTION_OPTIONS")]
    public static PotionOptions PhialHolsterPotionOption1 { get; set; } = PotionOptions.Any;

    [ConfigHideInUI]
    [ConfigDropdownOverrideLocalization("POTION_OPTIONS")]
    public static PotionOptions PhialHolsterPotionOption2 { get; set; } = PotionOptions.Any;
    
    [ConfigSection("Act2Section")]
    public static ActLocations Act2Locations { get; set; } = ActLocations.Any;
    
    [ConfigDropdownOverrideLocalization("BOSS_OPTIONS")]
    public static BossOptions Act2Boss { get; set; } = BossOptions.Any;
    
    public static Ancient Act2Ancient { get; set; } = Ancient.Any;
    
    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Orobas)]
    public static OrobasOptions OrobasOptions { get; set; } = OrobasOptions.Any;
    
    [ConfigVisibleIf(nameof(ShouldShowSeaGlassCharacters))]
    public static CharacterOptions SeaGlassCharacter { get; set; } = CharacterOptions.Any;

    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Pael)]
    public static PaelOptions PaelOptions { get; set; } = PaelOptions.Any;

    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Tezcatara)]
    public static TezcataraOptions TezcataraOptions { get; set; } = TezcataraOptions.Any;
    
    [ConfigSection("Act3Section")]
    public static ActLocations Act3Locations { get; set; } = ActLocations.Any;
    
    [ConfigDropdownOverrideLocalization("BOSS_OPTIONS")]
    public static BossOptions Act3Boss { get; set; } = BossOptions.Any;
    
    public static Ancient Act3Ancient { get; set; } = Ancient.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancient), Ancient.Nonupeipe)]
    public static NonupeipeOptions NonupeipeOptions { get; set; } = NonupeipeOptions.Any;

    [ConfigVisibleIf(nameof(Act3Ancient), Ancient.Tanx)]
    public static TanxOptions TanxOptions { get; set; } = TanxOptions.Any;

    [ConfigVisibleIf(nameof(Act3Ancient), Ancient.Vakuu)]
    public static VakuuOptions VakuuOptions { get; set; } = VakuuOptions.Any;

    [ConfigHideInUI]
    public static DarvOptions DarvOptions { get; set; } = DarvOptions.Any;
    
    [ConfigSection("RelicsSection")]
    [ConfigHoverTip]
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions CommonRelic { get; set; } = RelicOptions.Any;
    
    [ConfigHoverTip]
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions UncommonRelic { get; set; } = RelicOptions.Any;
    
    [ConfigHoverTip]
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions RareRelic { get; set; } = RelicOptions.Any;
    
    [ConfigDropdownOverrideLocalization("RELIC_OPTIONS")]
    public static RelicOptions ShopRelic { get; set; } = RelicOptions.Any;
    
    public static ColorRect CreateCardDivider() => CreateDividerControl();

    public NConfigOptionRow CreateHiddenOptionRow(string propertyName, out List<NConfigDropdownItem.ItemData> masterItems)
    {
        var property = typeof(FilterTheSpire2Config).GetCachedProperty(propertyName, BindingFlags.Public | BindingFlags.Static)!;
        var row = GenerateOptionFromProperty(property);
        var dropdown = ConfigDropdownUtilities.GetDropdownFromRow(row);
        masterItems = dropdown != null ? ConfigDropdownUtilities.GetItems(dropdown) : [];
        return row;
    }
    
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
                var actLocation = (ActLocations)actLocationItem.Value!;
                if (actLocation == ActLocations.Any)
                {
                    newItems.Add(actLocationItem);
                    continue;
                }

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

        SetupFocusNeighbors(optionContainer);

        AncientConfigController.SetupAncientDropdownConfig(optionContainer);
        MultiActAncientController.SetupMultiActAncientConfig(optionContainer);
        CharacterConfigController.SetupCharacterDropdownConfig(optionContainer);
        NeowConfigController.SetupNeowDropdownConfig(optionContainer);
        BossConfigController.SetupBossDropdownConfig(optionContainer);
    }
    
    public static bool ShouldShowNeowOutcomeOption(NeowOptions option)
    {
        return Character != CharacterOptions.Any && 
               (NeowOptions == option ||
                (NeowOptions == NeowOptions.NeowsBones && 
                 (NeowsBonesRelicOption1 == option || NeowsBonesRelicOption2 == option)));
    }
    
    private static bool ShouldShowSeaGlassCharacters()
    {
        return Character != CharacterOptions.Any && 
               Act2Ancient == Ancient.Orobas && 
               OrobasOptions == OrobasOptions.SeaGlass;
    }
    
    public static bool IsCapsuleSelected()
    {
        return NeowOptions is NeowOptions.SmallCapsule or NeowOptions.LargeCapsule;
    }

    public static bool ShouldShowCapsuleRelicOption2()
    {
        return NeowOptions == NeowOptions.LargeCapsule;
    }
}