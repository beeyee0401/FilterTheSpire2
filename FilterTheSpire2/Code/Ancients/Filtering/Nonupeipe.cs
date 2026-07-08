using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public class Nonupeipe : AbstractAncient
{
    public Nonupeipe()
    {
        Id = "NONUPEIPE";
    }

    public override bool CheckOptions(uint seed, Enum? relicOption)
    {
        if (relicOption is not NonupeipeOptions relic)
        {
            return true;
        }
        
        var rng = RngHelper.GetEventRng(seed, Id!);
        var list = new List<NonupeipeOptions>()
        {
            NonupeipeOptions.BlessedAntler,
            NonupeipeOptions.BrilliantScarf,
            NonupeipeOptions.DelicateFrond,
            NonupeipeOptions.DiamondDiadem,
            NonupeipeOptions.FurCoat,
            NonupeipeOptions.Glitter,
            NonupeipeOptions.JewelryBox,
            NonupeipeOptions.LoomingFruit,
            NonupeipeOptions.SignetRing,
            NonupeipeOptions.BeautifulBracelet
        };

        list.UnstableShuffle(rng);
        var options = list.Take(3);
        return options.Contains(relic);
    }
}