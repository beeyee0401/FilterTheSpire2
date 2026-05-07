using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public class Darv : AbstractAncient
{
    private int _actNum;
    public Darv(int actNum)
    {
        Id = ModelDb.AncientEvent<MegaCrit.Sts2.Core.Models.Events.Darv>().Id.Entry;
        Ancient = Ancient.Darv;
        _actNum = actNum;
    }

    public override bool CheckOptions(uint seed, RelicModel relic)
    {
        // return true;
        var rng = GetEventRng(seed);
        var source = new List<List<DarvOptions>>()
        {
            new() {DarvOptions.Astrolabe},
            new() {DarvOptions.BlackStar},
            new() {DarvOptions.CallingBell},
            new() {DarvOptions.EmptyCage},
            new() {DarvOptions.PandorasBox},
            new() {DarvOptions.RunicPyramid},
            new() {DarvOptions.SneckoEye},
        };

        switch (_actNum)
        {
            case 2:
                source.Add([DarvOptions.EctoplasmAct2, DarvOptions.SozuAct2]);
                break;
            case 3:
                source.Add([DarvOptions.PhilosophersStoneAct3, DarvOptions.VelvetChokerAct3]);
                break;
        }

        var rngList = source.Select(rng.NextItem).ToList();
        rngList.UnstableShuffle(rng);

        List<DarvOptions> finalList;
        if (rng.NextBool())
        {
            finalList = rngList.Take(2).ToList();
            finalList.Add(DarvOptions.DustyTome);
        }
        else
        {
            finalList = rngList.Take(3).ToList();
        }

        return finalList
            .Select(r => RelicModelFactory.GetRelicModel(r)!.Id)
            .Contains(relic.Id);
    }
}