using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.SeedSearcher;

public sealed class SeedSearchRunner(SeedSearchRequest request)
{
    private readonly CancellationTokenSource _cts = new();

    private int _winnerFound;

    private long _totalSeedsExamined;
    
    public SeedSearchResult? Result { get; private set; }

    private const ulong UInt32SeedSpace = uint.MaxValue + 1UL;

    public void Run()
    {
        var rng = new MegaRandom((ulong)DateTimeOffset.Now.ToUnixTimeSeconds());
        var start = (uint)(DateTime.UtcNow.Ticks * rng.Next(1, 100));

        var baseStart = (ulong)start;
        var endExclusive = baseStart + UInt32SeedSpace;

        var workers = Enumerable.Range(0, request.ThreadCount)
            .Select(i => new SeedSearchWorker(
                this,
                request,
                baseStart + (ulong)i,
                endExclusive,
                _cts.Token))
            .ToList();

        var tasks = workers
            .Select(worker => Task.Run(worker.Run))
            .ToArray();

        Task.WaitAll(tasks);

        Console.WriteLine("All seed search workers completed.");
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