using FilterTheSpire2.FilterTheSpire2Code.Ancients;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

public static class AncientMapping
{
    public static readonly Dictionary<Ancient, AncientEventModel> AncientEvents = new()
    {
        { Ancient.Orobas, ModelDb.AncientEvent<Orobas>() },
        { Ancient.Pael, ModelDb.AncientEvent<Pael>() },
        { Ancient.Tezcatara, ModelDb.AncientEvent<Tezcatara>() },
        { Ancient.Darv, ModelDb.AncientEvent<Darv>() },
        { Ancient.Nonupeipe, ModelDb.AncientEvent<Nonupeipe>() },
        { Ancient.Tanx, ModelDb.AncientEvent<Tanx>() },
        { Ancient.Vakuu, ModelDb.AncientEvent<Vakuu>() },
    };
}