using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters.PotionFilters;
using FilterTheSpire2.Code.Filters.RelicOutcomeFilters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.Relics;
using FilterTheSpire2.Code.SeedSearcher;

namespace FilterTheSpire2.Code.Filters;

public static class FilterManager
{
    public static bool ValidateFilters(SeedSearchRequest request, string seed)
    {
        return request.Filters
            .All(f => f.IsSeedValid(request, seed));
    }

    public static List<IFilter> CreateFiltersFromSettings()
    {
        var filters = new List<IFilter>();

        HandleAncientFilters(filters);
        AddNeowRelicOutcomeFilters(filters);
        AddCapsuleRelicOutcomeFilter(filters);
        AddPhialHolsterPotionFilter(filters);
        AddGenericRelicFilters(filters);
        AddActLocationFilters(filters);

        return filters;
    }

    private static void HandleAncientFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.Any)
            AddAncientRelicFilterIfNeeded(
                filters,
                FilterTheSpire2Config.NeowOptions,
                NeowOptions.Any,
                1);

        switch (FilterTheSpire2Config.Act2Ancient)
        {
            case Ancient.Orobas:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.OrobasOptions,
                    OrobasOptions.Any,
                    2);
                break;

            case Ancient.Pael:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.PaelOptions,
                    PaelOptions.Any,
                    2);
                break;

            case Ancient.Tezcatara:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.TezcataraOptions,
                    TezcataraOptions.Any,
                    2);
                break;
        }

        switch (FilterTheSpire2Config.Act3Ancient)
        {
            case Ancient.Nonupeipe:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.NonupeipeOptions,
                    NonupeipeOptions.Any,
                    3);
                break;

            case Ancient.Tanx:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.TanxOptions,
                    TanxOptions.Any,
                    3);
                break;

            case Ancient.Vakuu:
                AddAncientRelicFilterIfNeeded(
                    filters,
                    FilterTheSpire2Config.VakuuOptions,
                    VakuuOptions.Any,
                    3);
                break;
        }

        if (FilterTheSpire2Config.Act2Ancient == Ancient.Darv || FilterTheSpire2Config.Act3Ancient == Ancient.Darv)
        {
            AddAncientRelicFilterIfNeeded(
                filters,
                FilterTheSpire2Config.DarvOptions,
                DarvOptions.Any,
                FilterTheSpire2Config.Act2Ancient == Ancient.Darv ? 2 : 3);
        }
    }

    private static void AddAncientRelicFilterIfNeeded<TEnum>(
        List<IFilter> filters,
        TEnum configVal,
        TEnum anyValue,
        int actNum) where TEnum : Enum
    {
        var isAnyVal = configVal.Equals(anyValue);

        filters.Add(new AncientRelicFilter(
            actNum == 2
                ? FilterTheSpire2Config.Act2Ancient
                : FilterTheSpire2Config.Act3Ancient,
            isAnyVal ? null : configVal,
            actNum));
    }

    private static void AddGenericRelicFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.ShopRelic != RelicOptions.Any)
        {
            filters.Add(new ShopRelicFilter(FilterTheSpire2Config.ShopRelic));
        }

        if (ShouldSuppressGenericRelicFilters())
        {
            return;
        }

        if (FilterTheSpire2Config.CommonRelic != RelicOptions.Any)
        {
            filters.Add(new CommonRelicFilter(FilterTheSpire2Config.CommonRelic));
        }

        if (FilterTheSpire2Config.UncommonRelic != RelicOptions.Any)
        {
            filters.Add(new UncommonRelicFilter(FilterTheSpire2Config.UncommonRelic));
        }

        if (FilterTheSpire2Config.RareRelic != RelicOptions.Any)
        {
            filters.Add(new RareRelicFilter(FilterTheSpire2Config.RareRelic));
        }
    }

    private static bool ShouldSuppressGenericRelicFilters()
    {
        return GetTotalCapsuleRelicCount() > 0 &&
               (
                   FilterTheSpire2Config.CapsuleRelicOption1 != RelicOptions.Any ||
                   FilterTheSpire2Config.CapsuleRelicOption2 != RelicOptions.Any ||
                   FilterTheSpire2Config.CapsuleRelicOption3 != RelicOptions.Any
               );
    }

    // private static void AddCapsuleRelicOutcomeFilter(List<IFilter> filters)
    // {
    //     var generatedRelicCount = GetTotalCapsuleRelicCount();
    //
    //     if (generatedRelicCount == 0)
    //     {
    //         return;
    //     }
    //
    //     var selectedRelics = new List<RelicOptions>
    //     {
    //         FilterTheSpire2Config.CapsuleRelicOption1,
    //         FilterTheSpire2Config.CapsuleRelicOption2,
    //         FilterTheSpire2Config.CapsuleRelicOption3,
    //     };
    //
    //     selectedRelics = selectedRelics
    //         .Take(generatedRelicCount)
    //         .Where(relic => relic != RelicOptions.Any)
    //         .Distinct()
    //         .ToList();
    //
    //     if (selectedRelics.Count == 0)
    //     {
    //         return;
    //     }
    //
    //     filters.Add(new CapsuleRelicFilter(
    //         selectedRelics,
    //         generatedRelicCount));
    // }

    /// <summary>
    ///     For specifically the outcome of what Neow relic is chosen. Such as card transforms or specific relics
    /// </summary>
    /// <param name="filters"></param>
    private static void AddNeowRelicOutcomeFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.NeowOptions == NeowOptions.NeowsBones)
        {
            var configuredOption1 = FilterTheSpire2Config.NeowsBonesRelicOption1;
            var configuredOption2 = FilterTheSpire2Config.NeowsBonesRelicOption2;

            var (orderedOption1, orderedOption2) = GetRequiredNeowOrder(
                configuredOption1,
                configuredOption2);

            var neowOptions = new List<NeowOptions>();
            if (orderedOption1 != NeowOptions.Any)
            {
                neowOptions.Add(orderedOption1);
            }

            if (orderedOption2 != NeowOptions.Any)
            {
                neowOptions.Add(orderedOption2);
            }

            CardOptions? curseOption = FilterTheSpire2Config.NeowsBonesCurseOption != CardOptions.Any
                ? FilterTheSpire2Config.NeowsBonesCurseOption
                : null;

            var bonesBaseConsumption = RngHelper.GetNeowsBonesBaseConsumption();
            var option1Consumption = GetDeterministicConsumption(orderedOption1);

            var requireSequence =
                orderedOption1 != NeowOptions.Any &&
                orderedOption2 != NeowOptions.Any &&
                DoRngStreamsOverlap(
                    GetRngStreams(orderedOption1),
                    GetRngStreams(orderedOption2));

            if (neowOptions.Count != 0 || curseOption != null)
            {
                filters.Add(new NeowsBonesFilter([..neowOptions], curseOption, requireSequence));
            }

            var option1Filter = BuildNeowOutcomeFilter(orderedOption1, bonesBaseConsumption);

            if (!requireSequence)
            {
                if (option1Filter != null)
                {
                    filters.Add(option1Filter);
                }

                var option2Filter = BuildNeowOutcomeFilter(orderedOption2, bonesBaseConsumption);
                if (option2Filter != null)
                {
                    filters.Add(option2Filter);
                }

                return;
            }

            if (option1Filter != null)
            {
                filters.Add(option1Filter);
            }

            if (orderedOption2 == NeowOptions.Any)
            {
                return;
            }
            
            var slot2Consumption = AddConsumption(
                bonesBaseConsumption,
                option1Consumption);

            var slot2Filter = BuildNeowOutcomeFilter(orderedOption2, slot2Consumption);
            if (slot2Filter != null)
            {
                filters.Add(slot2Filter);
            }

            return;
        }

        var directFilter = BuildNeowOutcomeFilter(FilterTheSpire2Config.NeowOptions, null);
        if (directFilter != null)
        {
            filters.Add(directFilter);
        }
    }

    private static INeowOutcomeFilter? BuildNeowOutcomeFilter(
        NeowOptions option,
        RngConsumptionSteps? slot1Consumption)
    {
        return option switch
        {
            NeowOptions.NewLeaf when FilterTheSpire2Config.NewLeafOption != CardOptions.Any =>
                new NewLeafFilter(FilterTheSpire2Config.NewLeafOption, slot1Consumption),

            NeowOptions.LeafyPoultice => BuildLeafyPoulticeFilter(slot1Consumption),

            NeowOptions.LeadPaperweight when FilterTheSpire2Config.LeadPaperweightOption != CardOptions.Any =>
                new LeadPaperweightFilter([FilterTheSpire2Config.LeadPaperweightOption], slot1Consumption),

            NeowOptions.LostCoffer when
                FilterTheSpire2Config.LostCofferCardOption != CardOptions.Any ||
                FilterTheSpire2Config.LostCofferPotionOption != PotionOptions.Any =>
                new LostCofferFilter(
                    [FilterTheSpire2Config.LostCofferCardOption],
                    FilterTheSpire2Config.LostCofferPotionOption,
                    slot1Consumption),

            NeowOptions.Kaleidoscope => BuildKaleidoscopeFilter(slot1Consumption),

            NeowOptions.ArcaneScroll when FilterTheSpire2Config.ArcaneScrollOption != CardOptions.Any =>
                new ArcaneScrollFilter([FilterTheSpire2Config.ArcaneScrollOption], slot1Consumption),

            _ => null
        };
    }

    private static INeowOutcomeFilter? BuildLeafyPoulticeFilter(RngConsumptionSteps? slot1Consumption)
    {
        var cardOptions = new List<CardOptions>();
        if (FilterTheSpire2Config.LeafyPoulticeOption1 != CardOptions.Any)
        {
            cardOptions.Add(FilterTheSpire2Config.LeafyPoulticeOption1);
        }

        if (FilterTheSpire2Config.LeafyPoulticeOption2 != CardOptions.Any)
        {
            cardOptions.Add(FilterTheSpire2Config.LeafyPoulticeOption2);
        }

        return cardOptions.Count > 0
            ? new LeafyPoulticeFilter(cardOptions, slot1Consumption)
            : null;
    }

    private static INeowOutcomeFilter? BuildKaleidoscopeFilter(RngConsumptionSteps? slot1Consumption)
    {
        var cardOptions = new List<CardOptions>();
        if (FilterTheSpire2Config.KaleidoscopeOption1 != CardOptions.Any)
        {
            cardOptions.Add(FilterTheSpire2Config.KaleidoscopeOption1);
        }

        if (FilterTheSpire2Config.KaleidoscopeOption2 != CardOptions.Any)
        {
            cardOptions.Add(FilterTheSpire2Config.KaleidoscopeOption2);
        }

        return cardOptions.Count > 0
            ? new KaleidoscopeFilter(cardOptions, slot1Consumption)
            : null;
    }

    private static void AddActLocationFilters(List<IFilter> filters)
    {
        if (FilterTheSpire2Config.Act1Locations != ActLocations.Any)
        {
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act1Locations, 1));
        }

        if (FilterTheSpire2Config.Act2Locations != ActLocations.Any)
        {
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act2Locations, 2));
        }

        if (FilterTheSpire2Config.Act3Locations != ActLocations.Any)
        {
            filters.Add(new ActLocationFilter(FilterTheSpire2Config.Act3Locations, 3));
        }
    }

    private static void AddCapsuleRelicOutcomeFilter(List<IFilter> filters)
    {
        var generatedRelicCount = GetTotalCapsuleRelicCount();

        if (generatedRelicCount == 0)
        {
            return;
        }

        var selectedRelics = new List<RelicOptions>
            {
                FilterTheSpire2Config.CapsuleRelicOption1,
                FilterTheSpire2Config.CapsuleRelicOption2,
                FilterTheSpire2Config.CapsuleRelicOption3,
            }
            .Take(generatedRelicCount)
            // Intentionally collapse Any slots. Capsule relic filters are treated as
            // "match these selected relics in order from the first generated relic",
            // not as positional slot constraints.
            .Where(relic => relic != RelicOptions.Any)
            .Distinct()
            .ToList();

        if (selectedRelics.Count == 0)
        {
            return;
        }

        filters.Add(new CapsuleRelicFilter(
            selectedRelics,
            generatedRelicCount,
            GetCapsulePriorConsumption()));
    }

    #region helpers

    [Flags]
    private enum RngStreams
    {
        None = 0,
        Rewards = 1 << 0,
        Transformations = 1 << 1,
        Niche = 1 << 2,
        CombatPotionGeneration = 1 << 3
    }

    private static RngStreams GetRngStreams(NeowOptions option)
    {
        return option switch
        {
            NeowOptions.SmallCapsule or
                NeowOptions.LargeCapsule or
                NeowOptions.LeadPaperweight or
                NeowOptions.LostCoffer or
                NeowOptions.ArcaneScroll or
                NeowOptions.ScrollBoxes => RngStreams.Rewards,

            NeowOptions.Kaleidoscope => RngStreams.Rewards | RngStreams.Niche,
            NeowOptions.LeafyPoultice => RngStreams.Transformations,
            NeowOptions.NewLeaf => RngStreams.Niche,
            NeowOptions.PhialHolster => RngStreams.CombatPotionGeneration,
            _ => RngStreams.None
        };
    }

    private static bool DoRngStreamsOverlap(RngStreams a, RngStreams b)
    {
        return (a & b) != RngStreams.None;
    }

    private static RngConsumptionSteps GetDeterministicConsumption(NeowOptions option)
    {
        return option switch
        {
            NeowOptions.SmallCapsule => new RngConsumptionSteps(1, 0, 0),
            NeowOptions.LargeCapsule => new RngConsumptionSteps(2, 0, 0),
            NeowOptions.LeadPaperweight => new RngConsumptionSteps(6, 0, 0),
            // 9 for card reward, 2 for potion
            NeowOptions.LostCoffer => new RngConsumptionSteps(11, 0, 0),
            NeowOptions.Kaleidoscope => new RngConsumptionSteps(18, 0, 6),
            NeowOptions.ArcaneScroll => new RngConsumptionSteps(1, 0, 0),
            NeowOptions.LeafyPoultice => new RngConsumptionSteps(0, 2, 0),
            NeowOptions.NewLeaf => new RngConsumptionSteps(0, 0, 1),
            NeowOptions.PhialHolster => new RngConsumptionSteps(0, 0, 0, 4),

            NeowOptions.ScrollBoxes => RngConsumptionSteps.None,
            _ => RngConsumptionSteps.None
        };
    }
    
    private static void AddPhialHolsterPotionFilter(List<IFilter> filters)
    {
        var selectedPotions = new List<PotionOptions>
            {
                FilterTheSpire2Config.PhialHolsterPotionOption1,
                FilterTheSpire2Config.PhialHolsterPotionOption2,
            }
            .Where(potion => potion != PotionOptions.Any)
            .Distinct()
            .ToList();

        if (selectedPotions.Count == 0)
        {
            return;
        }

        if (FilterTheSpire2Config.NeowOptions == NeowOptions.PhialHolster)
        {
            filters.Add(new PhialHolsterFilter(selectedPotions));
            return;
        }

        if (FilterTheSpire2Config.NeowOptions != NeowOptions.NeowsBones)
        {
            return;
        }

        var option1 = FilterTheSpire2Config.NeowsBonesRelicOption1;
        var option2 = FilterTheSpire2Config.NeowsBonesRelicOption2;

        if (option1 == NeowOptions.PhialHolster)
        {
            filters.Add(new PhialHolsterFilter(
                selectedPotions,
                RngHelper.GetNeowsBonesBaseConsumption().CombatPotionGenerationRngSteps));
        }
        else if (option2 == NeowOptions.PhialHolster)
        {
            var priorConsumption = AddConsumption(
                RngHelper.GetNeowsBonesBaseConsumption(),
                GetDeterministicConsumption(option1));

            filters.Add(new PhialHolsterFilter(
                selectedPotions,
                priorConsumption.CombatPotionGenerationRngSteps));
        }
    }

    private static RngConsumptionSteps GetCapsulePriorConsumption()
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.NeowsBones)
        {
            return RngConsumptionSteps.None;
        }

        var configuredOption1 = FilterTheSpire2Config.NeowsBonesRelicOption1;
        var configuredOption2 = FilterTheSpire2Config.NeowsBonesRelicOption2;

        var (orderedOption1, _) = GetRequiredNeowOrder(
            configuredOption1,
            configuredOption2);

        var bonesBaseConsumption = RngHelper.GetNeowsBonesBaseConsumption();

        if (GetCapsuleRelicCount(orderedOption1) > 0)
        {
            return bonesBaseConsumption;
        }

        return AddConsumption(
            bonesBaseConsumption,
            GetDeterministicConsumption(orderedOption1));
    }

    

    private static (NeowOptions Option1, NeowOptions Option2) GetRequiredNeowOrder(
        NeowOptions option1,
        NeowOptions option2)
    {
        if (option1 == NeowOptions.ScrollBoxes &&
            DoRngStreamsOverlap(GetRngStreams(option1), GetRngStreams(option2)))
        {
            return (option2, option1);
        }

        return (option1, option2);
    }

    private static int GetTotalCapsuleRelicCount()
    {
        if (FilterTheSpire2Config.NeowOptions != NeowOptions.NeowsBones)
        {
            return GetCapsuleRelicCount(FilterTheSpire2Config.NeowOptions);
        }

        return GetCapsuleRelicCount(FilterTheSpire2Config.NeowsBonesRelicOption1) +
               GetCapsuleRelicCount(FilterTheSpire2Config.NeowsBonesRelicOption2);
    }

    public static int GetCapsuleRelicCount(NeowOptions option)
    {
        return option switch
        {
            NeowOptions.SmallCapsule => 1,
            NeowOptions.LargeCapsule => 2,
            _ => 0
        };
    }

    private static RngConsumptionSteps AddConsumption(
        RngConsumptionSteps a,
        RngConsumptionSteps b)
    {
        return new RngConsumptionSteps(
            RewardsRngSteps: a.RewardsRngSteps + b.RewardsRngSteps,
            TransformationsRngSteps: a.TransformationsRngSteps + b.TransformationsRngSteps,
            NicheRngSteps: a.NicheRngSteps + b.NicheRngSteps,
            CombatPotionGenerationRngSteps: a.CombatPotionGenerationRngSteps + b.CombatPotionGenerationRngSteps);
    }

    #endregion
}