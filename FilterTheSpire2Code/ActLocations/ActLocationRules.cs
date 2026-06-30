using System.Collections.Immutable;

namespace FilterTheSpire2.FilterTheSpire2Code.ActLocations;

public static class ActLocationRules
{
    private static readonly HashSet<ActLocations> Act1Allowed =
    [
        ActLocations.Overgrowth,
        ActLocations.Underdocks
    ];

    private static readonly HashSet<ActLocations> Act2Allowed =
    [
        ActLocations.Hive
    ];
    
    private static readonly HashSet<ActLocations> Act3Allowed =
    [
        ActLocations.Glory
    ];
    
    public static ImmutableArray<ImmutableArray<ActLocations>> ActsByIndex =
    [
        [ActLocations.Overgrowth, ActLocations.Underdocks], 
        [ActLocations.Hive],
        [ActLocations.Glory]
    ];
    
    public static bool IsValidForAct(int act, ActLocations value)
    {
        return act switch
        {
            1 => Act1Allowed.Contains(value),
            2 => Act2Allowed.Contains(value),
            3 => Act3Allowed.Contains(value),
            _ => false
        };
    }
}