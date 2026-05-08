
using FilterTheSpire2.FilterTheSpire2Code.Filters;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

public static class SeedSearcher
{
    public static SeedSearchResult? SearchForSeed(CharacterModel character)
    {
        var filters = FilterManager.CreateFiltersFromSettings();
        if (filters.Count == 0)
        {
            return null;
        }
        
        var request = new SeedSearchRequest
        {
            Character = character,
            Filters = filters,
            ThreadCount = 4
        };
        var runner = new SeedSearchRunner(request);

        runner.Run();

        return runner.Result;
    }
}
