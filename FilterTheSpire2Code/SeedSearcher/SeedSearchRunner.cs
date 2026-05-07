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
        var rng = new Rng((uint)(DateTime.UtcNow.Ticks * 100));

        var workers = Enumerable.Range(0, request.ThreadCount)
            .Select(_ => new SeedSearchWorker( 
                this,
                request,
                rng.NextUnsignedInt(),
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
        Console.WriteLine($"Timestamp: {result.SeedSourceTimestamp}");

        // Stop all workers
        _cts.Cancel();

        return true;
    }

    internal void AddSeedsExamined(long count)
    {
        Interlocked.Add(ref _totalSeedsExamined, count);
    }
}