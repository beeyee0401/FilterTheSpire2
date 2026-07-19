using System.Collections.Immutable;

namespace FilterTheSpire2.Code.Acts;

public static class BossRules
{
    private static readonly Dictionary<ActLocations, ImmutableArray<BossOptions>> BossOrderByLocation = new()
    {
        [ActLocations.Overgrowth] = [BossOptions.CeremonialBeast, BossOptions.TheKin, BossOptions.Vantom],
        [ActLocations.Underdocks] = [BossOptions.LagavulinMatriarch, BossOptions.SoulFysh, BossOptions.WaterfallGiant],
        [ActLocations.Hive] = [BossOptions.KaiserCrab, BossOptions.KnowledgeDemon, BossOptions.TheInsatiable],
        [ActLocations.Glory] = [BossOptions.Aeonglass, BossOptions.Queen, BossOptions.TestSubject]
    };

    private static ImmutableArray<BossOptions> GetBossesForLocation(ActLocations location) =>
        BossOrderByLocation.GetValueOrDefault(location, ImmutableArray<BossOptions>.Empty);

    /// <summary>
    /// All bosses reachable for a given act (1-3), across every location that act could roll.
    /// </summary>
    private static IEnumerable<BossOptions> GetPossibleBosses(int actNum)
    {
        return ActLocationRules.ActsByIndex[actNum - 1]
            .SelectMany(a => GetBossesForLocation(a))
            .Distinct();
    }

    /// <summary>
    /// Bosses valid to filter on for an act given its currently configured location.
    /// ActLocations.Any means the location itself hasn't been narrowed, so every boss reachable by
    /// any of that act's possible locations remains valid — same convention as ActLocationFilter.
    /// </summary>
    public static IEnumerable<BossOptions> GetValidBosses(int actNum, ActLocations selectedLocation)
    {
        return selectedLocation == ActLocations.Any
            ? GetPossibleBosses(actNum)
            : GetBossesForLocation(selectedLocation);
    }
    
    public static ActLocations GetLocationForBoss(BossOptions boss)
    {
        foreach (var (location, bosses) in BossOrderByLocation)
        {
            if (bosses.Contains(boss))
            {
                return location;
            }
        }

        return ActLocations.Any;
    }
}