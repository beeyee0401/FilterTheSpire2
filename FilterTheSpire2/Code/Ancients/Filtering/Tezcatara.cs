using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public class Tezcatara : AbstractAncient
{
    public Tezcatara()
    {
        Id = "TEZCATARA";
    }

    public override bool CheckOptions(uint seed, Enum? relicOption)
    {
        if (relicOption is not TezcataraOptions relic)
        {
            return true;
        }
        
        var rng = RngHelper.GetEventRng(seed, Id!);
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
            var option = rng.NextItem(optionList);
            if (option == relic)
            {
                return true;
            }
        }
        return false;
    }
}