namespace FilterTheSpire2.Code.Filters.RelicOutcomeFilters;

public interface INeowOutcomeFilter : IFilter
{
    NeowRngConsumption RngConsumption { get; }
}