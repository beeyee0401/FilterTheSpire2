using FilterTheSpire2.FilterTheSpire2Code.Filters;
using FilterTheSpire2.FilterTheSpire2Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

public sealed class SeedSearchWorker(
    SeedSearchRunner runner,
    SeedSearchRequest request,
    long offset,
    CancellationToken token)
{
    public void Run()
    {
        long seedsExamined = 0;

        while (!token.IsCancellationRequested)
        {
            seedsExamined++;
            runner.AddSeedsExamined(seedsExamined);

            var result = TryRandomSeed();

            if (result != null)
            {
                var won = runner.TrySetWinner(result);

                if (won)
                {
                    break;
                }
            }

            // Optional pacing for visual/debug purposes
            if (request.DelayMs > 0)
            {
                Thread.Sleep(request.DelayMs);
            }
        }
    }

    private SeedSearchResult? TryRandomSeed()
    {
        var timestamp = DateTime.UtcNow.Ticks * 100 + offset;

        var stringSeed = RngHelper.GetRandomSeed(offset);
        // var stringSeed = "7Y6Y0L923U";

        var seed =
            (uint)StringHelper.GetDeterministicHashCode(stringSeed) +
            (uint)StringHelper.GetDeterministicHashCode(
                StringHelper.SnakeCase(nameof(RunRngType.UpFront)));

        var passed = FilterManager.ValidateFilters(request, stringSeed);

        if (!passed)
        {
            return null;
        }

        return new SeedSearchResult
        {
            StringSeed = stringSeed,
            Seed = seed,
            SeedSourceTimestamp = timestamp
        };
    }
}