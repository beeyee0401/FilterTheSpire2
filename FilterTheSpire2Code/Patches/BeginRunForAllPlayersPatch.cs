using FilterTheSpire2.FilterTheSpire2Code.Filters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

namespace FilterTheSpire2.FilterTheSpire2Code.Patches;

[HarmonyPatch(typeof(StartRunLobby), "BeginRunForAllPlayers")]
internal class BeginRunForAllPlayersPatch
{ 
    [HarmonyPrefix]
    private static void StartSearcher(ref string seed, List<ModifierModel> modifiers)
    {
        FilterManager.InitializeFiltersFromSettings();
        Console.WriteLine("start seed: "  + seed);
        if (FilterManager.HasFilters())
        {
            SeedSearcher.SeedSearcher.SearchForSeed();
            while (!SeedSearcher.SeedSearcher.IsFinished())
            {
            
            }
            seed = SeedSearcher.SeedSearcher.FoundSeed!;
        }
        Console.WriteLine("found seed: "  + seed);
    }
}