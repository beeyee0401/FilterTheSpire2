using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public static class SeedSearcher
{
    private static int _numThreads = 4;

    public static string? FoundSeed; 

    // --------------------------------------------------------------------------------

    // This runner is responsible for initiating and joining all the child tasks - the children do the work
    //    testing all the seeds. This is responsible for making them all quit once the target is found by any of
    //    the children.
    public class MainSeedRunnable
    {
        private volatile bool _running = true;
        public bool Finished { get; private set; } = false;
        public List<SeedSearcherThread> Threads { get; } = new List<SeedSearcherThread>();

        public void Run()
        {
            var rng = new Rng((uint)(DateTime.UtcNow.Ticks * 100)); // System.nanoTime() equivalent

            for (var i = 0; i < _numThreads; ++i)
                Threads.Add(new SeedSearcherThread(this, rng.NextUnsignedInt()));

            Task[] tasks = Threads
                .Select(task => Task.Run(task.Run))
                .ToArray();

            Task.WaitAll(tasks);

            Console.WriteLine("All tasks completed.");
            Finished = true;
        }

        public void NotifyFinished() { _running = false; }
        public bool IsRunning() { return _running; }

        public string GetTotalNumSeedsExamined()
        {
            var sum = Threads.Sum(t => t.SeedsExamined);
            if (sum <= _numThreads)
                sum = 1;
            return sum.ToString();
        }
    }

    private static MainSeedRunnable? _runner;
    private static Thread? _runnerThread;

    // --------------------------------------------------------------------------------

    // The main starting point of the searching algorithm - launches a new thread to handle further child threads
    public static void SearchForSeed()
    {
        _numThreads = 4;
        _runner = new MainSeedRunnable();
        _runnerThread = new Thread(_runner.Run);
        _runnerThread.Start();
    }

    // --------------------------------------------------------------------------------

    // Info getters (for main StS thread to hook into)
    public static string GetNumSeedsExamined() => _runner != null ? _runner.GetTotalNumSeedsExamined() : "ERROR";
    public static bool IsFinished() => _runner is { Finished: true };

    // --------------------------------------------------------------------------------

    // Makes the found seed into a "real" one - by letting the game know it is the seed to use and forcing a restart
    public static void MakeSeedReal()
    {
        if (_runner == null)
            return;

        foreach (var t in _runner.Threads.Where(t => t.FoundWinningSeed))
        {
            Console.WriteLine("Updating the seed to match a winning result:");
            Console.WriteLine("Seed: " + t.StringSeed);
            Console.WriteLine("Timestamp: " + t.SeedSourceTimestamp);

            FoundSeed = t.StringSeed;
            // Settings.Seed = t.Seed;
            // Settings.SeedSet = true;
            // Settings.SeedSourceTimestamp = t.SeedSourceTimestamp;
            // SeedHelper.CachedSeed = null;

            return;
        }

        Console.WriteLine("ERROR: no winning seed found - called before finished?");
    }
}
