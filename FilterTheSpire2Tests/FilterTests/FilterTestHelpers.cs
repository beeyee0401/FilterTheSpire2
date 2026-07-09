using FilterTheSpire2.Code.Acts;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Cards;
using FilterTheSpire2.Code.Characters;
using FilterTheSpire2.Code.Config;
using FilterTheSpire2.Code.Relics;
using FilterTheSpire2.Code.SeedSearcher;
using MegaCrit.Sts2.Core.Entities.Ascension;

namespace FilterTheSpire2Tests.FilterTests;

internal static class FilterTestHelpers
{
    public static SeedSearchRequest Request(AscensionLevel ascensionLevel = AscensionLevel.None) => new()
    {
        AscensionLevel = ascensionLevel,
        Filters = []
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
        FilterTheSpire2Config.LostCofferOption = CardOptions.Any;
        FilterTheSpire2Config.KaleidoscopeOption1 = CardOptions.Any;
        FilterTheSpire2Config.KaleidoscopeOption2 = CardOptions.Any;
        FilterTheSpire2Config.ArcaneScrollOption = CardOptions.Any;

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
    }
}