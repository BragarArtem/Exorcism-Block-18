using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 67.0f;
	[Export] public float AttackAngle = 45.0f;
	[Export] public float AttackRange = 150.0f;
	[Export] public float APS = 2.0f;
	[Export] public int Damage = 5;
	[Export] public float MaxHP = 100.0f;

	private AnimatedSprite2D _hero;
	private string _currentDir = "down";
	private float _attackTimer = 0.0f;
	private bool _isAttacking = false;
	private Node2D _currentTarget;
	private float _currentHP;
	private HudController _hud;
	private AttackZone _attackZone;

	public override void _Ready()
	{
		_hero = GetNode<AnimatedSprite2D>("Hero");
		_hero.AnimationFinished += OnAnimationFinished;
		var hurtBox = GetNode<Area2D>("HurtBox");
		hurtBox.Connect("Hurt", Callable.From<float>(TakeDamage));
		_hud = GetTree().GetFirstNodeInGroup("hud") as HudController;
		_currentHP = MaxHP;
		_hud?.UpdateHp(_currentHP, MaxHP);
		_attackZone = GetNode<AttackZone>("AttackZone");	
		_attackZone.UpdateConeShape();
	}
	public void TakeDamage(float damage)
		{
			_currentHP -= damage;
			_currentHP = Mathf.Clamp(_currentHP, 0, MaxHP);
			_hud?.UpdateHp(_currentHP, MaxHP);
			if(_currentHP <= 0)
			{
				GD.Print("死");
				
				// Test for save system

				if (HasNode("/root/SaveManager"))
			{
				var saveManager = GetNode<SaveManager>("/root/SaveManager");
				saveManager.CurrentSaveData.Gold += 100;
				saveManager.CurrentSaveData.BestRunScore.Add(500); 
				saveManager.CurrentSaveData.UnlockedEncyclopediaIds.Add("goblin"); 

				saveManager.SaveGame(saveManager.CurrentSaveData);
			}
				GetTree().ChangeSceneToFile("res://scences/DeathScreen.tscn");
			}
		}	

	public override void _PhysicsProcess(double delta)
	{
		_attackTimer += (float)delta;

		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 directionToMouse = (mousePosition - GlobalPosition).Normalized();
		_currentTarget = GetClosestEnemy(directionToMouse, AttackAngle);
		if (!_isAttacking)
		{
		UpdateDirection(directionToMouse);
		_attackZone.RotateTo(directionToMouse);
		}

		if (_currentTarget != null && IsInstanceValid(_currentTarget) && !_isAttacking)
		{
		if (GlobalPosition.DistanceTo(_currentTarget.GlobalPosition) <= _attackZone.AttackRange && _attackTimer >= 1.0f / APS)
		{
			PerformAttackSequence(directionToMouse);
			_attackTimer = 0.0f;
		}
		}
		
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = inputDir * Speed;
		MoveAndSlide();
		if (!_isAttacking)
		{
		UpdateAnimations(inputDir);
		}
	}

	private void PerformAttackSequence(Vector2 directionToMouse)
	{
		_isAttacking = true;
		
		if (Mathf.Abs(directionToMouse.X) > Mathf.Abs(directionToMouse.Y))
		{
		_hero.FlipH = directionToMouse.X < 0;
		_currentDir = "right";
		}
		else
		{
		_hero.FlipH = false;
		_currentDir = directionToMouse.Y > 0 ? "down" : "up";
		}

		_hero.Play($"Attack_{_currentDir}");
		_hero.SpeedScale = APS;

		if (_currentTarget != null && IsInstanceValid(_currentTarget) && _currentTarget.HasMethod("TakeDamage"))
		{
		_currentTarget.Call("TakeDamage", Damage);
		}
	}

	private void OnAnimationFinished()
	{
		if (_hero.Animation.ToString().StartsWith("Attack"))
		{
		_isAttacking = false;
		_hero.SpeedScale = 1.0f;
		}
	}

	private void UpdateDirection(Vector2 dir)
	{
		if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
		{
			_currentDir = "right";
			_hero.FlipH = dir.X < 0;
		}
		else
		{
			_currentDir = dir.Y > 0 ? "down" : "up";
			_hero.FlipH = false;
		}
	}

	private void UpdateAnimations(Vector2 inputDir)
	{
		string action = (inputDir.Length() > 0) ? "Run" : "Idle";
		_hero.Play($"{action}_{_currentDir}");
	}

	private Node2D GetClosestEnemy(Vector2 zoneDirection, float zoneAngleDegrees)
	{
		var enemies = GetTree().GetNodesInGroup("Enemies");
		Node2D closest = null;
		float minDistance = float.MaxValue;

		foreach (Node node in enemies)
		{
			if (node is Node2D enemy && IsInstanceValid(enemy))
			{
				Vector2 directionToEnemy = (enemy.GlobalPosition - GlobalPosition).Normalized();
				float angle = Mathf.RadToDeg(zoneDirection.AngleTo(directionToEnemy));
				if (Mathf.Abs(angle) > zoneAngleDegrees / 2) continue;
				float distSq = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
				float radiusSq = AttackRange * AttackRange;
				if (distSq > radiusSq) continue;
				if (distSq < minDistance)
				{
				minDistance = distSq;
				closest = enemy;
				}
		}
		}
		return closest;
	}
}
