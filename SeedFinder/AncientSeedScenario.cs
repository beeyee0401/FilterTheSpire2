using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Ancients.Filtering;
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Ascension;

namespace SeedFinder;

internal sealed record AncientSeedScenario(
    string Name,
    string ConstantName,
    Func<string, ulong, bool> Matches);

internal static class AncientSeedScenarios
{
    public static IReadOnlyList<AncientSeedScenario> All { get; } =
    [
        DirectOption(
            name: "Neow rolls Lost Coffer",
            constantName: "SeedKnownOutcome",
            ancient: Ancient.Neow,
            option: NeowOptions.LostCoffer,
            actNumber: 1),

        FilterOutcome(
            name: "Act 1 Neow rolls Lost Coffer",
            constantName: "SeedAct1NeowKnown",
            ancient: Ancient.Neow,
            option: NeowOptions.LostCoffer,
            actNumber: 1),

        FilterOutcome(
            name: "Act 2 rolls Orobas with Touch of Orobas",
            constantName: "SeedAct2RollsOrobas",
            ancient: Ancient.Orobas,
            option: OrobasOptions.TouchOfOrobas,
            actNumber: 2),

        FilterOutcome(
            name: "Act 3 rolls Vakuu with Fiddle",
            constantName: "SeedAct3RollsVakuu",
            ancient: Ancient.Vakuu,
            option: VakuuOptions.Fiddle,
            actNumber: 3),

        FilterOutcome(
            name: "Act 2 rolls Darv with Snecko Eye",
            constantName: "SeedAct2RollsDarv",
            ancient: Ancient.Darv,
            option: DarvOptions.SneckoEye,
            actNumber: 2),

        FilterOutcome(
            name: "Act 3 rolls Darv with Philosopher's Stone",
            constantName: "SeedAct3RollsDarv",
            ancient: Ancient.Darv,
            option: DarvOptions.PhilosophersStone,
            actNumber: 3)
    ];

    private static AncientSeedScenario DirectOption<TOption>(
        string name,
        string constantName,
        Ancient ancient,
        TOption option,
        int actNumber)
        where TOption : struct, Enum
    {
        return new AncientSeedScenario(
            name,
            constantName,
            (_, numericSeed) =>
            {
                var ancientFilter = AncientFactory.GetAncient(
                    ancient,
                    actNumber);

                return ancientFilter.CheckOptions(numericSeed, option);
            });
    }

    private static AncientSeedScenario FilterOutcome<TOption>(
        string name,
        string constantName,
        Ancient ancient,
        TOption option,
        int actNumber)
        where TOption : struct, Enum
    {
        return new AncientSeedScenario(
            name,
            constantName,
            (seed, _) =>
            {
                var request = CreateRequest();

                var filter = new AncientRelicFilter(
                    ancient,
                    option,
                    actNumber);

                return filter.IsSeedValid(request, seed);
            });
    }

    private static SeedSearchRequest CreateRequest(IReadOnlyList<IFilter>? filters = null)
    {
        return new SeedSearchRequest()
        {
            AscensionLevel = AscensionLevel.None,
            Filters = filters ?? []
        };
    }
}