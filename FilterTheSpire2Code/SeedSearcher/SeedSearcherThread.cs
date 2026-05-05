using FilterTheSpire2.FilterTheSpire2Code.Filters;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

using System;
using System.Threading;

public class SeedSearcherThread
{
    private readonly SeedSearcher.MainSeedRunnable _parent;
    private readonly long _offset;

    public long SeedsExamined { get; private set; }

    public bool FoundWinningSeed { get; private set; }
    public long SeedSourceTimestamp { get; private set; }
    public string? StringSeed { get; private set; }
    public uint Seed { get; private set; }

    private static int _globalId;
    public int Id { get; }

    public SeedSearcherThread(SeedSearcher.MainSeedRunnable parent, long offset)
    {
        _parent = parent;
        _offset = offset;
        Id = Interlocked.Increment(ref _globalId) - 1;
    }

    public void Run()
    {
        while (_parent.IsRunning())
        {
            ++SeedsExamined;

            if (TryRandomSeed())
            {
                FoundWinningSeed = true;
                _parent.NotifyFinished();
            }

            // Unironically kind of like having the delay because it makes the animations cooler lol
            // Without it, there's literally no animation in the base use cases as it goes too fast and finishes in a
            // split second
            try
            {
                Thread.Sleep(10);
            }
            catch (ThreadInterruptedException e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }

    private bool TryRandomSeed()
    {
        long sTime = DateTime.UtcNow.Ticks * 100 + _offset; // nanoTime equivalent
        StringSeed = SeedHelper.GetRandomSeed();
        uint seed = (uint)StringHelper.GetDeterministicHashCode(StringSeed) + (uint)StringHelper.GetDeterministicHashCode(StringHelper.SnakeCase(nameof(RunRngType.UpFront)));

        SeedSourceTimestamp = sTime + _offset;
        Seed = seed;

        return FilterManager.ValidateFilters(StringSeed);
    }
}