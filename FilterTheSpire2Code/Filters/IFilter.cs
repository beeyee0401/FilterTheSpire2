using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public interface IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed);
    // public FilterType FilterType { get; }
}