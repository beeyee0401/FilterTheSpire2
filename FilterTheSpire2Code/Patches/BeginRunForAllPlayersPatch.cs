using FilterTheSpire2.FilterTheSpire2Code.Patches;
using FilterTheSpire2.FilterTheSpire2Code.SeedSearcher;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;

[HarmonyPatch(typeof(StartRunLobby), "BeginRunForAllPlayers")]
internal class BeginRunForAllPlayersPatch
{
    private static bool _searching = false;

    [HarmonyPrefix]
    private static bool Prefix(StartRunLobby __instance, ref string seed, List<ModifierModel> modifiers)
    {
        if (_searching || 
            __instance.GameMode != GameMode.Standard || 
            __instance.Players.Count > 1 || 
            __instance.Players[0].unlockState.UnlockedEpochs.Count != UnlockState.all.EpochUnlockCount())
        {
            return true;
        }

        var capturedSeed = seed;
        var capturedModifiers = modifiers;

        TaskHelper.RunSafely(SearchAndBegin(__instance, capturedSeed, capturedModifiers));
        return false;
    }

    private static async Task SearchAndBegin(
        StartRunLobby instance,
        string seed,
        List<ModifierModel> modifiers)
    {
        using var cts = new CancellationTokenSource();
        var searcher = new SeedSearcher();

        var screen = instance.LobbyListener as NCharacterSelectScreen;
        CanvasLayer? overlay = null;
        RichTextLabel? statusLabel = null;

        var leftArrowWasVisible = false;
        var rightArrowWasVisible = false;

        if (screen != null)
        {
            var ap = Traverse.Create(Traverse.Create(screen).Field("_ascensionPanel").GetValue<NAscensionPanel>());
            var leftArrow = ap.Field("_leftArrow").GetValue<NButton>();
            var rightArrow = ap.Field("_rightArrow").GetValue<NButton>();

            leftArrowWasVisible = leftArrow.Visible;
            rightArrowWasVisible = rightArrow.Visible;

            leftArrow.Visible = false;
            rightArrow.Visible = false;
            
            (overlay, statusLabel) = BuildOverlay(searcher, screen, cts, leftArrowWasVisible, rightArrowWasVisible);
            screen.AddChild(overlay);
        }

        var searchTask = Task.Run(() =>
            searcher.SearchForSeed(instance.Players[0].character, instance.Ascension)?.StringSeed, cts.Token);

        string? foundSeed;
        try
        {
            while (!searchTask.IsCompleted && !cts.IsCancellationRequested)
            {
                var count = searcher.Runner?.TotalSeedsExamined;
                if (statusLabel != null)
                {
                    Callable.From(() =>
                        statusLabel.Text = $"Searching for seed...\n{count:N0} examined"
                    ).CallDeferred();
                }

                await Task.Delay(100, cts.Token);
            }

            foundSeed = await searchTask;
            
            if (!cts.IsCancellationRequested && foundSeed != null && statusLabel != null)
            {
                var finalCount = searcher.Runner?.TotalSeedsExamined;
                Callable.From(() =>
                    statusLabel.Text = $"Seed found!\nExamined [color=yellow]{finalCount:N0}[/color] seeds"
                ).CallDeferred();

                await Task.Delay(1500, cts.Token);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            if (statusLabel != null)
            {
                Callable.From(() =>
                    statusLabel.Text = "An error occurred while searching.\nPlease try again."
                ).CallDeferred();
        
                await Task.Delay(2500, cts.Token);
            }
    
            overlay?.QueueFree();
            instance.SetReady(false);
            RestoreScreenUi(screen, leftArrowWasVisible, rightArrowWasVisible);
            return;
        }
        finally
        {
            overlay?.QueueFree();
        }

        if (cts.IsCancellationRequested)
        {
            instance.SetReady(false);
            RestoreScreenUi(screen, leftArrowWasVisible, rightArrowWasVisible);
            return;
        }

        _searching = true;
        try
        {
            // Attempt to label it a Custom run since it's essentially seeded.
            StartNewSingleplayerRunPatch.IsFilteredSeedRun = true;
            AccessTools.Method(typeof(StartRunLobby), "BeginRunForAllPlayers")
                .Invoke(instance, [foundSeed ?? seed, modifiers]);
        }
        finally
        {
            _searching = false;
        }
    }

    private static (CanvasLayer overlay, RichTextLabel  statusLabel) BuildOverlay(
        SeedSearcher searcher,
        NCharacterSelectScreen? screen,
        CancellationTokenSource cts,
        bool leftArrowWasVisible,
        bool rightArrowWasVisible)
    {
        var overlay = new CanvasLayer();

        var panel = new Panel();
        panel.SetAnchorsPreset(Control.LayoutPreset.Center);
        panel.CustomMinimumSize = new Vector2(300, 120);

        var vbox = new VBoxContainer();
        vbox.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        vbox.AddThemeConstantOverride("separation", 16);

        var label = new RichTextLabel
        {
            BbcodeEnabled = true,
            Text = "Searching for seed...",
            FitContent = true,
            AutowrapMode = TextServer.AutowrapMode.Off,
        };

        var cancelButton = new Button { Text = "Cancel" };
        cancelButton.Pressed += () =>
        {
            cts.Cancel();
            label.Text = "Cancelling...";
            cancelButton.Disabled = true;
            searcher.Cancel();
            RestoreScreenUi(screen, leftArrowWasVisible, rightArrowWasVisible);
        };

        vbox.AddChild(label);
        vbox.AddChild(cancelButton);
        panel.AddChild(vbox);
        overlay.AddChild(panel);

        return (overlay, label);
    }

    private static void RestoreScreenUi(NCharacterSelectScreen? screen, bool leftArrowWasVisible, bool rightArrowWasVisible)
    {
        if (screen == null)
        {
            return;
        }

        var t = Traverse.Create(screen);
        t.Field("_embarkButton").GetValue<NConfirmButton>().Enable();
        t.Field("_backButton").GetValue<NBackButton>().Enable();

        var container = t.Field("_charButtonContainer").GetValue<Control>();
        foreach (var btn in container.GetChildren().OfType<NCharacterSelectButton>())
        {
            btn.Enable();
        }

        var ascensionPanel = t.Field("_ascensionPanel").GetValue<NAscensionPanel>();
        var ap = Traverse.Create(ascensionPanel);
        ap.Field("_leftArrow").GetValue<NButton>().Visible = leftArrowWasVisible;
        ap.Field("_rightArrow").GetValue<NButton>().Visible = rightArrowWasVisible;
    }
}