using FilterTheSpire2.FilterTheSpire2Code.Filters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Patches;

[HarmonyPatch(typeof(StartRunLobby), "BeginRunForAllPlayers")]
internal class BeginRunForAllPlayersPatch
{
    [HarmonyPrefix]
    private static void StartSearcher(
        StartRunLobby __instance,
        ref string seed)
    {
        if (__instance.GameMode == GameMode.Standard)
        {
            Console.WriteLine("Starting seed: " + seed);
        
            var result = SeedSearcher.SeedSearcher.SearchForSeed(__instance.Players[0].character);

            if (result == null)
            {
                return;
            }

            Console.WriteLine("Found seed: " + result.StringSeed);
            seed = result.StringSeed;
        }
    }
}