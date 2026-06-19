using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
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
        var rng = RngHelper.GetEventRng(seed, Id!);
        CharacterOptions? seaGlassChar = null;
        if (FilterTheSpire2Config.Character != CharacterOptions.Any && 
            FilterTheSpire2Config.SeaGlassCharacter != CharacterOptions.Any && 
            FilterTheSpire2Config.Character !=  FilterTheSpire2Config.SeaGlassCharacter &&
            FilterTheSpire2Config.OrobasOptions == OrobasOptions.SeaGlass)
        {
            seaGlassChar = rng.NextItem(CharacterMapping.AllCharacters
                .Where(c => c != FilterTheSpire2Config.Character));
        }
        else
        {
            rng.NextItem([0]);
        }

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
            if (optionId == relic.Id && (seaGlassChar == null || seaGlassChar == FilterTheSpire2Config.SeaGlassCharacter))
            {
                return true;
            }
        }
        
        return false;
    }
}