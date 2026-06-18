using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

public sealed class SeedSearchRunner(SeedSearchRequest request)
{
    private readonly CancellationTokenSource _cts = new();

    private int _winnerFound;

    private long _totalSeedsExamined;
    
    public SeedSearchResult? Result { get; private set; }

    public void Run()
    {
        var start = (uint)(DateTime.UtcNow.Ticks * 100);
        var workers = Enumerable.Range(0, request.ThreadCount)
            .Select(i => new SeedSearchWorker( 
                this,
                request,
                start + i,
                _cts.Token))
            .ToList();

        var tasks = workers
            .Select(worker => Task.Run(worker.Run))
            .ToArray();

        Task.WaitAll(tasks);

        Console.WriteLine("All seed search workers completed.");

        Console.WriteLine($"Total seeds examined: {_totalSeedsExamined}");
    }

    internal bool TrySetWinner(SeedSearchResult result)
    {
        if (Interlocked.CompareExchange(ref _winnerFound, 1, 0) != 0)
            return false;

        Result = result;

        Console.WriteLine("Winning seed found!");
        Console.WriteLine($"Seed: {result.StringSeed}");
        Console.WriteLine($"Total seeds examined: {_totalSeedsExamined}");

        // Stop all workers
        Cancel();

        return true;
    }

    internal void IncrementSeedsExamined()
    {
        Interlocked.Increment(ref _totalSeedsExamined);
    }

    public void Cancel()
    {
        _cts.Cancel();
    }
    
    public long TotalSeedsExamined => _totalSeedsExamined;
}