using FilterTheSpire2.FilterTheSpire2Code.Filters;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

public sealed class SeedSearchRequest
{
    public required CharacterModel Character { get; init; }
    public required AscensionLevel AscensionLevel { get; init; }

    public required IReadOnlyList<IFilter> Filters { get; init; }

    public int DelayMs { get; init; } = 10;

    public int ThreadCount { get; init; } = 2;
}