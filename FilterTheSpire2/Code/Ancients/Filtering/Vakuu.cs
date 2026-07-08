using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public class Vakuu : AbstractAncient
{
    public Vakuu()
    {
        Id = "VAKUU";
    }

    public override bool CheckOptions(uint seed, Enum? relicOption)
    {
        if (relicOption is not VakuuOptions relic)
        {
            return true;
        }
        
        var rng = RngHelper.GetEventRng(seed, Id!);
        var list1 = new List<VakuuOptions>()
        {
            VakuuOptions.BloodSoakedRose,
            VakuuOptions.WhisperingEarring,
            VakuuOptions.Fiddle
        };
        
        var list2 = new List<VakuuOptions>()
        {
            VakuuOptions.PreservedFog,
            VakuuOptions.SereTalon,
            VakuuOptions.DistinguishedCape
            
        };
        
        var list3 = new List<VakuuOptions>()
        {
            VakuuOptions.ChoicesParadox,
            VakuuOptions.MusicBox,
            VakuuOptions.LordsParasol,
            VakuuOptions.JeweledMask
        };

        var optionLists = new List<List<VakuuOptions>>()
        {
            list1,
            list2,
            list3
        };
        
        foreach (var optionList in optionLists)
        {
            optionList.UnstableShuffle(rng);
            if (optionList[0] == relic)
            {
                return true;
            }
        }
        
        return false;
    }
}