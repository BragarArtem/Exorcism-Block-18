using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 300.0f;

	private AnimatedSprite2D _hero;
	private string _currentDir = "down"; 

	public override void _Ready()
	{
		_hero = GetNode<AnimatedSprite2D>("Hero");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		
		Velocity = inputDir * Speed;
		MoveAndSlide();

		UpdateDirection(inputDir);
		UpdateAnimations(inputDir);
	}

	private void UpdateDirection(Vector2 inputDir)
{
	if (inputDir.Y > 0) _currentDir = "down";
	else if (inputDir.Y < 0) _currentDir = "up";
	else if (inputDir.X != 0) _currentDir = "right"; 
	
	if (inputDir.X < 0) 
		_hero.FlipH = true;
	else if (inputDir.X > 0) 
		_hero.FlipH = false;
}

	private void UpdateAnimations(Vector2 inputDir)
	{
		string action = (inputDir.Length() > 0) ? "Run" : "Idle";

		string animName = $"{action}_{_currentDir}";
		if (Input.IsActionJustPressed("attack"))
		{
			_hero.Play($"Attack_{_currentDir}");
			return;
		}

		if ((_hero.Animation.ToString().StartsWith("Attack") || _hero.Animation == "Dying") && _hero.IsPlaying())
		{
			return;
		}

		_hero.Play(animName);
	}
}
