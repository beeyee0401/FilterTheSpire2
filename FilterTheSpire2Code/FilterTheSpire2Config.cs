using System.Reflection;
using BaseLib.Config;
using BaseLib.Config.UI;
using FilterTheSpire2.FilterTheSpire2Code.DropdownOptions;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace FilterTheSpire2.FilterTheSpire2Code;

[ConfigHoverTipsByDefault]
internal class FilterTheSpire2Config : SimpleModConfig
{
    [ConfigSection("AncientsSection")]
    public static NeowOptions NeowOptions { get; set; } = NeowOptions.Any;
    public static Ancient Act2Ancients { get; set; } = Ancient.Any;
    public static Ancient Act3Ancients { get; set; } = Ancient.Any;
    
    [ConfigSection("AncientRelicsSection")]
    [ConfigVisibleIf(nameof(Act2Ancients), Ancient.Orobas)]
    public static OrobasOptions OrobasOptions { get; set; } = OrobasOptions.Any;
    
    [ConfigVisibleIf(nameof(Act2Ancients), Ancient.Pael)]
    public static PaelOptions PaelOptions { get; set; } = PaelOptions.Any;
    
    [ConfigVisibleIf(nameof(Act2Ancients), Ancient.Tezcatara)]
    public static TezcataraOptions TezcataraOptions { get; set; } = TezcataraOptions.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancients), Ancient.Nonupeipe)]
    public static NonupeipeOptions NonupeipeOptions { get; set; } = NonupeipeOptions.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancients), Ancient.Tanx)]
    public static TanxOptions TanxOptions { get; set; } = TanxOptions.Any;
    
    [ConfigVisibleIf(nameof(Act3Ancients), Ancient.Vakuu)]
    public static VakuuOptions VakuuOptions { get; set; } = VakuuOptions.Any;
    
    [ConfigHideInUI]
    public static DarvOptions DarvOptions { get; set; } = DarvOptions.Any;
    
    [ConfigSection("ActLocationsSection")]
    public static Act1Locations Act1Locations { get; set; } = Act1Locations.Any;
    public static Act2Locations Act2Locations { get; set; } = Act2Locations.Hive;
    public static Act3Locations Act3Locations { get; set; } = Act3Locations.Glory;

    private static readonly Dictionary<int, Ancient> CurrentSelection = new();
    private static readonly Dictionary<int, List<NConfigDropdownItem.ItemData>> MasterItems = new();
    private static readonly Dictionary<int, NConfigDropdown> Dropdowns = new();
    
    private static void RegisterDropdown(int act, NConfigDropdown dropdown)
    {
        Dropdowns[act] = dropdown;
    }
    
    private void SetActSpecificAncients(Control optionContainer, int act, string propName)
    {
        var row = optionContainer.GetNodeOrNull<NConfigOptionRow>($"%{propName}");
        if (row?.SettingControl is not NDropdownPositioner pos)
            return;

        var dropdownField = pos.GetType()
            .GetField("_dropdownNode", BindingFlags.NonPublic | BindingFlags.Instance);

        var inner = dropdownField?.GetValue(pos);
        if (inner is not NConfigDropdown dropDown)
            return;

        RegisterDropdown(act, dropDown);

        var itemsField = dropDown.GetType()
            .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

        var items = (List<NConfigDropdownItem.ItemData>?)itemsField?.GetValue(dropDown);
        if (items == null)
            return;

        // store original master list once
        if (!MasterItems.ContainsKey(act))
            MasterItems[act] = items.ToList();

        RebuildDropdownForAct(act, dropDown);
    }
    
    private Action WrapOnSet(int act, Ancient value, Action originalOnSet)
    {
        return () =>
        {
            originalOnSet?.Invoke();

            CurrentSelection[act] = value;

            SyncAllDropdowns();
        };
    }

    private void SyncAllDropdowns()
    {
        foreach (var kvp in Dropdowns)
        {
            RebuildDropdownForAct(kvp.Key, kvp.Value);
        }
    }
    
    private void RebuildDropdownForAct(int act, NConfigDropdown dropdown)
    {
        var itemsField = dropdown.GetType()
            .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

        var items = (List<NConfigDropdownItem.ItemData>?)itemsField?.GetValue(dropdown);
        if (items == null)
            return;
        
        var source = MasterItems[act];

        var rebuilt = new List<NConfigDropdownItem.ItemData>();
        foreach (var item in source)
        {
            var value = (Ancient)item.Value!;

            if (!AncientRules.IsValidForAct(act, value))
                continue;

            var takenElsewhere =
                AncientRules.MultiActAncients.Contains(value)
                && CurrentSelection.Any(x => x.Key != act && x.Value == value);

            if (takenElsewhere)
                continue;

            var wrappedOnSet = WrapOnSet(act, value, item.OnSet);

            rebuilt.Add(new NConfigDropdownItem.ItemData(
                item.Text,
                item.Value,
                wrappedOnSet
            ));
        }
        
        items.Clear();
        items.AddRange(rebuilt);
        
        var currentDisplayIndexField = dropdown.GetType()
            .GetField("_currentDisplayIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        var currentDisplayIndex = (int)currentDisplayIndexField?.GetValue(dropdown)!;
        if (currentDisplayIndex >= 0)
        {
            var selectedAncient = source[currentDisplayIndex].Value;
            var newIndex = rebuilt.FindIndex(i => i.Value == selectedAncient);
            currentDisplayIndexField.SetValue(dropdown, newIndex);
        }
        
        dropdown._Ready();
    }
    
    public override void SetupConfigUI(Control optionContainer)
    {
        base.SetupConfigUI(optionContainer);

        // TODO: Add Darv options for either act 2 or act 3 here
        
        SetActSpecificAncients(optionContainer, 2, nameof(Act2Ancients));
        SetActSpecificAncients(optionContainer, 3, nameof(Act3Ancients));
    }
}