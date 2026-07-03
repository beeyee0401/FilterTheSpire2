using FilterTheSpire2.Code.Ancients.Config;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.Code.Helpers;

public static class RelicModelFactory
{
    public static RelicModel? GetRelicModel(Enum e)
    {
        return e switch
        {
            NeowOptions n => RelicModelMappings.GetRelicModel(n),
            OrobasOptions o => RelicModelMappings.GetRelicModel(o),
            PaelOptions p => RelicModelMappings.GetRelicModel(p),
            TezcataraOptions t => RelicModelMappings.GetRelicModel(t),
            NonupeipeOptions n => RelicModelMappings.GetRelicModel(n),
            TanxOptions t => RelicModelMappings.GetRelicModel(t),
            VakuuOptions v => RelicModelMappings.GetRelicModel(v),
            DarvOptions d => RelicModelMappings.GetRelicModel(d),
            _ => throw new NotSupportedException($"Unsupported enum {e.GetType()}")
        };
    }
}