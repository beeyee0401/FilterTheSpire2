using System.Reflection;
using BaseLib.Config.UI;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace FilterTheSpire2.FilterTheSpire2Code.Config.Logic;

public static class ConfigDropdownUtilities
{
    // Cache lookups — reflection is expensive, these never change
    private static readonly Dictionary<(Type, string), FieldInfo?> FieldCache = new();
    private static readonly Dictionary<(Type, string), PropertyInfo?> PropertyCache = new();
    private static readonly Dictionary<(Type, string), MethodInfo?> MethodCache = new();

    public static FieldInfo? GetCachedField(this Type type, string name, BindingFlags flags)
    {
        var key = (type, name);
        if (FieldCache.TryGetValue(key, out var fi))
        {
            return fi;
        }
        fi = type.GetField(name, flags);
        FieldCache[key] = fi;
        return fi;
    }
    
    public static PropertyInfo? GetCachedProperty(this Type type, string name, BindingFlags flags)
    {
        var key = (type, name);
        if (PropertyCache.TryGetValue(key, out var pi))
        {
            return pi;
        }
        pi = type.GetProperty(name, flags);
        PropertyCache[key] = pi;
        return pi;
    }
    
    private static MethodInfo? GetCachedMethod(this Type type, string name)
    {
        var key = (type, name);
        if (MethodCache.TryGetValue(key, out var mi))
        {
            return mi;
        }
        mi = type.GetMethod(name);
        MethodCache[key] = mi;
        return mi;
    }
    
    public static (NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> dropdownItems) GetDropdownListItems(
        Control optionContainer,
        string propName)
    {
        propName = propName.StartsWith("%") ? propName :  $"%{propName}";
        var row = optionContainer.GetNodeOrNull<NConfigOptionRow>($"{propName}");
        if (row?.SettingControl is not NDropdownPositioner pos)
        {
            return (null, [])!;
        }

        var dropdownField = pos.GetType()
            .GetCachedField("_dropdownNode", BindingFlags.NonPublic | BindingFlags.Instance);

        var inner = dropdownField?.GetValue(pos);
        if (inner is not NConfigDropdown dropdown)
            return (null, [])!;

        var itemsField = dropdown.GetType()
            .GetCachedField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

        var items = (List<NConfigDropdownItem.ItemData>?)itemsField?.GetValue(dropdown);
        return items == null ? (dropdown, []) : (dropdown, items);
    }

    public static void RefreshDropdownItems(NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> newItems)
    {
        // Get _dropdownItems via reflection (inherited from NSettingsDropdown)
        var dropdownItemsField = typeof(NSettingsDropdown).GetCachedField("_dropdownItems",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var dropdownItems = (Control)dropdownItemsField!.GetValue(dropdown)!;
            
        // Clear existing child nodes
        foreach (var child in dropdownItems.GetChildren())
        {
            dropdownItems.RemoveChild(child);
            child.QueueFree();
        }
            
        // Rebuild child nodes (mirrors _Ready logic)
        for (var i = 0; i < newItems.Count; i++)
        {
            var child = NConfigDropdownItem.Create(newItems[i]);
            dropdownItems.AddChildSafely(child);
            child.Connect(NDropdownItem.SignalName.Selected,
                Callable.From(new Action<NDropdownItem>(item =>
                {
                    if (item is not NConfigDropdownItem configItem)
                    {
                        return;
                    }
                    dropdown.Call("CloseDropdown");
            
                    // Update display label via reflection
                    var labelField = typeof(NSettingsDropdown).GetCachedField("_currentOptionLabel",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    var label = labelField!.GetValue(dropdown);
                    label!.GetType().GetCachedMethod("SetTextAutoSize")?.Invoke(label, [configItem.Data.Text]);
                    configItem.Data.OnSet();
                })));
            child.Init(i);
        }
            
        dropdownItems.GetParent<NDropdownContainer>().RefreshLayout();
    }
}