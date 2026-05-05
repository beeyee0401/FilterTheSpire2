using FilterTheSpire2.FilterTheSpire2Code.DropdownOptions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace FilterTheSpire2.FilterTheSpire2Code.Helpers;

public static class RelicModelMappings
{
    private static readonly Dictionary<NeowOptions, RelicModel> NeowMappings = new()
    {
        { NeowOptions.ArcaneScroll, new ArcaneScroll() },
        { NeowOptions.BoomingConch, new BoomingConch() },
        { NeowOptions.CursedPearl, new CursedPearl() },
        { NeowOptions.GoldenPearl, new GoldenPearl() },
        { NeowOptions.HeftyTablet, new HeftyTablet() },
        { NeowOptions.LargeCapsule, new LargeCapsule() },
        { NeowOptions.LavaRock, new LavaRock() },
        { NeowOptions.LeadPaperweight, new LeadPaperweight() },
        { NeowOptions.LeafyPoultice, new LeafyPoultice() },
        { NeowOptions.LostCoffer, new LostCoffer() },
        { NeowOptions.MassiveScroll, new MassiveScroll() },
        { NeowOptions.NeowsTalisman, new NeowsTalisman() },
        { NeowOptions.NeowsTorment, new NeowsTorment() },
        { NeowOptions.NewLeaf, new NewLeaf() },
        { NeowOptions.NutritiousOyster, new NutritiousOyster() },
        { NeowOptions.PhialHolster, new PhialHolster() },
        { NeowOptions.Pomander, new Pomander() },
        { NeowOptions.PrecariousShears, new PrecariousShears() },
        { NeowOptions.PreciseScissors, new PreciseScissors() },
        { NeowOptions.SilverCrucible, new SilverCrucible() },
        { NeowOptions.SmallCapsule, new SmallCapsule() },
        { NeowOptions.StoneHumidifier, new StoneHumidifier() },
        { NeowOptions.NeowsBones, new NeowsBones() },
        { NeowOptions.WingedBoots, new WingedBoots() },
    };

    private static readonly Dictionary<OrobasOptions, RelicModel> OrobasMappings = new()
    {
        { OrobasOptions.AlchemicalCoffer, new AlchemicalCoffer() },
        { OrobasOptions.ArchaicTooth, new ArchaicTooth() },
        { OrobasOptions.Driftwood, new Driftwood() },
        { OrobasOptions.ElectricShrymp, new ElectricShrymp() },
        { OrobasOptions.GlassEye, new GlassEye() },
        { OrobasOptions.PrismaticGem, new PrismaticGem() },
        { OrobasOptions.RadiantPearl, new RadiantPearl() },
        { OrobasOptions.SandCastle, new SandCastle() },
        { OrobasOptions.SeaGlass, new SeaGlass() },
        { OrobasOptions.TouchOfOrobas, new TouchOfOrobas() },
    };

    private static readonly Dictionary<PaelOptions, RelicModel> PaelMappings = new()
    {
        { PaelOptions.PaelsBlood, new PaelsBlood() },
        { PaelOptions.PaelsClaw, new PaelsClaw() },
        { PaelOptions.PaelsEye, new PaelsEye() },
        { PaelOptions.PaelsFlesh, new PaelsFlesh() },
        { PaelOptions.PaelsGrowth, new PaelsGrowth() },
        { PaelOptions.PaelsHorn, new PaelsHorn() },
        { PaelOptions.PaelsLegion, new PaelsLegion() },
        { PaelOptions.PaelsTears, new PaelsTears() },
        { PaelOptions.PaelsTooth, new PaelsTooth() },
        { PaelOptions.PaelsWing, new PaelsWing() },
    };

    private static readonly Dictionary<TezcataraOptions, RelicModel> TezcataraMappings = new()
    {
        { TezcataraOptions.BiiigHug, new BiiigHug() },
        { TezcataraOptions.GoldenCompass, new GoldenCompass() },
        { TezcataraOptions.NutritiousSoup, new NutritiousSoup() },
        { TezcataraOptions.PumpkinCandle, new PumpkinCandle() },
        { TezcataraOptions.SealOfGold, new SealOfGold() },
        { TezcataraOptions.Storybook, new Storybook() },
        { TezcataraOptions.ToastyMittens, new ToastyMittens() },
        { TezcataraOptions.ToyBox, new ToyBox() },
        { TezcataraOptions.VeryHotCocoa, new VeryHotCocoa() },
        { TezcataraOptions.YummyCookie, new YummyCookie() },
    };
    
    private static readonly Dictionary<NonupeipeOptions, RelicModel> NonupeipeMappings = new()
    {
        { NonupeipeOptions.BeautifulBracelet, new BeautifulBracelet() },
        { NonupeipeOptions.BlessedAntler, new BlessedAntler() },
        { NonupeipeOptions.BrilliantScarf, new BrilliantScarf() },
        { NonupeipeOptions.DelicateFrond, new DelicateFrond() },
        { NonupeipeOptions.DiamondDiadem, new DiamondDiadem() },
        { NonupeipeOptions.FurCoat, new FurCoat() },
        { NonupeipeOptions.Glitter, new Glitter() },
        { NonupeipeOptions.JewelryBox, new JewelryBox() },
        { NonupeipeOptions.LoomingFruit, new LoomingFruit() },
        { NonupeipeOptions.SignetRing, new SignetRing() },
    };

    private static readonly Dictionary<TanxOptions, RelicModel> TanxMappings = new()
    {
        { TanxOptions.Claws, new Claws() },
        { TanxOptions.Crossbow, new Crossbow() },
        { TanxOptions.IronClub, new IronClub() },
        { TanxOptions.MeatCleaver, new MeatCleaver() },
        { TanxOptions.Sai, new Sai() },
        { TanxOptions.SpikedGauntlets, new SpikedGauntlets() },
        { TanxOptions.TanxsWhistle, new TanxsWhistle() },
        { TanxOptions.ThrowingAxe, new ThrowingAxe() },
        { TanxOptions.TriBoomerang, new TriBoomerang() },
        { TanxOptions.WarHammer, new WarHammer() },
    };

    private static readonly Dictionary<VakuuOptions, RelicModel> VakuuMappings = new()
    {
        { VakuuOptions.BloodSoakedRose, new BloodSoakedRose() },
        { VakuuOptions.ChoicesParadox, new ChoicesParadox() },
        { VakuuOptions.DistinguishedCape, new DistinguishedCape() },
        { VakuuOptions.Fiddle, new Fiddle() },
        { VakuuOptions.JeweledMask, new JeweledMask() },
        { VakuuOptions.LordsParasol, new LordsParasol() },
        { VakuuOptions.MusicBox, new MusicBox() },
        { VakuuOptions.PreservedFog, new PreservedFog() },
        { VakuuOptions.SereTalon, new SereTalon() },
        { VakuuOptions.WhisperingEarring, new WhisperingEarring() },
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
}
