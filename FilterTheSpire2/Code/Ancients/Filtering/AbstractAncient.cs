namespace FilterTheSpire2.Code.Ancients.Filtering;

public abstract class AbstractAncient
{
    protected string? Id;

    public abstract bool CheckOptions(ulong seed, Enum? relic);
}