using BaseLib.Config;
using FilterTheSpire2.FilterTheSpire2Code.Filters;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace FilterTheSpire2.FilterTheSpire2Code;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "FilterTheSpire2"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        ModConfigRegistry.Register(ModId, new FilterTheSpire2Config());
        harmony.PatchAll();
    }
}