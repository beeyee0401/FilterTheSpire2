using BaseLib.Config;
using FilterTheSpire2.FilterTheSpire2Code.DropdownOptions;
using Godot;

namespace FilterTheSpire2.FilterTheSpire2Code;

[ConfigHoverTipsByDefault]
internal class FilterTheSpire2Config : SimpleModConfig
{
    [ConfigSection("AncientsSection")]
    public static NeowOptions NeowOptions { get; set; } = NeowOptions.Any;
    public static Act2Ancients Act2Ancients { get; set; } = Act2Ancients.Any;
    
    public static Act3Ancients Act3Ancients { get; set; } = Act3Ancients.Any;
    
    [ConfigSection("AncientRelicsSection")]
    [ConfigVisibleIf(nameof(Act2Ancients), Act2Ancients.Orobas)]
    public static OrobasOptions OrobasOptions { get; set; } = OrobasOptions.Any;
    
    [ConfigVisibleIf(nameof(Act2Ancients), Act2Ancients.Pael)]
    public static PaelOptions PaelOptions { get; set; } = PaelOptions.Any;
    
    [ConfigVisibleIf(nameof(Act2Ancients), Act2Ancients.Tezcatara)]
    public static TezcataraOptions TezcataraOptions { get; set; } = TezcataraOptions.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancients), Act3Ancients.Nonupeipe)]
    public static NonupeipeOptions NonupeipeOptions { get; set; } = NonupeipeOptions.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancients), Act3Ancients.Tanx)]
    public static TanxOptions TanxOptions { get; set; } = TanxOptions.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancients), Act3Ancients.Vakuu)]
    public static VakuuOptions VakuuOptions { get; set; } = VakuuOptions.Any;
    
    [ConfigSection("ActLocationsSection")]
    public static Act1Locations Act1Locations { get; set; } = Act1Locations.Any;
    public static Act2Locations Act2Locations { get; set; } = Act2Locations.Hive;
    public static Act3Locations Act3Locations { get; set; } = Act3Locations.Glory;

    // TODO: Change the ancient relic selectors to a screen with rendered images and descriptions that are selectable
    public override void SetupConfigUI(Control optionContainer)
    {
        base.SetupConfigUI(optionContainer);
    }
}