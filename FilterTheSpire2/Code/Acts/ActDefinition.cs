using System.Collections.Immutable;
using FilterTheSpire2.Code.Ancients.Config;
using FilterTheSpire2.Code.Events;
using FilterTheSpire2.Code.Helpers;
using MegaCrit.Sts2.Core.Entities.Encounters;

namespace FilterTheSpire2.Code.Acts;

public class ActDefinition
{
    public required Ancient[] NativeAncients { get; init; }

    public required SimpleEncounterDef[] WeakEncounters { get; init; }

    public required SimpleEncounterDef[] RegularEncounters { get; init; }

    public required SimpleEncounterDef[] EliteEncounters { get; init; }

    public required ImmutableArray<BossOptions> Bosses { get; init; }

    /// <summary>
    /// This act's own events, in the same order as ActModel.AllEvents. Does not include the shared
    /// event pool — see EventRules.GetEventPool for the combined pool used at generation time.
    /// </summary>
    public required ImmutableArray<EventOptions> ActEvents { get; init; }

    public required int WeakEncounterCount { get; init; }

    /// <summary>
    /// Total number of rooms in the act (excluding Ancient/Boss rooms).
    /// </summary>
    public required int RoomCount { get; init; }

    /// <summary>
    /// Populated after the run chooses which shared ancients belong to this act.
    /// </summary>
    public List<Ancient> SharedAncients { get; } = [];
    
    public Ancient? RolledAncient { get; set; }
    public BossOptions? RolledBoss { get; set; }
    public BossOptions? RolledSecondBoss { get; set; } // only ever set on the last act, at Double Boss+
    public EventOptions? RolledFirstEvent { get; set; }
    
    public static readonly ActDefinition Overgrowth = new()
    {
        NativeAncients =
        [
            Ancient.Neow
        ],

        WeakEncounterCount = 3,
        RoomCount = 15,

        ActEvents =
        [
            EventOptions.AromaOfChaos,
            EventOptions.ByrdonisNest,
            EventOptions.DenseVegetation,
            EventOptions.JungleMazeAdventure,
            EventOptions.LuminousChoir,
            EventOptions.MorphicGrove,
            EventOptions.SapphireSeed,
            EventOptions.SunkenStatue,
            EventOptions.TabletOfTruth,
            EventOptions.UnrestSite,
            EventOptions.Wellspring,
            EventOptions.WhisperingHollow,
            EventOptions.WoodCarvings,
        ],

        WeakEncounters =
        [
            new SimpleEncounterDef(EncounterTag.Crawler),                       // FuzzyWurmCrawlerWeak
            new SimpleEncounterDef(EncounterTag.Nibbit),                        // NibbitsWeak
            new SimpleEncounterDef(EncounterTag.Shrinker),                      // ShrinkerBeetleWeak
            new SimpleEncounterDef(EncounterTag.Slimes),                        // SlimesWeak
        ],

        RegularEncounters =
        [
            new SimpleEncounterDef(),                                           // CubexConstructNormal
            new SimpleEncounterDef(EncounterTag.Mushroom, EncounterTag.Slimes), // FlyconidNormal
            new SimpleEncounterDef(),                                           // FogmogNormal
            new SimpleEncounterDef(),                                           // InkletsNormal
            new SimpleEncounterDef(),                                           // MawlerNormal
            new SimpleEncounterDef(),                                           // NibbitsNormal
            new SimpleEncounterDef(EncounterTag.Shrinker, EncounterTag.Crawler),// OvergrowthCrawlers
            new SimpleEncounterDef(),                                           // RubyRaidersNormal
            new SimpleEncounterDef(EncounterTag.Slimes),                        // SlimesNormal
            new SimpleEncounterDef(EncounterTag.Jaxfruit, EncounterTag.Slimes), // SlitheringStranglerNormal
            new SimpleEncounterDef(EncounterTag.Mushroom, EncounterTag.Jaxfruit),// SnappingJaxfruitNormal
            new SimpleEncounterDef(),                                           // VineShamblerNormal
        ],

        EliteEncounters =
        [
            new SimpleEncounterDef(), // BygoneEffigyElite
            new SimpleEncounterDef(), // ByrdonisElite
            new SimpleEncounterDef(), // PhrogParasiteElite
        ],
        
        Bosses = [BossOptions.CeremonialBeast, BossOptions.TheKin, BossOptions.Vantom]
    };
    
     public static readonly ActDefinition Underdocks = new()
    {
        NativeAncients =
        [
            Ancient.Neow
        ],

        WeakEncounterCount = 3,
        RoomCount = 15,

        ActEvents =
        [
            EventOptions.AbyssalBaths,
            EventOptions.DrowningBeacon,
            EventOptions.EndlessConveyor,
            EventOptions.PunchOff,
            EventOptions.SpiralingWhirlpool,
            EventOptions.SunkenStatue,
            EventOptions.SunkenTreasury,
            EventOptions.DoorsOfLightAndDark,
            EventOptions.TrashHeap,
            EventOptions.WaterloggedScriptorium,
        ],

        WeakEncounters =
        [
            new SimpleEncounterDef(EncounterTag.Slugs),     // CorpseSlugsWeak
            new SimpleEncounterDef(EncounterTag.Seapunk),   // SeapunkWeak
            new SimpleEncounterDef(),                        // SludgeSpinnerWeak
            new SimpleEncounterDef(),                        // ToadpolesWeak
        ],

        RegularEncounters =
        [
            new SimpleEncounterDef(EncounterTag.Slugs),     // CorpseSlugsNormal
            new SimpleEncounterDef(),                        // CultistsNormal
            new SimpleEncounterDef(),                        // FossilStalkerNormal
            new SimpleEncounterDef(),                        // GremlinMercNormal
            new SimpleEncounterDef(),                        // HauntedShipNormal
            new SimpleEncounterDef(),                        // LivingFogNormal
            new SimpleEncounterDef(),                        // PunchConstructNormal
            new SimpleEncounterDef(EncounterTag.Seapunk),   // SeapunkNormal
            new SimpleEncounterDef(),                        // SewerClamNormal
            new SimpleEncounterDef(),                        // TwoTailedRatsNormal
        ],

        EliteEncounters =
        [
            new SimpleEncounterDef(), // PhantasmalGardenersElite
            new SimpleEncounterDef(), // SkulkingColonyElite
            new SimpleEncounterDef(), // TerrorEelElite
        ],
        
        Bosses = [BossOptions.LagavulinMatriarch, BossOptions.SoulFysh, BossOptions.WaterfallGiant]
    };

    public static readonly ActDefinition Hive = new()
    {
        NativeAncients =
        [
            Ancient.Orobas,
            Ancient.Pael,
            Ancient.Tezcatara
        ],

        WeakEncounterCount = 2,
        RoomCount = 14,

        ActEvents =
        [
            EventOptions.Amalgamator,
            EventOptions.Bugslayer,
            EventOptions.ColorfulPhilosophers,
            EventOptions.ColossalFlower,
            EventOptions.FieldOfManSizedHoles,
            EventOptions.InfestedAutomaton,
            EventOptions.LostWisp,
            EventOptions.SpiritGrafter,
            EventOptions.TheLanternKey,
            EventOptions.ZenWeaver,
        ],

        WeakEncounters =
        [
            new SimpleEncounterDef(EncounterTag.Workers),    // BowlbugsWeak
            new SimpleEncounterDef(EncounterTag.Exoskeletons), // ExoskeletonsWeak
            new SimpleEncounterDef(EncounterTag.Thieves),    // ThievingHopperWeak
            new SimpleEncounterDef(EncounterTag.Burrower),   // TunnelerWeak
        ],

        RegularEncounters =
        [
            new SimpleEncounterDef(EncounterTag.Workers),      // BowlbugsNormal
            new SimpleEncounterDef(EncounterTag.Chomper),      // ChompersNormal
            new SimpleEncounterDef(EncounterTag.Exoskeletons), // ExoskeletonsNormal
            new SimpleEncounterDef(),                          // HunterKillerNormal
            new SimpleEncounterDef(),                          // LouseProgenitorNormal
            new SimpleEncounterDef(),                          // MytesNormal
            new SimpleEncounterDef(),                          // OvicopterNormal
            new SimpleEncounterDef(EncounterTag.Workers),      // SlumberingBeetleNormal
            new SimpleEncounterDef(),                          // SpinyToadNormal
            new SimpleEncounterDef(),                          // TheObscuraNormal
        ],

        EliteEncounters =
        [
            new SimpleEncounterDef(), // DecimillipedeElite
            new SimpleEncounterDef(), // EntomancerElite
            new SimpleEncounterDef(), // InfestedPrismsElite
        ],
        
        Bosses = [BossOptions.KaiserCrab, BossOptions.KnowledgeDemon, BossOptions.TheInsatiable]
    };

    public static readonly ActDefinition Glory = new()
    {
        NativeAncients =
        [
            Ancient.Nonupeipe,
            Ancient.Tanx,
            Ancient.Vakuu
        ],

        WeakEncounterCount = 2,
        RoomCount = 13,

        ActEvents =
        [
            EventOptions.BattlewornDummy,
            EventOptions.GraveOfTheForgotten,
            EventOptions.HungryForMushrooms,
            EventOptions.Reflections,
            EventOptions.RoundTeaParty,
            EventOptions.Trial,
            EventOptions.TinkerTime,
        ],

        WeakEncounters =
        [
            new SimpleEncounterDef(),                        // DevotedSculptorWeak
            new SimpleEncounterDef(EncounterTag.Scrolls),    // ScrollsOfBitingWeak
            new SimpleEncounterDef(),                        // TurretOperatorWeak
        ],

        RegularEncounters =
        [
            new SimpleEncounterDef(),                        // AxebotsNormal
            new SimpleEncounterDef(),                        // ConstructMenagerieNormal
            new SimpleEncounterDef(),                        // FabricatorNormal
            new SimpleEncounterDef(),                        // FrogKnightNormal
            new SimpleEncounterDef(),                        // GlobeHeadNormal
            new SimpleEncounterDef(),                        // OwlMagistrateNormal
            new SimpleEncounterDef(EncounterTag.Scrolls),    // ScrollsOfBitingNormal
            new SimpleEncounterDef(),                        // SlimedBerserkerNormal
            new SimpleEncounterDef(),                        // TheLostAndForgottenNormal
        ],

        EliteEncounters =
        [
            new SimpleEncounterDef(EncounterTag.Knights), // KnightsElite
            new SimpleEncounterDef(),                     // MechaKnightElite
            new SimpleEncounterDef(),                     // SoulNexusElite
        ],
        
        Bosses = [BossOptions.Aeonglass, BossOptions.Queen, BossOptions.TestSubject]
    };
    
    public ActDefinition Clone()
    {
        return new ActDefinition
        {
            NativeAncients = NativeAncients,
            WeakEncounters = WeakEncounters,
            RegularEncounters = RegularEncounters,
            EliteEncounters = EliteEncounters,
            Bosses = Bosses,
            ActEvents = ActEvents,
            WeakEncounterCount = WeakEncounterCount,
            RoomCount = RoomCount
        };
    }
}