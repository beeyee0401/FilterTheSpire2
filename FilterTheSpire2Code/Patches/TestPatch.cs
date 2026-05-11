using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace FilterTheSpire2.FilterTheSpire2Code.Patches;

// [HarmonyPatch(typeof (RunManager), "GenerateRooms")]
// internal class TestPatch
// {
//     [HarmonyPrefix]
//     private static void ForceAncientToSpawnBefore(RunManager __instance)
//     {
//         var propInfo = AccessTools.Property(typeof(RunManager), "State");
//         var state = (RunState?) propInfo.GetValue(__instance)!;
//         Rng rng = new Rng((uint) StringHelper.GetDeterministicHashCode(state.Rng.StringSeed));
//         List<ActModel> list = ActModel.GetRandomList(rng, state.Players[0].UnlockState, false).ToList();
//         var mutable = list.Select((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList();
//         var upfrontRng = new Rng(state.Rng.UpFront.Seed, 231);
//         var ancientList = new List<Ancient>()
//         {
//             Ancient.Orobas, 
//             Ancient.Pael, 
//             Ancient.Tezcatara
//         };
//         
//         Console.Write("test");
//     }
//     
//     [HarmonyPostfix]
//     private static void Postfix(RunManager __instance)
//     {
//         var propInfo = AccessTools.Property(typeof(RunManager), "State");
//         var state = (RunState?) propInfo.GetValue(__instance)!;
//         Console.Write("test");
//     }
// }
//
// [HarmonyPatch(typeof (ActModel), "GenerateRooms")]
// internal class TestPatch2
// {
//     [HarmonyPrefix]
//     private static void PrefixCheck(ActModel __instance, Rng rng)
//     {
//         Console.Write("test");
//     }
//     
//     [HarmonyPostfix]
//     private static void Postfix(ActModel __instance, Rng rng)
//     {
//         Console.Write("test");
//     }
// }
