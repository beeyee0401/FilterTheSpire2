using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;

namespace FilterTheSpire2.Code.SeedSearcher;

public sealed class SeedSearchWorker(
    SeedSearchRunner runner,
    SeedSearchRequest request,
    ulong startSeed,
    CancellationToken token)
{
    private ulong _current = startSeed;

    public void Run()
    {
        while (!token.IsCancellationRequested)
        {
            runner.IncrementSeedsExamined();

            var result = TryRandomSeed(_current);

            if (result != null && runner.TrySetWinner(result))
            {
                return;
            }

            _current = unchecked(
                _current + (ulong)request.ThreadCount);
        }
    }

    private SeedSearchResult? TryRandomSeed(ulong candidate)
    {
        var stringSeed = RngHelper.GetRandomSeed(candidate);
        // var stringSeed = "9DAL99C3PGWJ";

        if (!FilterManager.ValidateFilters(request, stringSeed))
        {
            return null;
        }

        return new SeedSearchResult
        {
            StringSeed = stringSeed
        };
    }
}