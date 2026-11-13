using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.Code.Patches;

[HarmonyPatch(typeof(NGame), "StartNewSingleplayerRun")]
internal class StartNewSingleplayerRunPatch
{
    public static bool IsFilteredSeedRun { get; set; }

    [HarmonyPrefix]
    private static void Prefix(ref bool isCustomGame)
    {
        if (IsFilteredSeedRun)
        {
            isCustomGame = true;
            IsFilteredSeedRun = false;
        }
    }
}