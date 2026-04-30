using Godot;
using System;

public partial class TransitionManager : Node
{
    private static TransitionManager _instance;
    private ColorRect _fogRect;
    private ShaderMaterial _shader;

    public override void _Ready()
    {
        _instance = this;
        
        var transitionScene = GD.Load<PackedScene>("res://scences/Transition.tscn");
        var transition = transitionScene.Instantiate();
        AddChild(transition);
        
        _fogRect = transition.GetNode<ColorRect>("CanvasLayer/ColorRect");
        _shader = _fogRect.Material as ShaderMaterial;
        _shader.SetShaderParameter("progress", 0.0f);
        _fogRect.Visible = false;
    }

    public static async void GoTo(string scenePath)
    {
        _instance._fogRect.Visible = true;
        await _instance.AnimateIn();
        _instance.GetTree().ChangeSceneToFile(scenePath);
        _instance._fogRect.Visible = false;
    }

    private async System.Threading.Tasks.Task AnimateIn()
    {
        _shader.SetShaderParameter("is_intro", true);
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            float speed = 0.001f + (progress * progress * 0.05f);
            progress += speed;
            _shader.SetShaderParameter("progress", progress);
            await ToSignal(GetTree(), "process_frame");
        }
    }
}