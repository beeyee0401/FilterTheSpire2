using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.Helpers;
using FilterTheSpire2.Code.Potions;
using FilterTheSpire2.Code.Relics;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Rngs;

namespace FilterTheSpire2Tests.FilterTests;

internal static class FilterTestHelpers
{
    public static SeedSearchRequest Request(
        AscensionLevel ascensionLevel = AscensionLevel.None,
        IReadOnlyList<IFilter>? filters = null) => new()
    {
        AscensionLevel = ascensionLevel,
        Filters = filters ?? []
    };

    public static void ResetConfig()
    {
        FilterTheSpire2Config.Character = CharacterOptions.Any;
        FilterTheSpire2Config.NeowOptions = NeowOptions.Any;
        FilterTheSpire2Config.NeowsBonesRelicOption1 = NeowOptions.Any;
        FilterTheSpire2Config.NeowsBonesRelicOption2 = NeowOptions.Any;
        FilterTheSpire2Config.NeowsBonesCurseOption = CardOptions.Any;
        
        FilterTheSpire2Config.CapsuleRelicOption1 = RelicOptions.Any;
        FilterTheSpire2Config.CapsuleRelicOption2 = RelicOptions.Any;
        FilterTheSpire2Config.CapsuleRelicOption3 = RelicOptions.Any;
        
        FilterTheSpire2Config.LeadPaperweightOption = CardOptions.Any;
        FilterTheSpire2Config.NewLeafOption = CardOptions.Any;
        FilterTheSpire2Config.LeafyPoulticeOption1 = CardOptions.Any;
        FilterTheSpire2Config.LeafyPoulticeOption2 = CardOptions.Any;
        FilterTheSpire2Config.LostCofferCardOption = CardOptions.Any;
        FilterTheSpire2Config.LostCofferPotionOption = PotionOptions.Any;
        FilterTheSpire2Config.KaleidoscopeOption1 = CardOptions.Any;
        FilterTheSpire2Config.KaleidoscopeOption2 = CardOptions.Any;
        FilterTheSpire2Config.ArcaneScrollOption = CardOptions.Any;
        FilterTheSpire2Config.PhialHolsterPotionOption1 = PotionOptions.Any;
        FilterTheSpire2Config.PhialHolsterPotionOption2 = PotionOptions.Any;
        
        FilterTheSpire2Config.Act2Ancient = Ancient.Any;
        FilterTheSpire2Config.Act3Ancient = Ancient.Any;
        FilterTheSpire2Config.OrobasOptions = OrobasOptions.Any;
        FilterTheSpire2Config.PaelOptions = PaelOptions.Any;
        FilterTheSpire2Config.TezcataraOptions = TezcataraOptions.Any;
        FilterTheSpire2Config.NonupeipeOptions = NonupeipeOptions.Any;
        FilterTheSpire2Config.TanxOptions = TanxOptions.Any;
        FilterTheSpire2Config.VakuuOptions = VakuuOptions.Any;
        FilterTheSpire2Config.DarvOptions = DarvOptions.Any;

        FilterTheSpire2Config.CommonRelic = RelicOptions.Any;
        FilterTheSpire2Config.UncommonRelic = RelicOptions.Any;
        FilterTheSpire2Config.RareRelic = RelicOptions.Any;
        FilterTheSpire2Config.ShopRelic = RelicOptions.Any;

        FilterTheSpire2Config.Act1Locations = ActLocations.Any;
        FilterTheSpire2Config.Act2Locations = ActLocations.Any;
        FilterTheSpire2Config.Act3Locations = ActLocations.Any;
        
        FilterTheSpire2Config.Act1Boss = BossOptions.Any;
        FilterTheSpire2Config.Act2Boss = BossOptions.Any;
        FilterTheSpire2Config.Act3FirstBoss = BossOptions.Any;
        FilterTheSpire2Config.Act3SecondBoss = BossOptions.Any;
    }
    
    private static readonly Dictionary<
        (NeowOptions Option1, NeowOptions Option2),
        string> BonesSeedCache = [];
    
    public static string FindSeedWithBonesOptions(
        NeowOptions option1,
        NeowOptions option2)
    {
        var key = (option1, option2);

        if (BonesSeedCache.TryGetValue(key, out var cachedSeed))
        {
            return cachedSeed;
        }

        for (var candidate = 0u; candidate < 1_000_000; candidate++)
        {
            var seed = candidate.ToString();

            var options = AncientRules.NeowsBonesOptions.ToList();
            var rng = RngHelper.GetPlayerRngType(
                RngHelper.GetSeedHash(seed),
                PlayerRngType.Rewards);

            rng.Shuffle(options);

            if (options[0] != option1 || options[1] != option2)
            {
                continue;
            }

            BonesSeedCache[key] = seed;
            return seed;
        }

        Assert.Fail(
            $"Could not find a seed with Neow's Bones options " +
            $"{option1}, {option2}.");

        return null!;
    }
}