using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;

namespace FilterTheSpire2.Code.SeedSearcher;

public sealed class SeedSearchWorker(
    SeedSearchRunner runner,
    SeedSearchRequest request,
    long startSeed,
    CancellationToken token)
{
    private long _current;
    
    public void Run()
    {
        _current = startSeed;
        while (!token.IsCancellationRequested)
        {
            runner.IncrementSeedsExamined();

            var result = TryRandomSeed(_current);

            if (result != null)
            {
                var won = runner.TrySetWinner(result);

                if (won)
                {
                    break;
                }
            }

            _current += request.ThreadCount;
        }
    }

    private SeedSearchResult? TryRandomSeed(long candidate)
    {
        var stringSeed = RngHelper.GetRandomSeed(candidate);
        // var stringSeed = "H8G58Z3P8H";

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
        };
    }
}