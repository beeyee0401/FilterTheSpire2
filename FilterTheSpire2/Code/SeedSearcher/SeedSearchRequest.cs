using FilterTheSpire2.Code.Filters;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.Code.SeedSearcher;

public sealed class SeedSearchRequest
{
    public required AscensionLevel AscensionLevel { get; init; }

    public required IReadOnlyList<IFilter> Filters { get; init; }

    public int ThreadCount { get; init; } = 2;
}