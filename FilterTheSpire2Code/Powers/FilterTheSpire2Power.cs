using BaseLib.Abstracts;
using BaseLib.Extensions;
using FilterTheSpire2.FilterTheSpire2Code.Extensions;
using Godot;

namespace FilterTheSpire2.FilterTheSpire2Code.Powers;

public abstract class FilterTheSpire2Power : CustomPowerModel
{
    //Loads from FilterTheSpire2/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}