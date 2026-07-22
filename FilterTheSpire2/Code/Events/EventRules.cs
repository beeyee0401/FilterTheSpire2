using System.Collections.Immutable;
using FilterTheSpire2.Code.Acts;

namespace FilterTheSpire2.Code.Events;

public static class EventRules
{
    // Order matches ModelDb.AllSharedEvents. WarHistorianRepy is deliberately included here for
    // completeness of the "shared" concept, but is always excluded from the actual shuffle pools
    // below — its IsAllowed always returns false; it only appears via the Lantern Key quest card
    // override (see GetValidEvents / FirstEventFilter), never through the normal shuffle.
    public static readonly ImmutableArray<EventOptions> SharedEvents =
    [
        EventOptions.BrainLeech,
        EventOptions.CrystalSphere,
        EventOptions.DollRoom,
        EventOptions.FakeMerchant,
        EventOptions.PotionCourier,
        EventOptions.RanwidTheElder,
        EventOptions.RelicTrader,
        EventOptions.RoomFullOfCheese,
        EventOptions.SelfHelpBook,
        EventOptions.SlipperyBridge,
        EventOptions.StoneOfAllTime,
        EventOptions.Symbiote,
        EventOptions.TeaMaster,
        EventOptions.TheFutureOfPotions,
        EventOptions.TheLegendsWereTrue,
        EventOptions.ThisOrThat,
        EventOptions.WarHistorianRepy,
        EventOptions.WelcomeToWongos,
    ];

    /// <summary>
    /// Act-index (0-based) restrictions per shared event, taken directly from each EventModel's
    /// IsAllowed override. Gold/HP/potion-count/deck-size conditions are intentionally not modeled —
    /// only act gating. Events absent from this dictionary have no act restriction.
    /// </summary>
    private static readonly Dictionary<EventOptions, HashSet<int>> ActIndexRestrictions = new()
    {
        { EventOptions.BrainLeech, [0, 1] },        // CurrentActIndex < 2
        { EventOptions.CrystalSphere, [1, 2] },     // CurrentActIndex > 0
        { EventOptions.DollRoom, [1] },              // CurrentActIndex == 1
        { EventOptions.FakeMerchant, [1, 2] },      // CurrentActIndex >= 1
        { EventOptions.PotionCourier, [1, 2] },     // CurrentActIndex > 0
        { EventOptions.RanwidTheElder, [1, 2] },    // CurrentActIndex != 0
        { EventOptions.RelicTrader, [1, 2] },       // CurrentActIndex != 0
        { EventOptions.RoomFullOfCheese, [0, 1] },  // CurrentActIndex < 2
        { EventOptions.StoneOfAllTime, [1] },        // CurrentActIndex == 1
        { EventOptions.Symbiote, [1, 2] },          // CurrentActIndex > 0
        { EventOptions.TeaMaster, [0, 1] },         // CurrentActIndex < 2
        { EventOptions.TheLegendsWereTrue, [0] },    // CurrentActIndex == 0
        { EventOptions.WelcomeToWongos, [1] },       // CurrentActIndex == 1
        // SelfHelpBook, ThisOrThat, TheFutureOfPotions: no act gate to model (only non-act conditions,
        // or IsAllowed not yet provided) — treated as unrestricted until confirmed otherwise.
    };

    public static bool IsAllowedForActIndex(EventOptions option, int actIndex)
    {
        return !ActIndexRestrictions.TryGetValue(option, out var allowed) || allowed.Contains(actIndex);
    }

    private static ImmutableArray<EventOptions> GetOwnEventsForLocation(ActLocations location) => location switch
    {
        ActLocations.Overgrowth => ActDefinition.Overgrowth.ActEvents,
        ActLocations.Underdocks => ActDefinition.Underdocks.ActEvents,
        ActLocations.Hive => ActDefinition.Hive.ActEvents,
        ActLocations.Glory => ActDefinition.Glory.ActEvents,
        _ => ImmutableArray<EventOptions>.Empty
    };

    /// <summary>
    /// Events valid to filter on for an act given its currently configured location — mirrors
    /// BossRules.GetValidBosses. Includes the act-gated shared pool, and (Act 3 only) the
    /// WarHistorianRepy override option, which is never part of the natural shuffle but is always
    /// the first event of Act 3 if the player holds the Lantern Key quest card.
    /// </summary>
    public static IEnumerable<EventOptions> GetValidEvents(int actNum, ActLocations selectedLocation)
    {
        var actIndex = actNum - 1;

        var ownEvents = selectedLocation == ActLocations.Any
            ? ActLocationRules.ActsByIndex[actIndex].SelectMany(a => GetOwnEventsForLocation(a)).Distinct()
            : GetOwnEventsForLocation(selectedLocation);

        var sharedForAct = SharedEvents
            .Where(e => e != EventOptions.WarHistorianRepy && IsAllowedForActIndex(e, actIndex));

        var result = ownEvents.Concat(sharedForAct);

        return result;
    }
}