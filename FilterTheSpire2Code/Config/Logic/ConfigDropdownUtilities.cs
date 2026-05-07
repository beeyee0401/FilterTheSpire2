using System.Reflection;
using BaseLib.Config.UI;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class ConfigDropdownUtilities
{
    public static (NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> dropdownItems) GetDropdownListItems(
        Control optionContainer,
        string propName)
    {
        var row = optionContainer.GetNodeOrNull<NConfigOptionRow>($"%{propName}");
        if (row?.SettingControl is not NDropdownPositioner pos)
            return (null, [])!;

        var dropdownField = pos.GetType()
            .GetField("_dropdownNode", BindingFlags.NonPublic | BindingFlags.Instance);

        var inner = dropdownField?.GetValue(pos);
        if (inner is not NConfigDropdown dropdown)
            return (null, [])!;

        var itemsField = dropdown.GetType()
            .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

        var items = (List<NConfigDropdownItem.ItemData>?)itemsField?.GetValue(dropdown);
        return items == null ? (dropdown, []) : (dropdown, items);
    }
}