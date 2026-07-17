using System.Collections.Immutable;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace FilterTheSpire2.Code.Filters;

public abstract class BaseCardTransformFilter(
    List<CardOptions> cardOptions,
    int transformCount,
    RngConsumptionSteps? slot1Consumption = null) : INeowOutcomeFilter
{
    protected abstract Rng GetTransformRng(ulong seed);
    protected abstract ImmutableArray<CardOptions> GetCardPool();
    public abstract RngConsumptionSteps RngConsumptionSteps { get; }

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

        var rng = GetTransformRng(RngHelper.GetSeedHash(seed));

        // Fast-forward past slot 1's consumption if we are slot 2
        if (slot1Consumption != null)
            FastForward(rng, slot1Consumption);

        var cardPool = GetCardPool();
        var remaining = requestedCards
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        for (var i = 0; i < transformCount; i++)
        {
            var rolled = rng.NextItem(cardPool);
            if (!remaining.TryGetValue(rolled, out var count)) continue;
            if (count == 1) { remaining.Remove(rolled); if (remaining.Count == 0) return true; }
            else remaining[rolled] = count - 1;
        }

        return remaining.Count == 0;
    }

    protected virtual void FastForward(Rng rng, RngConsumptionSteps consumptionSteps)
    {
        // Subclasses override to fast-forward the specific stream this filter uses.
        // Default: no-op (streams that this filter doesn't need fast forwarding).
    }
}