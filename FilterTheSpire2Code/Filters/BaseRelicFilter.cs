using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public abstract class BaseRelicFilter : IFilter
{
    public abstract List<TEnum> GetRelicPoolList<TEnum>() where TEnum : Enum;
    
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        throw new NotImplementedException();
    }
}