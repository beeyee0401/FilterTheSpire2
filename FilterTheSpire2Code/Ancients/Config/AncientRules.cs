using System.Collections.Immutable;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;

public static class AncientRules
{
    private static readonly HashSet<Ancient> Act2Allowed =
    [
        Ancient.Any,
        Ancient.Orobas,
        Ancient.Pael,
        Ancient.Tezcatara,
        Ancient.Darv
    ];

    private static readonly HashSet<Ancient> Act3Allowed =
    [
        Ancient.Any,
        Ancient.Nonupeipe,
        Ancient.Tanx,
        Ancient.Vakuu,
        Ancient.Darv
    ];

    public static bool IsValidForAct(int act, Ancient value)
    {
        return act switch
        {
            2 => Act2Allowed.Contains(value),
            3 => Act3Allowed.Contains(value),
            _ => false
        };
    }

    public static readonly Dictionary<Ancient, Dictionary<int, List<DarvOptions>>> MultiActAncientsAndRelics = new()
    {
        {Ancient.Darv, new Dictionary<int, List<DarvOptions>>()
        {
            {2, [DarvOptions.Any, DarvOptions.Astrolabe, DarvOptions.BlackStar, DarvOptions.CallingBell, 
                DarvOptions.DustyTome, DarvOptions.EmptyCage, DarvOptions.PandorasBox, DarvOptions.RunicPyramid, 
                DarvOptions.SneckoEye, DarvOptions.EctoplasmAct2, DarvOptions.SozuAct2]},
            {3, [DarvOptions.Any, DarvOptions.Astrolabe, DarvOptions.BlackStar, DarvOptions.CallingBell, 
                DarvOptions.DustyTome, DarvOptions.EmptyCage, DarvOptions.PandorasBox, DarvOptions.RunicPyramid, 
                DarvOptions.SneckoEye, DarvOptions.PhilosophersStoneAct3, DarvOptions.VelvetChokerAct3 ]}
        }  },
    };

    public static ImmutableArray<NeowOptions> NeowsBonesOptions =
    [
        NeowOptions.CursedPearl,
        NeowOptions.HeftyTablet,
        NeowOptions.LargeCapsule,
        NeowOptions.LeafyPoultice,
        NeowOptions.PrecariousShears,
        NeowOptions.SilkenTress,
        NeowOptions.SilverCrucible,

        NeowOptions.ArcaneScroll,
        NeowOptions.BoomingConch,
        NeowOptions.FishingRod,
        NeowOptions.GoldenPearl,
        NeowOptions.Kaleidoscope,
        NeowOptions.LeadPaperweight,
        NeowOptions.LostCoffer,
        NeowOptions.NeowsTorment,
        NeowOptions.NewLeaf,
        NeowOptions.PhialHolster,
        NeowOptions.PreciseScissors,
        NeowOptions.ScrollBoxes,
        NeowOptions.WingedBoots,

        NeowOptions.LavaRock,
        NeowOptions.NeowsTalisman,
        NeowOptions.NutritiousOyster,
        NeowOptions.Pomander,
        NeowOptions.SmallCapsule,
        NeowOptions.StoneHumidifier
    ];
}