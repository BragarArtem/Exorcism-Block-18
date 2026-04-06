using Godot;
using System;

public partial class Attack : Node2D
{
[Export] public float AttackRange = 150.0f;
[Export] public float APS = 2.0f; // APS - Attacks per secomnd 
[Export] public int Damage = 5;

public float _attackTimer = 0.0f;
public Node2D _currentTarget;
private AnimatedSprite2D _sprite;

public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

public override void _PhysicsProcess(double delta)
	{
		_attackTimer += (float) delta;
		_currentTarget = GetClosestEnemy();
	
	if (_currentTarget != null && IsInstanceValid (_currentTarget))
		{
			float distance = GlobalPosition.DistanceTo(_currentTarget.GlobalPosition);
			if (distance <= AttackRange && _attackTimer >= 1.0f /APS )
			{
				if (CanAttack())
				PerformAttack();
				_attackTimer = 0.0f; 
			}
			else if (!_sprite.IsPlaying() || !_sprite.Animation.ToString().Contains("Attack"))
		{
			_sprite.Play("idle"); 
		}
	}
	}
	private bool CanAttack()
	{
		if (_currentTarget == null || !IsInstanceValid(_currentTarget)) return false;
		float distance = GlobalPosition.DistanceTo(_currentTarget.GlobalPosition);
		return distance <= AttackRange && _attackTimer >= 1.0f / APS;
	}

	private void PerformAttack()
	{
		Vector2 dir = (_currentTarget.GlobalPosition - GlobalPosition).Normalized();
		string animName = GetAttackAnimationName(dir);
		
		_sprite.Play(animName);
		_sprite.SpeedScale = APS; 

		GD.Print($"Attack: {_currentTarget.Name}! Hit: {Damage}");

	}
	private string GetAttackAnimationName(Vector2 direction)
	{
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			_sprite.FlipH = direction.X < 0;
			return "Attack_right";
		}
	
		else
		{
			_sprite.FlipH = false; 
			return direction.Y > 0 ? "Attack_down" : "Attack_up";
		}
	}

	private Node2D GetClosestEnemy() 
	{ 
		return null; 
	}
}


			
