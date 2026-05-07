using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.ActLocations;
using FilterTheSpire2.FilterTheSpire2Code.Ancients;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Config.Logic;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace FilterTheSpire2.FilterTheSpire2Code.Config;

[ConfigHoverTipsByDefault]
public class FilterTheSpire2Config : SimpleModConfig
{
    [ConfigSection("AncientsSection")]
    public static NeowOptions NeowOptions { get; set; } = NeowOptions.Any;
    public static Ancient Act2Ancient { get; set; } = Ancient.Any;
    public static Ancient Act3Ancient { get; set; } = Ancient.Any;
    
    [ConfigSection("AncientRelicsSection")]
    [ConfigVisibleIf(nameof(Act2Ancient), Ancient.Orobas)]
    public static OrobasOptions OrobasOptions { get; set; } = OrobasOptions.Any;
    
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
    
    [ConfigSection("ActLocationsSection")]
    public static Act1Locations Act1Locations { get; set; } = Act1Locations.Any;
    public static Act2Locations Act2Locations { get; set; } = Act2Locations.Hive;
    public static Act3Locations Act3Locations { get; set; } = Act3Locations.Glory;
    
    public override void SetupConfigUI(Control optionContainer)
    {
        base.SetupConfigUI(optionContainer);

        AncientConfigController.SetupAncientDropdownConfig(optionContainer);
        MultiActAncientController.UpdateMultiActAncientActSpecificRelics(optionContainer);
    }
    
    private static bool ShouldShowMultiActOptions(MemberInfo memberInfo)
    {
        if (memberInfo is not PropertyInfo propertyInfo) return false;

        return propertyInfo.Name switch
        {
            nameof(DarvOptions) => Act2Ancient == Ancient.Darv || 
                                   Act3Ancient == Ancient.Darv,
            _ => false
        };
    }
}