using System.Collections.Immutable;
using FilterTheSpire2.FilterTheSpire2Code.Cards;
using FilterTheSpire2.FilterTheSpire2Code.Characters;
using FilterTheSpire2.FilterTheSpire2Code.Config;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.FilterTheSpire2Code.Filters;

public abstract class BaseCardTransformFilter(List<CardOptions> cardOptions, int transformCount) : IFilter
{
    protected abstract Rng GetTransformRng(uint seed);
    protected abstract ImmutableArray<CardOptions> GetCardPool();
    
    public bool IsSeedValid(SeedSearchRequest request, string seed)
    {
        var requestedCards = cardOptions
            .Where(c => c != CardOptions.Any)
            .ToList();
        if (requestedCards.Count == 0 || requestedCards.Count > transformCount)
        {
            return true;
        }

        if (FilterTheSpire2Config.Character == CharacterOptions.Any)
        {
            return true;
        }
        
        var baseRng = new Rng((uint) StringHelper.GetDeterministicHashCode(seed));
        var rng = GetTransformRng(baseRng.Seed);
        var cardPool = GetCardPool();
        var remaining = requestedCards
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        for (var i = 0; i < transformCount; i++)
        {
            var rolled = rng.NextItem(cardPool);

            if (!remaining.TryGetValue(rolled, out var count))
            {
                continue;
            }

            if (count == 1)
            {
                remaining.Remove(rolled);

                // Early success once all requested cards have been found.
                if (remaining.Count == 0)
                {
                    return true;
                }
            }
            else
            {
                remaining[rolled] = count - 1;
            }
        }

        return remaining.Count == 0;
    }
}