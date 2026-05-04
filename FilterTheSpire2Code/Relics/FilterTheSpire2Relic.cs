using BaseLib.Abstracts;
using BaseLib.Extensions;
using FilterTheSpire2.FilterTheSpire2Code.Extensions;
using Godot;

namespace FilterTheSpire2.FilterTheSpire2Code.Relics;

public abstract class FilterTheSpire2Relic : CustomRelicModel
{
    //FilterTheSpire2/images/relics
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}