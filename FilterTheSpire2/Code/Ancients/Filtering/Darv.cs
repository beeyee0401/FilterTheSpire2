using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Extensions;

namespace FilterTheSpire2.Code.Ancients.Filtering;

public class Darv : AbstractAncient
{
    private readonly int _actNum;

    public Darv(int actNum)
    {
        Id = "DARV";
        _actNum = actNum;
    }

    public override bool CheckOptions(ulong seed, Enum? relicOption)
    {
        if (relicOption is not DarvOptions relic)
        {
            return true;
        }

        var rng = RngHelper.GetEventRng(seed, Id!);

        var source = new List<DarvOptions>
        {
            DarvOptions.Astrolabe,
            DarvOptions.BlackStar,
            DarvOptions.CallingBell,
            DarvOptions.EmptyCage,
            DarvOptions.PandorasBox,
            DarvOptions.RunicPyramid,
            DarvOptions.SneckoEye
        };

        if (_actNum == 2)
        {
            source.Add(DarvOptions.EctoplasmAct2);
            source.Add(DarvOptions.SozuAct2);
        }

        if (_actNum >= 2)
        {
            source.Add(DarvOptions.PhilosophersStone);
            source.Add(DarvOptions.VelvetChoker);
        }
        
        foreach (var _ in source)
        {
            rng.NextInt(1);
        }

        source.UnstableShuffle(rng);

        List<DarvOptions> finalList;
        if (rng.NextBool())
        {
            finalList = source.Take(2).ToList();
            finalList.Add(DarvOptions.DustyTome);
        }
        else
        {
            finalList = source.Take(3).ToList();
        }
        return finalList.Contains(relic);
    }
}