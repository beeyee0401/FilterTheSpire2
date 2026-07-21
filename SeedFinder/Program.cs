using SeedFinder;

const long defaultMaximumCandidates = 100_000_000;

var startAt = ReadLongArgument(
    args,
    argumentName: "--start",
    defaultValue: 0);

var maximumCandidates = ReadLongArgument(
    args,
    argumentName: "--max",
    defaultValue: defaultMaximumCandidates);

var requestedScenario = ReadStringArgument(
    args,
    argumentName: "--scenario");

var scenarios = AncientSeedScenarios.All
    .Where(scenario =>
        requestedScenario is null ||
        scenario.ConstantName.Equals(
            requestedScenario,
            StringComparison.OrdinalIgnoreCase) ||
        scenario.Name.Contains(
            requestedScenario,
            StringComparison.OrdinalIgnoreCase))
    .ToArray();

if (scenarios.Length == 0)
{
    Console.Error.WriteLine(
        $"No scenario matched '{requestedScenario}'.");

    Console.Error.WriteLine("Available scenarios:");

    foreach (var scenario in AncientSeedScenarios.All)
    {
        Console.Error.WriteLine(
            $"  {scenario.ConstantName}: {scenario.Name}");
    }

    return 1;
}

Console.WriteLine(
    $"Searching {scenarios.Length} scenario(s) from candidate " +
    $"{startAt:N0}, up to {maximumCandidates:N0} candidates each.");
Console.WriteLine();

var finder = new AncientSeedFinder();
var results = new List<SeedSearchResult>();
var failures = new List<AncientSeedScenario>();

foreach (var scenario in scenarios)
{
    Console.WriteLine($"Searching: {scenario.Name}");

    var result = finder.FindFirst(
        scenario,
        startAt,
        maximumCandidates);

    if (result is null)
    {
        Console.WriteLine(
            $"  No result in {maximumCandidates:N0} candidates.");
        Console.WriteLine();

        failures.Add(scenario);
        continue;
    }

    Console.WriteLine($"  Seed string: {result.Seed}");
    Console.WriteLine($"  Numeric seed: {result.NumericSeed}");
    
    Console.WriteLine();

    results.Add(result);
}

Console.WriteLine("Ready-to-paste constants:");
Console.WriteLine();

foreach (var result in results)
{
    Console.WriteLine(
        $"private const string {result.ConstantName} = " +
        $"\"{EscapeCSharpString(result.Seed)}\";");
}

if (failures.Count > 0)
{
    Console.WriteLine();
    Console.WriteLine("Not found:");

    foreach (var failure in failures)
    {
        Console.WriteLine(
            $"  {failure.ConstantName}: {failure.Name}");
    }

    return 2;
}

return 0;

static long ReadLongArgument(
    string[] arguments,
    string argumentName,
    long defaultValue)
{
    var value = ReadStringArgument(arguments, argumentName);

    if (value is null)
    {
        return defaultValue;
    }

    if (!long.TryParse(value, out var parsed) || parsed < 0)
    {
        throw new ArgumentException(
            $"{argumentName} requires a non-negative integer.");
    }

    return parsed;
}

static string? ReadStringArgument(
    string[] arguments,
    string argumentName)
{
    for (var index = 0; index < arguments.Length; index++)
    {
        var argument = arguments[index];

        if (argument.Equals(
                argumentName,
                StringComparison.OrdinalIgnoreCase))
        {
            if (index + 1 >= arguments.Length)
            {
                throw new ArgumentException(
                    $"{argumentName} requires a value.");
            }

            return arguments[index + 1];
        }

        var prefix = argumentName + "=";

        if (argument.StartsWith(
                prefix,
                StringComparison.OrdinalIgnoreCase))
        {
            return argument[prefix.Length..];
        }
    }

    return null;
}

static string EscapeCSharpString(string value)
{
    return value
        .Replace("\\", "\\\\")
        .Replace("\"", "\\\"");
}