using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.SeedSearcher;

public sealed class SeedSearchRunner(SeedSearchRequest request)
{
    private readonly CancellationTokenSource _cts = new();

    private int _winnerFound;
    private long _totalSeedsExamined;

    public SeedSearchResult? Result { get; private set; }

    public void Run()
    {
        var random = new MegaRandom(
            unchecked((ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));

        var baseStart = random.NextULong();

        var workers = Enumerable.Range(0, request.ThreadCount)
            .Select(workerIndex => new SeedSearchWorker(
                this,
                request,
                unchecked(baseStart + (ulong)workerIndex),
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
        {
            return false;
        }

        Result = result;

        Console.WriteLine("Winning seed found!");
        Console.WriteLine($"Seed: {result.StringSeed}");
        Console.WriteLine($"Total seeds examined: {TotalSeedsExamined:N0}");

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

    public long TotalSeedsExamined =>
        Interlocked.Read(ref _totalSeedsExamined);
}