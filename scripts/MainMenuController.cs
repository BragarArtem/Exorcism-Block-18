using Godot;
using System;

public partial class MainMenuController : Control
{
    public override void _Ready()
    {
        SetupButton(GetNode<Button>("CenterContainer/PlayButton"), OnPlayPressed);
        SetupButton(GetNode<Button>("CenterContainer/DifficultyButton"), OnDifficultyPressed);
        SetupButton(GetNode<Button>("InventoryButton"), OnInventoryPressed);
        SetupButton(GetNode<Button>("EncyclopediaButton"), OnEncyclopediaButton);
        SetupButton(GetNode<Button>("SettingsButton"), OnSettingsPressed);
        SetupButton(GetNode<Button>("SettingsButton/SettingsPanel/SettingsContainer/ExitButton"), OnExitPressed);
        GetNode<CheckButton>("SettingsButton/SettingsPanel/SettingsContainer/FullscreenCheckButton").Toggled += OnFullscreenTogled;
    }
    private void SetupButton(Button button, System.Action action)
    {
        button.AddThemeStyleboxOverride("hover", new StyleBoxEmpty());
        button.AddThemeStyleboxOverride("normal", new StyleBoxEmpty());
        button.AddThemeStyleboxOverride("pressed", new StyleBoxEmpty());
        button.AddThemeStyleboxOverride("focus", new StyleBoxEmpty());

        button.AddThemeColorOverride("font_color", new Color(1, 1, 1, 1));
        button.AddThemeColorOverride("font_pressed_color", new Color(0.5f, 0.5f, 0.5f, 0.5f));
        button.AddThemeColorOverride("font_hover_color", new Color(1, 0.8f, 0.3f, 1));

        button.AddThemeFontSizeOverride("font_size", 24);
        button.Pressed += () => action();
    }
    private void OnPlayPressed()
    {
        TransitionManager.GoTo("res://scences/main.tscn");
    }
    private void OnDifficultyPressed()
    {
        GD.Print("would be added soon");
    }
    private void OnInventoryPressed()
    {
        GD.Print("would be added soon");
    }
    private void OnEncyclopediaButton()
    {
        GD.Print("would be added soon");
    }
    private void OnSettingsPressed()
    {
        var SettingsPanel = GetNode<Panel>("SettingsButton/SettingsPanel");
        SettingsPanel.Visible = !SettingsPanel.Visible;
    }
    private void OnExitPressed()
    {
        GetTree().Quit();
    }
    private void OnFullscreenTogled(bool toggled)
    {
        if (toggled)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
        }
    }
}
