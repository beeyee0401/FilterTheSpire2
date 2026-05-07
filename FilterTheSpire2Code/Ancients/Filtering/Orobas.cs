using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Orobas : AbstractAncient
{
    public Orobas()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Orobas>().Id.Entry;
        Ancient = Ancient.Orobas;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
        var rng = GetEventRng(seed);
        rng.NextItem([0]);

        var list1 = new List<OrobasOptions>
        {
            OrobasOptions.ElectricShrymp,
            OrobasOptions.GlassEye,
            OrobasOptions.SandCastle,
            rng.NextFloat() < 0.3333333134651184 ? OrobasOptions.PrismaticGem : OrobasOptions.SeaGlass
        };

        var list2 = new List<OrobasOptions>()
        {
            OrobasOptions.AlchemicalCoffer,
            OrobasOptions.Driftwood,
            OrobasOptions.RadiantPearl
        };

        var list3 = new List<OrobasOptions>()
        {
            OrobasOptions.TouchOfOrobas,
            OrobasOptions.ArchaicTooth,
        };
        
        var optionLists = new List<List<OrobasOptions>>()
        {
            list1,
            list2,
            list3
        };

        foreach (var optionList in optionLists)
        {
            var optionId =  RelicModelMappings.GetRelicModel(rng.NextItem(optionList))!.Id;
            if (optionId == relic.Id)
            {
                return true;
            }
        }
        
        return false;
    }
}