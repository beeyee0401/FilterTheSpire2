using System.Diagnostics;
using FilterTheSpire2.Code.Helpers;

namespace SeedFinder;

internal sealed class AncientSeedFinder
{
    public SeedSearchResult? FindFirst(
        AncientSeedScenario scenario,
        long startAt,
        long maximumCandidates,
        int progressInterval = 1_000_000)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        if (startAt < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(startAt),
                "The starting candidate cannot be negative.");
        }

        if (maximumCandidates <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumCandidates),
                "At least one candidate must be searched.");
        }

        var stopwatch = Stopwatch.StartNew();

        for (long offset = 0; offset < maximumCandidates; offset++)
        {
            var candidateNumber = checked(startAt + offset);
            var seed = candidateNumber.ToString();
            var numericSeed = RngHelper.GetSeedHash(seed);

            if (scenario.Matches(seed, numericSeed))
            {
                stopwatch.Stop();

                return new SeedSearchResult(
                    scenario.Name,
                    scenario.ConstantName,
                    seed,
                    numericSeed,
                    offset + 1,
                    stopwatch.Elapsed);
            }

            if (progressInterval > 0 &&
                offset > 0 &&
                offset % progressInterval == 0)
            {
                Console.WriteLine(
                    $"{scenario.Name}: searched {offset:N0} candidates...");
            }
        }

        stopwatch.Stop();
        return null;
    }
}

internal sealed record SeedSearchResult(
    string ScenarioName,
    string ConstantName,
    string Seed,
    ulong NumericSeed,
    long CandidatesSearched,
    TimeSpan Elapsed);