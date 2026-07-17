using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public class Pael : AbstractAncient
{
    public Pael()
    {
        Id = "PAEL";
    }

    public override bool CheckOptions(ulong seed, Enum? relicOption)
    {
        if (relicOption is not PaelOptions relic)
        {
            return true;
        }
        
        var rng = RngHelper.GetEventRng(seed, Id!);
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
            var option =  rng.NextItem(optionList);
            if (option == relic)
            {
                return true;
            }
        }
        return false;
    }
}