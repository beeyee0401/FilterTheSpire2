using FilterTheSpire2.FilterTheSpire2Code.ActLocations;
using FilterTheSpire2.FilterTheSpire2Code.Ancients;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class AncientRelicFilter(Ancient selectedAncient, RelicModel? relicModel, int actNum) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var rng = new Rng((uint) StringHelper.GetDeterministicHashCode(seed));
        var unlockState = UnlockState.all;
        // var player = Player.CreateForNewRun(request.Character, unlockState, 1UL);
        
        var actList = ActModel.GetRandomList(rng, unlockState, false)
            .Select(a => a.ToMutable()).ToList();
        
        if (actNum == 1)
        {
            var neow = AncientFactory.GetAncient(Ancient.Neow, actNum);
            return relicModel != null && neow.CheckOptions(rng.Seed, relicModel);
        } 
        else if (actNum > 1)
        {
            var runRng = new RunRngSet(seed);
            var upfrontRng = new Rng(runRng.UpFront.Seed, RngHelper.RngCounters.AncientCounter);

            var multiActAncients = AncientRules.MultiActAncientsAndRelics.Keys
                .Select(a => AncientMapping.AncientEvents[a]).ToList();
            multiActAncients.UnstableShuffle(upfrontRng);
            foreach (var actModel in actList.Skip(1))
            {
                var count = upfrontRng.NextInt(multiActAncients.Count + 1);
                var list = multiActAncients.Take(count).ToList();
                multiActAncients = multiActAncients.Except(list).ToList();
                actModel.SetSharedAncientSubset(list);
            }
            
            foreach (var act in actList)
            {
                act.GenerateRooms(upfrontRng, unlockState);
            }

            if (actList[actNum - 1].Ancient.Id != AncientMapping.AncientEvents[selectedAncient].Id)
            {
                return false;
            }

            if (relicModel == null) return true;
            
            var ancient = AncientFactory.GetAncient(selectedAncient, actNum);
            return ancient.CheckOptions(rng.Seed, relicModel);
        }
        return true;
    }
}