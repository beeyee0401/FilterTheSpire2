using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Pael : AbstractAncient
{
    public Pael()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Pael>().Id.Entry;
        Ancient = Ancient.Pael;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
        var rng = GetEventRng(seed);
        var list1 = new List<PaelOptions>()
        {
            PaelOptions.PaelsFlesh,
            PaelOptions.PaelsHorn,
            PaelOptions.PaelsTears
        };
        
        var list2 = new List<PaelOptions>()
        {
            PaelOptions.PaelsWing,
            PaelOptions.PaelsClaw,
            PaelOptions.PaelsTooth,
            PaelOptions.PaelsWing,
            PaelOptions.PaelsClaw,
            PaelOptions.PaelsTooth,
            PaelOptions.PaelsGrowth,
        };
        
        var list3 = new List<PaelOptions>()
        {
            PaelOptions.PaelsEye,
            PaelOptions.PaelsBlood,
            PaelOptions.PaelsLegion
        };

        var optionLists = new List<List<PaelOptions>>()
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