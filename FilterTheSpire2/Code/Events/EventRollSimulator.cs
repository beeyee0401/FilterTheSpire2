using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

namespace FilterTheSpire2.Code.Events;

public static class EventRollSimulator
{
    private const float MonsterThreshold = 0.10f;
    private const float TreasureThreshold = 0.12f;
    private const float ShopThreshold = 0.15f;

    public static bool RollIsEvent(Rng rng)
    {
        ArgumentNullException.ThrowIfNull(rng);

        return ResolveRoll(rng.NextFloat()) == RoomType.Event;
    }

    private static RoomType ResolveRoll(float roll)
    {
        return roll switch
        {
            <= MonsterThreshold => RoomType.Monster,
            <= TreasureThreshold => RoomType.Treasure,
            <= ShopThreshold => RoomType.Shop,
            _ => RoomType.Event
        };
    }
}