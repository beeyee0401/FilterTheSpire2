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
using Darv = MegaCrit.Sts2.Core.Models.Events.Darv;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public class AncientRelicFilter(Ancient selectedAncient, RelicModel? relicModel, int actNum) : IFilter
{
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var rng = new Rng((uint) StringHelper.GetDeterministicHashCode(seed));
        var unlockState = UnlockState.all;
        var player = Player.CreateForNewRun(request.Character, unlockState, 1UL);
        
        if (actNum == 1)
        {
            var neow = ModelDb.AncientEvent<Neow>();
            var mutableNeow = neow.ToMutable();
            mutableNeow.BeginEvent(player, false);
            return mutableNeow.CurrentOptions.Any(o => o.Relic!.Id == relicModel!.Id);
        } 
        else if (actNum > 1)
        {
            var actList = ActModel.GetRandomList(rng, unlockState, false)
                .Select(a => a.ToMutable()).ToList();
            var runState = RunState.CreateForNewRun(
                [player], 
                actList, 
                [], 
                GameMode.Standard, 
                10, 
                seed);
            
            // This is the number of increments it gets to before generating the rooms for the acts, we don't care about stuff before this
            const int startingRngCounter = 230;
            
            var upfrontRng = new Rng(runState.Rng.UpFront.Seed, startingRngCounter);

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
            return ancient!.CheckOptions(rng.Seed, relicModel);
        }
        return true;
    }
}