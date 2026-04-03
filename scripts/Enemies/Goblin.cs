using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;
public partial class Goblin : CharacterBody2D
{
[Export] public float Speed = 120.0f;
[Export] public float StopDistance = 50.0f;
private Node2D _player;
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Node2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return; 
		float distanceToPlayer = GlobalPosition.DistanceTo(_player.GlobalPosition);
		if (distanceToPlayer > StopDistance)
		{
		
		Vector2 direction = GlobalPosition.DirectionTo(_player.GlobalPosition);
		Velocity = direction * Speed;
		}

		else
		
		{
			Velocity = Vector2.Zero;
		}
		MoveAndSlide();
	}
}
