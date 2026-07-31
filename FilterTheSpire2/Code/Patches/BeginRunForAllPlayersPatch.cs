using FilterTheSpire2.Code.Filters;
using FilterTheSpire2.Code.SeedSearcher;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace FilterTheSpire2.Code.Patches;

[HarmonyPatch(typeof(StartRunLobby), "BeginRunForAllPlayers")]
internal class BeginRunForAllPlayersPatch
{
    private static bool _searching = false;

    private sealed class SearchUiState
    {
        public bool LeftArrowWasVisible { get; init; }
        public bool RightArrowWasVisible { get; init; }
        public bool LeftTabIconWasVisible { get; init; }
        public bool RightTabIconWasVisible { get; init; }

        public Action? CancelAction { get; set; }
    }

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

        var screen = instance.LobbyListener as NCharacterSelectScreen;
        CanvasLayer? overlay = null;
        RichTextLabel? statusLabel = null;
        SearchUiState? uiState = null;

        var filters = FilterManager.CreateFiltersFromSettings();
        if (filters.Count == 0)
        {
            BeginRunWithSeed(instance, seed, modifiers, filteredSeedRun: false);
            return;
        }

        var request = new SeedSearchRequest
        {
            AscensionLevel = (AscensionLevel)instance.Ascension,
            Filters = filters,
            ThreadCount = 6
        };

        var runner = new SeedSearchRunner(request);

        var searchTask = Task.Run(() =>
        {
            runner.Run();
            return runner.Result?.StringSeed;
        }, cts.Token);

        string? foundSeed = null;

        if (screen != null)
        {
            uiState = HideSearchUi(screen);
            (overlay, statusLabel) = BuildOverlay(runner, cts, uiState);
            screen.AddChild(overlay);
        }

        try
        {
            while (!searchTask.IsCompleted && !cts.IsCancellationRequested)
            {
                var count = runner.TotalSeedsExamined;

                if (statusLabel != null)
                {
                    Callable.From(() =>
                        statusLabel.Text = $"Searching for seed...\n{count:N0} examined"
                    ).CallDeferred();
                }

                await Task.Delay(100, cts.Token);
            }

            foundSeed = await searchTask;

            if (!cts.IsCancellationRequested && statusLabel != null)
            {
                var finalCount = runner.TotalSeedsExamined;

                if (foundSeed != null)
                {
                    Callable.From(() =>
                        statusLabel.Text = $"Seed found!\nExamined [color=yellow]{finalCount:N0}[/color] seeds"
                    ).CallDeferred();

                    await Task.Delay(1500, cts.Token);
                }
                else
                {
                    Callable.From(() =>
                        statusLabel.Text = $"No matching seed found.\nSearched [color=yellow]{finalCount:N0}[/color] seeds"
                    ).CallDeferred();

                    await Task.Delay(2500, cts.Token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            runner.Cancel();
        }
        catch (Exception)
        {
            if (statusLabel != null)
            {
                Callable.From(() =>
                    statusLabel.Text = "An error occurred while searching.\nPlease try again."
                ).CallDeferred();

                await Task.Delay(2500, cts.Token);
            }

            instance.SetReady(false);
            return;
        }
        finally
        {
            overlay?.QueueFree();
            RestoreScreenUi(screen, uiState);
        }

        if (cts.IsCancellationRequested || foundSeed == null)
        {
            instance.SetReady(false);
            return;
        }

        BeginRunWithSeed(instance, foundSeed, modifiers, filteredSeedRun: true);
    }

    private static SearchUiState HideSearchUi(NCharacterSelectScreen screen)
    {
        var screenTraverse = Traverse.Create(screen);

        screenTraverse.Field("_embarkButton").GetValue<NConfirmButton>().Disable();
        screenTraverse.Field("_backButton").GetValue<NBackButton>().Disable();

        var characterButtonContainer = screenTraverse.Field("_charButtonContainer").GetValue<Control>();
        foreach (var button in characterButtonContainer.GetChildren().OfType<NCharacterSelectButton>())
        {
            button.Disable();
        }

        var ascensionPanel = screenTraverse.Field("_ascensionPanel").GetValue<NAscensionPanel>();
        var ascensionTraverse = Traverse.Create(ascensionPanel);

        var leftArrow = ascensionTraverse.Field("_leftArrow").GetValue<NButton>();
        var rightArrow = ascensionTraverse.Field("_rightArrow").GetValue<NButton>();
        var leftTriggerIcon = ascensionTraverse.Field("_leftTabIcon").GetValue<NHotkeyIcon>();
        var rightTriggerIcon = ascensionTraverse.Field("_rightTabIcon").GetValue<NHotkeyIcon>();

        var state = new SearchUiState
        {
            LeftArrowWasVisible = leftArrow.Visible,
            RightArrowWasVisible = rightArrow.Visible,
            LeftTabIconWasVisible = leftTriggerIcon.Visible,
            RightTabIconWasVisible = rightTriggerIcon.Visible
        };

        leftArrow.Visible = false;
        rightArrow.Visible = false;
        leftTriggerIcon.Visible = false;
        rightTriggerIcon.Visible = false;

        return state;
    }

    private static (CanvasLayer overlay, RichTextLabel statusLabel) BuildOverlay(
        SeedSearchRunner searcher,
        CancellationTokenSource cts,
        SearchUiState uiState)
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

        void CancelSearch()
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }

            cts.Cancel();
            searcher.Cancel();

            label.Text = "Cancelling...";
            cancelButton.Disabled = true;
        }

        uiState.CancelAction = CancelSearch;

        cancelButton.Pressed += CancelSearch;

        NHotkeyManager.Instance!.PushHotkeyPressedBinding(
            MegaInput.cancel,
            uiState.CancelAction);

        vbox.AddChild(label);
        vbox.AddChild(cancelButton);
        panel.AddChild(vbox);
        overlay.AddChild(panel);

        cancelButton.CallDeferred(Control.MethodName.GrabFocus);

        return (overlay, label);
    }

    private static void RestoreScreenUi(NCharacterSelectScreen? screen, SearchUiState? state)
    {
        if (screen == null || state == null)
        {
            return;
        }

        if (state.CancelAction != null)
        {
            NHotkeyManager.Instance!.RemoveHotkeyPressedBinding(
                MegaInput.cancel,
                state.CancelAction);
        }

        var screenTraverse = Traverse.Create(screen);

        screenTraverse.Field("_embarkButton").GetValue<NConfirmButton>().Enable();
        screenTraverse.Field("_backButton").GetValue<NBackButton>().Enable();

        var characterButtonContainer = screenTraverse.Field("_charButtonContainer").GetValue<Control>();
        foreach (var button in characterButtonContainer.GetChildren().OfType<NCharacterSelectButton>())
        {
            button.Enable();
        }

        var ascensionPanel = screenTraverse.Field("_ascensionPanel").GetValue<NAscensionPanel>();
        var ascensionTraverse = Traverse.Create(ascensionPanel);

        ascensionTraverse.Field("_leftArrow").GetValue<NButton>().Visible = state.LeftArrowWasVisible;
        ascensionTraverse.Field("_rightArrow").GetValue<NButton>().Visible = state.RightArrowWasVisible;
        ascensionTraverse.Field("_leftTabIcon").GetValue<NHotkeyIcon>().Visible = state.LeftTabIconWasVisible;
        ascensionTraverse.Field("_rightTabIcon").GetValue<NHotkeyIcon>().Visible = state.RightTabIconWasVisible;
        
        RestoreCharacterFocus(screen);
    }
    
    private static void RestoreCharacterFocus(NCharacterSelectScreen screen)
    {
        var screenTraverse = Traverse.Create(screen);
        var selectedButton = screenTraverse
            .Field("_selectedButton")
            .GetValue<NCharacterSelectButton?>();

        if (selectedButton != null && selectedButton.Visible)
        {
            selectedButton.CallDeferred(Control.MethodName.GrabFocus);
            return;
        }

        var characterButtonContainer = screenTraverse.Field("_charButtonContainer").GetValue<Control>();

        var firstVisibleButton = characterButtonContainer
            .GetChildren()
            .OfType<NCharacterSelectButton>()
            .FirstOrDefault(button => button.Visible);

        firstVisibleButton?.CallDeferred(Control.MethodName.GrabFocus);
    }

    private static void BeginRunWithSeed(
        StartRunLobby instance,
        string seed,
        List<ModifierModel> modifiers,
        bool filteredSeedRun)
    {
        _searching = true;

        try
        {
            if (filteredSeedRun)
            {
                StartNewSingleplayerRunPatch.IsFilteredSeedRun = true;
            }

            AccessTools.Method(typeof(StartRunLobby), "BeginRunForAllPlayers")
                .Invoke(instance, [seed, modifiers]);
        }
        finally
        {
            _searching = false;
        }
    }
}