using Godot;

public enum UIPanel {None, Settings, Encyclopedia, Difficulty}

public partial class MainMenuController : Control
{
    private TextureRect _settingsPanel;
    private Control _encyclopediaPanel;
    private TextureRect _difficultyPanel;
    public override void _Ready()
    {
        SetupButton(GetNode<Button>("MainMenuButtons/PlayButton"), OnPlayPressed);
        SetupButton(GetNode<Button>("MainMenuButtons/DifficultyButton"), OnDifficultyPressed);
        SetupButton(GetNode<Button>("MainMenuButtons/InventoryButton"), OnInventoryPressed);
        SetupButton(GetNode<Button>("MainMenuButtons/EncyclopediaButton"), OnEncyclopediaButton);
        SetupButton(GetNode<Button>("MainMenuButtons/SettingsButton"), OnSettingsPressed);
        SetupButton(GetNode<Button>("MainMenuButtons/ExitButton"), OnExitPressed);
        GetNode<CheckButton>("SettingsPanel/SettingsVBox/FullscreenCheckButton").Toggled += OnFullscreenTogled;
        _settingsPanel = GetNode<TextureRect>("SettingsPanel"); 
        _encyclopediaPanel = GetNode<Control>("EncyclopediaPanel");
        _difficultyPanel = GetNode<TextureRect>("DifficultyPanel");
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
        if (_difficultyPanel.Visible)
        {
            OpenPanel(UIPanel.None);
        }
        else
        {
            OpenPanel(UIPanel.Difficulty);
        }
    }
    private void OnInventoryPressed()
    {
        GD.Print("would be added soon");
    }
    private void OnEncyclopediaButton()
    {
        if (_encyclopediaPanel.Visible)
        {
            OpenPanel(UIPanel.None);
        }
        else
        {
            OpenPanel(UIPanel.Encyclopedia);
        }
    }
    private void OnSettingsPressed()
    {
        if (_settingsPanel.Visible)
        {
            OpenPanel(UIPanel.None);
        }
        else
        {
            OpenPanel(UIPanel.Settings);
        }
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
    private void OpenPanel(UIPanel panel)
    {
        _settingsPanel.Visible = false;
        _encyclopediaPanel.Visible = false;
        _difficultyPanel.Visible = false;
        switch (panel)
        {
            case UIPanel.Settings:
               _settingsPanel.Visible = true;
                break; 

            case UIPanel.Encyclopedia:
                _encyclopediaPanel.Visible = true;
                break;
            case UIPanel.Difficulty:
                _difficultyPanel.Visible = true;
                break;
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            OpenPanel(UIPanel.None);
        }
    }
}
