using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.Code.Filters;

public interface IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed);
    // public FilterType FilterType { get; }
}