using FilterTheSpire2.FilterTheSpire2Code.Relics;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class ShopRelicFilter(ShopRelicOptions shopRelicOption) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var list = new List<ShopRelicOptions>
        {
            ShopRelicOptions.BeltBuckle,
            ShopRelicOptions.Bread,
            ShopRelicOptions.BurningSticks,
            ShopRelicOptions.Cauldron,
            ShopRelicOptions.ChemicalX,
            ShopRelicOptions.DingyRug,
            ShopRelicOptions.DollysMirror,
            ShopRelicOptions.DragonFruit,
            ShopRelicOptions.GhostSeed,
            ShopRelicOptions.GnarledHammer,
            ShopRelicOptions.Kifuda,
            ShopRelicOptions.LavaLamp,
            ShopRelicOptions.LeesWaffle,
            ShopRelicOptions.MembershipCard,
            ShopRelicOptions.MiniatureTent,
            ShopRelicOptions.MysticLighter,
            ShopRelicOptions.Orrery,
            ShopRelicOptions.PunchDagger,
            ShopRelicOptions.RingingTriangle,
            ShopRelicOptions.RoyalStamp,
            ShopRelicOptions.ScreamingFlagon,
            ShopRelicOptions.SlingOfCourage,
            ShopRelicOptions.TheAbacus,
            ShopRelicOptions.Toolbox,
            ShopRelicOptions.WingCharm,
            ShopRelicOptions.CharacterShopRelic,
        };
        
        const int startingRngCounter = 205;
        var runRng = new RunRngSet(seed);
        var upfrontRng = new Rng(runRng.UpFront.Seed, startingRngCounter);

        list.UnstableShuffle(upfrontRng);
        var option = list.Last();
        return option == shopRelicOption;
    }
}