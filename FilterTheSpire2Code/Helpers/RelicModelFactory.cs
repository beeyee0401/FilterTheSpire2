using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

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
            ShopRelicOptions s => RelicModelMappings.GetRelicModel(s),
            _ => throw new NotSupportedException($"Unsupported enum {e.GetType()}")
        };
    }
}