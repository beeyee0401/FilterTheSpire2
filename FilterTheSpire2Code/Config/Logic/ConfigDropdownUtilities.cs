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
    
    public static List<NConfigDropdownItem.ItemData> GetItems(NConfigDropdown dropdown)
    {
        var itemsField = dropdown.GetType()
            .GetCachedField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
        return (List<NConfigDropdownItem.ItemData>?)itemsField?.GetValue(dropdown) ?? [];
    }
    
    public static NConfigDropdown? GetDropdownFromRow(NConfigOptionRow row)
    {
        if (row.SettingControl is not NDropdownPositioner pos)
        {
            return null;
        }
        var dropdownField = pos.GetType()
            .GetCachedField("_dropdownNode", BindingFlags.NonPublic | BindingFlags.Instance);
        return dropdownField?.GetValue(pos) as NConfigDropdown;
    }
    
    public static void SeedItemsBeforeReady(NConfigDropdown dropdown, List<NConfigDropdownItem.ItemData> items)
    {
        var itemsField = dropdown.GetType()
            .GetCachedField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
        itemsField?.SetValue(dropdown, items);
        dropdown.SetFromProperty(); // recompute _currentDisplayIndex against the new (short) list — safe pre-_Ready, IsNodeReady() is still false so no label mutation happens yet
    }
    
    public static void RefreshDropdownItems(
        NConfigDropdown dropdown,
        List<NConfigDropdownItem.ItemData> newItems)
    {
        var dropdownItemsField = typeof(NSettingsDropdown).GetCachedField("_dropdownItems",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var dropdownItems =
            (Control)dropdownItemsField?.GetValue(dropdown)!;

        var children = dropdownItems.GetChildren();
        var existingCount = children.Count;
        for (var i = 0; i < newItems.Count; i++)
        {
            NConfigDropdownItem item;
            if (i < existingCount)
            {
                item = (NConfigDropdownItem) children[i];
                item.Data = newItems[i];
            }
            else
            {
                item = NConfigDropdownItem.Create(newItems[i]);
                dropdownItems.AddChildSafely(item);
                ConnectDropdownItem(item, dropdown);
            }

            item.Init(i);
        }
        
        for (var i = existingCount - 1; i >= newItems.Count; i--)
        {
            dropdownItems.RemoveChild(children[i]);
            children[i].QueueFree();
        }

        if (existingCount != newItems.Count)
        {
            dropdownItems.GetParent<NDropdownContainer>()
                .RefreshLayout();
        }
    }
    
    private static void ConnectDropdownItem(
        NConfigDropdownItem item,
        NConfigDropdown dropdown)
    {
        var labelField = typeof(NSettingsDropdown).GetCachedField("_currentOptionLabel", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        var label = labelField?.GetValue(dropdown);
        var setTextMethod =
            label?.GetType().GetCachedMethod("SetTextAutoSize");

        item.Connect(
            NDropdownItem.SignalName.Selected,
            Callable.From(new Action<NDropdownItem>(selected =>
            {
                if (selected is not NConfigDropdownItem configItem)
                {
                    return;
                }

                dropdown.Call("CloseDropdown");

                setTextMethod?.Invoke(
                    label,
                    [configItem.Data.Text]);

                configItem.Data.OnSet();
            })));
    }
}