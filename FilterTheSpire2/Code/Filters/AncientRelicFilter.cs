using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Filters;

public class AncientRelicFilter(Ancient selectedAncient, Enum? relicOption, int actNum) : IFilter
{
    private static List<ActDefinition> GetRandomActDefinitions(Rng actSelectionRng)
    {
        return
        [
            actSelectionRng.NextItem([
                ActDefinition.Overgrowth.Clone(),
                ActDefinition.Underdocks.Clone()
            ])!,
            ActDefinition.Hive.Clone(),
            ActDefinition.Glory.Clone()
        ];
    }
    
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var seedLong = RngHelper.GetSeedHash(seed);
        
        if (actNum == 1)
        {
            var neow = AncientFactory.GetAncient(Ancient.Neow, actNum);
            return relicOption != null && neow.CheckOptions(seedLong, relicOption);
        } 
        else if (actNum > 1)
        {
            var actSelectionRng = RngHelper.GetActSelectionRng(seed);

            var actList = GetRandomActDefinitions(actSelectionRng);

            var runRng = new RunRngSet(seed);
            var upfrontRng = runRng.UpFront;
            upfrontRng.FastForwardCounter(RngHelper.RngCounters.AncientCounter);

            var multiActAncients = AncientRules.MultiActAncientsAndRelics.Keys.ToList();

            multiActAncients.UnstableShuffle(upfrontRng);

            foreach (var act in actList.Skip(1))
            {
                var count = upfrontRng.NextInt(multiActAncients.Count + 1);

                var sharedAncientsForAct = multiActAncients
                    .Take(count)
                    .ToList();

                multiActAncients = multiActAncients
                    .Except(sharedAncientsForAct)
                    .ToList();

                act.SharedAncients.AddRange(sharedAncientsForAct);
            }

            var rolledAncients = actList
                .Select(act => AncientGenerator.Generate(act, upfrontRng))
                .ToList();

            if (rolledAncients[actNum - 1] != selectedAncient)
            {
                return false;
            }

            if (relicOption == null)
            {
                return true;
            }

            var ancient = AncientFactory.GetAncient(selectedAncient, actNum);
            return ancient.CheckOptions(seedLong, relicOption);
        }
        return true;
    }
}