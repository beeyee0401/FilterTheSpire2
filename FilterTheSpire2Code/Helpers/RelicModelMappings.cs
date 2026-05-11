using FilterTheSpire2.FilterTheSpire2Code.Ancients;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using FilterTheSpire2.FilterTheSpire2Code.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

public static class RelicModelMappings
{
    static RelicModelMappings()
    {
    }
    
    private static readonly Dictionary<NeowOptions, RelicModel> NeowMappings = new()
    {
        { NeowOptions.ArcaneScroll, ModelDb.Relic<ArcaneScroll>() },
        { NeowOptions.BoomingConch, ModelDb.Relic<BoomingConch>() },
        { NeowOptions.CursedPearl, ModelDb.Relic<CursedPearl>() },
        { NeowOptions.FishingRod, ModelDb.Relic<FishingRod>() },
        { NeowOptions.GoldenPearl, ModelDb.Relic<GoldenPearl>() },
        { NeowOptions.HeftyTablet, ModelDb.Relic<HeftyTablet>() },
        { NeowOptions.Kaleidoscope, ModelDb.Relic<Kaleidoscope>() },
        { NeowOptions.LargeCapsule, ModelDb.Relic<LargeCapsule>() },
        { NeowOptions.LavaRock, ModelDb.Relic<LavaRock>() },
        { NeowOptions.LeadPaperweight, ModelDb.Relic<LeadPaperweight>() },
        { NeowOptions.LeafyPoultice, ModelDb.Relic<LeafyPoultice>() },
        { NeowOptions.LostCoffer, ModelDb.Relic<LostCoffer>() },
        // { NeowOptions.MassiveScroll, ModelDb.Relic<MassiveScroll>() },
        { NeowOptions.NeowsTalisman, ModelDb.Relic<NeowsTalisman>() },
        { NeowOptions.NeowsTorment, ModelDb.Relic<NeowsTorment>() },
        { NeowOptions.NewLeaf, ModelDb.Relic<NewLeaf>() },
        { NeowOptions.NutritiousOyster, ModelDb.Relic<NutritiousOyster>() },
        { NeowOptions.PhialHolster, ModelDb.Relic<PhialHolster>() },
        { NeowOptions.Pomander, ModelDb.Relic<Pomander>() },
        { NeowOptions.PrecariousShears, ModelDb.Relic<PrecariousShears>() },
        { NeowOptions.PreciseScissors, ModelDb.Relic<PreciseScissors>() },
        { NeowOptions.ScrollBoxes, ModelDb.Relic<ScrollBoxes>() },
        { NeowOptions.SilkenTress, ModelDb.Relic<SilkenTress>() },
        { NeowOptions.SilverCrucible, ModelDb.Relic<SilverCrucible>() },
        { NeowOptions.SmallCapsule, ModelDb.Relic<SmallCapsule>() },
        { NeowOptions.StoneHumidifier, ModelDb.Relic<StoneHumidifier>() },
        { NeowOptions.NeowsBones, ModelDb.Relic<NeowsBones>() },
        { NeowOptions.WingedBoots, ModelDb.Relic<WingedBoots>() },
    };

    private static readonly Dictionary<OrobasOptions, RelicModel> OrobasMappings = new()
    {
        { OrobasOptions.AlchemicalCoffer, ModelDb.Relic<AlchemicalCoffer>() },
        { OrobasOptions.ArchaicTooth, ModelDb.Relic<ArchaicTooth>() },
        { OrobasOptions.Driftwood, ModelDb.Relic<Driftwood>() },
        { OrobasOptions.ElectricShrymp, ModelDb.Relic<ElectricShrymp>() },
        { OrobasOptions.GlassEye, ModelDb.Relic<GlassEye>() },
        { OrobasOptions.PrismaticGem, ModelDb.Relic<PrismaticGem>() },
        { OrobasOptions.RadiantPearl, ModelDb.Relic<RadiantPearl>() },
        { OrobasOptions.SandCastle, ModelDb.Relic<SandCastle>() },
        { OrobasOptions.SeaGlass, ModelDb.Relic<SeaGlass>() },
        { OrobasOptions.TouchOfOrobas, ModelDb.Relic<TouchOfOrobas>() },
    };

    private static readonly Dictionary<PaelOptions, RelicModel> PaelMappings = new()
    {
        { PaelOptions.PaelsBlood, ModelDb.Relic<PaelsBlood>() },
        { PaelOptions.PaelsClaw, ModelDb.Relic<PaelsClaw>() },
        { PaelOptions.PaelsEye, ModelDb.Relic<PaelsEye>() },
        { PaelOptions.PaelsFlesh, ModelDb.Relic<PaelsFlesh>() },
        { PaelOptions.PaelsGrowth, ModelDb.Relic<PaelsGrowth>() },
        { PaelOptions.PaelsHorn, ModelDb.Relic<PaelsHorn>() },
        { PaelOptions.PaelsLegion, ModelDb.Relic<PaelsLegion>() },
        { PaelOptions.PaelsTears, ModelDb.Relic<PaelsTears>() },
        { PaelOptions.PaelsTooth, ModelDb.Relic<PaelsTooth>() },
        { PaelOptions.PaelsWing, ModelDb.Relic<PaelsWing>() },
    };

    private static readonly Dictionary<TezcataraOptions, RelicModel> TezcataraMappings = new()
    {
        { TezcataraOptions.BiiigHug, ModelDb.Relic<BiiigHug>() },
        { TezcataraOptions.GoldenCompass, ModelDb.Relic<GoldenCompass>() },
        { TezcataraOptions.NutritiousSoup, ModelDb.Relic<NutritiousSoup>() },
        { TezcataraOptions.PumpkinCandle, ModelDb.Relic<PumpkinCandle>() },
        { TezcataraOptions.SealOfGold, ModelDb.Relic<SealOfGold>() },
        { TezcataraOptions.Storybook, ModelDb.Relic<Storybook>() },
        { TezcataraOptions.ToastyMittens, ModelDb.Relic<ToastyMittens>() },
        { TezcataraOptions.ToyBox, ModelDb.Relic<ToyBox>() },
        { TezcataraOptions.VeryHotCocoa, ModelDb.Relic<VeryHotCocoa>() },
        { TezcataraOptions.YummyCookie, ModelDb.Relic<YummyCookie>() },
    };

    private static readonly Dictionary<NonupeipeOptions, RelicModel> NonupeipeMappings = new()
    {
        { NonupeipeOptions.BeautifulBracelet, ModelDb.Relic<BeautifulBracelet>() },
        { NonupeipeOptions.BlessedAntler, ModelDb.Relic<BlessedAntler>() },
        { NonupeipeOptions.BrilliantScarf, ModelDb.Relic<BrilliantScarf>() },
        { NonupeipeOptions.DelicateFrond, ModelDb.Relic<DelicateFrond>() },
        { NonupeipeOptions.DiamondDiadem, ModelDb.Relic<DiamondDiadem>() },
        { NonupeipeOptions.FurCoat, ModelDb.Relic<FurCoat>() },
        { NonupeipeOptions.Glitter, ModelDb.Relic<Glitter>() },
        { NonupeipeOptions.JewelryBox, ModelDb.Relic<JewelryBox>() },
        { NonupeipeOptions.LoomingFruit, ModelDb.Relic<LoomingFruit>() },
        { NonupeipeOptions.SignetRing, ModelDb.Relic<SignetRing>() },
    };

    private static readonly Dictionary<TanxOptions, RelicModel> TanxMappings = new()
    {
        { TanxOptions.Claws, ModelDb.Relic<Claws>() },
        { TanxOptions.Crossbow, ModelDb.Relic<Crossbow>() },
        { TanxOptions.IronClub, ModelDb.Relic<IronClub>() },
        { TanxOptions.MeatCleaver, ModelDb.Relic<MeatCleaver>() },
        { TanxOptions.Sai, ModelDb.Relic<Sai>() },
        { TanxOptions.SpikedGauntlets, ModelDb.Relic<SpikedGauntlets>() },
        { TanxOptions.TanxsWhistle, ModelDb.Relic<TanxsWhistle>() },
        { TanxOptions.ThrowingAxe, ModelDb.Relic<ThrowingAxe>() },
        { TanxOptions.TriBoomerang, ModelDb.Relic<TriBoomerang>() },
        { TanxOptions.WarHammer, ModelDb.Relic<WarHammer>() },
    };

    private static readonly Dictionary<VakuuOptions, RelicModel> VakuuMappings = new()
    {
        { VakuuOptions.BloodSoakedRose, ModelDb.Relic<BloodSoakedRose>() },
        { VakuuOptions.ChoicesParadox, ModelDb.Relic<ChoicesParadox>() },
        { VakuuOptions.DistinguishedCape, ModelDb.Relic<DistinguishedCape>() },
        { VakuuOptions.Fiddle, ModelDb.Relic<Fiddle>() },
        { VakuuOptions.JeweledMask, ModelDb.Relic<JeweledMask>() },
        { VakuuOptions.LordsParasol, ModelDb.Relic<LordsParasol>() },
        { VakuuOptions.MusicBox, ModelDb.Relic<MusicBox>() },
        { VakuuOptions.PreservedFog, ModelDb.Relic<PreservedFog>() },
        { VakuuOptions.SereTalon, ModelDb.Relic<SereTalon>() },
        { VakuuOptions.WhisperingEarring, ModelDb.Relic<WhisperingEarring>() },
    };

    private static readonly Dictionary<DarvOptions, RelicModel> DarvMappings = new()
    {
        { DarvOptions.Astrolabe, ModelDb.Relic<Astrolabe>() },
        { DarvOptions.BlackStar, ModelDb.Relic<BlackStar>() },
        { DarvOptions.CallingBell, ModelDb.Relic<CallingBell>() },
        { DarvOptions.DustyTome, ModelDb.Relic<DustyTome>() },
        { DarvOptions.EmptyCage, ModelDb.Relic<EmptyCage>() },
        { DarvOptions.PandorasBox, ModelDb.Relic<PandorasBox>() },
        { DarvOptions.RunicPyramid, ModelDb.Relic<RunicPyramid>() },
        { DarvOptions.SneckoEye, ModelDb.Relic<SneckoEye>() },
        { DarvOptions.EctoplasmAct2, ModelDb.Relic<Ectoplasm>() },
        { DarvOptions.SozuAct2, ModelDb.Relic<Sozu>() },
        { DarvOptions.PhilosophersStoneAct3, ModelDb.Relic<PhilosophersStone>() },
        { DarvOptions.VelvetChokerAct3, ModelDb.Relic<VelvetChoker>() },
    };

    private static readonly Dictionary<ShopRelicOptions, RelicModel> ShopRelicMappings = new()
    {
        { ShopRelicOptions.BeltBuckle, ModelDb.Relic<BeltBuckle>() },
        { ShopRelicOptions.Bread, ModelDb.Relic<Bread>() },
        { ShopRelicOptions.BurningSticks, ModelDb.Relic<BurningSticks>() },
        { ShopRelicOptions.Cauldron, ModelDb.Relic<Cauldron>() },
        { ShopRelicOptions.ChemicalX, ModelDb.Relic<ChemicalX>() },
        { ShopRelicOptions.DingyRug, ModelDb.Relic<DingyRug>() },
        { ShopRelicOptions.DollysMirror, ModelDb.Relic<DollysMirror>() },
        { ShopRelicOptions.DragonFruit, ModelDb.Relic<DragonFruit>() },
        { ShopRelicOptions.GhostSeed, ModelDb.Relic<GhostSeed>() },
        { ShopRelicOptions.GnarledHammer, ModelDb.Relic<GnarledHammer>() },
        { ShopRelicOptions.Kifuda, ModelDb.Relic<Kifuda>() },
        { ShopRelicOptions.LavaLamp, ModelDb.Relic<LavaLamp>() },
        { ShopRelicOptions.LeesWaffle, ModelDb.Relic<LeesWaffle>() },
        { ShopRelicOptions.MembershipCard, ModelDb.Relic<MembershipCard>() },
        { ShopRelicOptions.MiniatureTent, ModelDb.Relic<MiniatureTent>() },
        { ShopRelicOptions.MysticLighter, ModelDb.Relic<MysticLighter>() },
        { ShopRelicOptions.Orrery, ModelDb.Relic<Orrery>() },
        { ShopRelicOptions.PunchDagger, ModelDb.Relic<PunchDagger>() },
        { ShopRelicOptions.RingingTriangle, ModelDb.Relic<RingingTriangle>() },
        { ShopRelicOptions.RoyalStamp, ModelDb.Relic<RoyalStamp>() },
        { ShopRelicOptions.ScreamingFlagon, ModelDb.Relic<ScreamingFlagon>() },
        { ShopRelicOptions.SlingOfCourage, ModelDb.Relic<SlingOfCourage>() },
        { ShopRelicOptions.TheAbacus, ModelDb.Relic<TheAbacus>() },
        { ShopRelicOptions.Toolbox, ModelDb.Relic<Toolbox>() },
        { ShopRelicOptions.WingCharm, ModelDb.Relic<WingCharm>() },
        { ShopRelicOptions.CharacterShopRelic, null! }
    };
    
    public static RelicModel? GetRelicModel(NeowOptions option)
    {
        return NeowMappings.GetValueOrDefault(option);
    }
    
    public static RelicModel? GetRelicModel(OrobasOptions option)
    {
        return OrobasMappings.GetValueOrDefault(option);
    }

    public static RelicModel? GetRelicModel(PaelOptions option)
    {
        return PaelMappings.GetValueOrDefault(option);
    }

    public static RelicModel? GetRelicModel(TezcataraOptions option)
    {
        return TezcataraMappings.GetValueOrDefault(option);
    }

    public static RelicModel? GetRelicModel(NonupeipeOptions option)
    {
        return NonupeipeMappings.GetValueOrDefault(option);
    }

    public static RelicModel? GetRelicModel(TanxOptions option)
    {
        return TanxMappings.GetValueOrDefault(option);
    }

    public static RelicModel? GetRelicModel(VakuuOptions option)
    {
        return VakuuMappings.GetValueOrDefault(option);
    }
    
    public static RelicModel? GetRelicModel(DarvOptions option)
    {
        return DarvMappings.GetValueOrDefault(option);
    }

    public static RelicModel? GetRelicModel(ShopRelicOptions option)
    {
        return ShopRelicMappings.GetValueOrDefault(option);
    }
}
