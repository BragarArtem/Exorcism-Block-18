using Godot;
using System;

public partial class Goblin : CharacterBody2D 
{
private AnimatedSprite2D _animatedSprite; 

[Export] public float Speed = 120.0f;
[Export] public float StopDistance = 50.0f;
private Node2D _player;
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		_animatedSprite = GetNode<AnimatedSprite2D>("Goblin");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return; 
		float distanceToPlayer = GlobalPosition.DistanceTo(_player.GlobalPosition);
		if (distanceToPlayer > StopDistance)
		{
		
		Vector2 direction = GlobalPosition.DirectionTo(_player.GlobalPosition);
		Velocity = direction * Speed;
		
		_animatedSprite.Play("walk");
		_animatedSprite.FlipH = direction.X < 0; 
		}

		else
		
		{
			Velocity = Vector2.Zero;
		}
		MoveAndSlide();
	}
}
