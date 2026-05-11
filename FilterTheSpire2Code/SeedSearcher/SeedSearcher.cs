
using FilterTheSpire2.FilterTheSpire2Code.Filters;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;

public class SeedSearcher
{
    public SeedSearchRunner? Runner { get; private set; }
    
    public SeedSearchResult? SearchForSeed(CharacterModel character)
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
            ThreadCount = 6
        };
        Runner = new SeedSearchRunner(request);

        Runner.Run();

        return Runner.Result;
    }

    public void Cancel()
    {
        Runner?.Cancel();
    }
}
