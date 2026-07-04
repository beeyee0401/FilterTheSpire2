using MegaCrit.Sts2.Core.Entities.Encounters;

namespace FilterTheSpire2.Code.Helpers;

public sealed class SimpleEncounterDef(params EncounterTag[] tags)
{
    public HashSet<EncounterTag> Tags { get; } = tags.ToHashSet();
}