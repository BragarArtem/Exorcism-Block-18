using Godot;
using System;

public partial class DeathScreen : Control
{
    public override async void _Ready()
    {
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        var random = new Random();
        int roll = random.Next(1, 3);
        if(roll == 2)
        {   
            var afLabel = GetNode<Label>("afLabel");
            afLabel.Visible = !afLabel.Visible;
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            GetTree().ChangeSceneToFile("res://scences/MainMenu.tscn");
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scences/MainMenu.tscn");
        }
    }
}
