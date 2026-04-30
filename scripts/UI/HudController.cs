using Godot;
using System;

public partial class HudController : CanvasLayer
{
    [Export] public ProgressBar HpBar;
    [Export] public ProgressBar ExpBar;
    public override void _Ready()
    {
        Layer = 10;
        HpBar.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
        HpBar.Position = new Vector2(10,10);
        HpBar.Size = new Vector2(200,20);
        ExpBar.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
        ExpBar.Position = new Vector2(0,620);
        ExpBar.Size = new Vector2(1152, 20);
    }
    public void UpdateHp(float current, float max)
    {
        HpBar.MaxValue = max;
        HpBar.Value = current;
    }
    public void UpdateExp(float current, float max)
    {
        ExpBar.MaxValue = max;
        ExpBar.Value = current;
    }
}
