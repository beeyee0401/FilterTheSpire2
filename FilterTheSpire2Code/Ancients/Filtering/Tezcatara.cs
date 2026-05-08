using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Tezcatara : AbstractAncient
{
    public Tezcatara()
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Tezcatara>().Id.Entry;
        Ancient = Ancient.Tezcatara;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
        var rng = GetEventRng(seed);
        var list1 = new List<TezcataraOptions>()
        {
            TezcataraOptions.VeryHotCocoa,
            TezcataraOptions.YummyCookie,
            TezcataraOptions.NutritiousSoup
        };
        
        var list2 = new List<TezcataraOptions>()
        {
            TezcataraOptions.BiiigHug,
            TezcataraOptions.Storybook,
            TezcataraOptions.ToastyMittens
        };
        
        var list3 = new List<TezcataraOptions>()
        {
            TezcataraOptions.GoldenCompass,
            TezcataraOptions.PumpkinCandle,
            TezcataraOptions.ToyBox,
            TezcataraOptions.SealOfGold,
        };

        var optionLists = new List<List<TezcataraOptions>>()
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