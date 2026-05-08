using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Vakuu : AbstractAncient
{
    public Vakuu()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Vakuu>().Id.Entry;
        Ancient = Ancient.Vakuu;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
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
            var relicId = RelicModelFactory.GetRelicModel(optionList[0])!.Id;
            if (relicId == relic.Id)
            {
                return true;
            }
        }
        
        return false;
    }
}