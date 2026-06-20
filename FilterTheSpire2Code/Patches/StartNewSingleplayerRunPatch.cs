using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;

namespace FilterTheSpire2.FilterTheSpire2Code.Patches;

[HarmonyPatch(typeof(NGame), "StartNewSingleplayerRun")]
internal class StartNewSingleplayerRunPatch
{
    public static bool IsFilteredSeedRun { get; set; }

    [HarmonyPrefix]
    private static void Prefix(ref GameMode gameMode)
    {
        if (IsFilteredSeedRun)
        {
            gameMode = GameMode.Custom;
            IsFilteredSeedRun = false;
        }
    }
}