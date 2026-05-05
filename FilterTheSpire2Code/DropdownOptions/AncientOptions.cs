using System.ComponentModel;

namespace FilterTheSpire2.FilterTheSpire2Code.DropdownOptions;

public enum Ancient
{
    Any,
    
    Orobas,
    Pael,
    Tezcatara,
    Darv,
    
    Nonupeipe,
    Tanx,
    Vakuu,
}

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

    public static readonly HashSet<Ancient> MultiActAncients = [Ancient.Darv];
}