using Godot;
using System;

public partial class Player : CharacterBody2D
{
  [Export] public float Speed = 300.0f;
  [Export] public float AttackRange = 150.0f;
  [Export] public float APS = 2.0f;
  [Export] public int Damage = 5;

  private AnimatedSprite2D _hero;
  private string _currentDir = "down";
  private float _attackTimer = 0.0f;
  private bool _isAttacking = false;
  private Node2D _currentTarget;

  public override void _Ready()
  {
	_hero = GetNode<AnimatedSprite2D>("Hero");
	_hero.AnimationFinished += OnAnimationFinished;
  }

  public override void _PhysicsProcess(double delta)
  {
	_attackTimer += (float)delta;
	_currentTarget = GetClosestEnemy();

	if (_currentTarget != null && IsInstanceValid(_currentTarget) && !_isAttacking)
	{
	  if (GlobalPosition.DistanceTo(_currentTarget.GlobalPosition) <= AttackRange && _attackTimer >= 1.0f / APS)
	  {
		PerformAttackSequence();
		_attackTimer = 0.0f;
	  }
	}
	
	  Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
	  Velocity = inputDir * Speed;
	  MoveAndSlide();
	if (!_isAttacking)
	{
	  UpdateDirection(inputDir);
	  UpdateAnimations(inputDir);
	}
  }

  private void PerformAttackSequence()
  {
	_isAttacking = true;
	Vector2 dirToTarget = (_currentTarget.GlobalPosition - GlobalPosition).Normalized();
	
	if (Mathf.Abs(dirToTarget.X) > Mathf.Abs(dirToTarget.Y))
	{
	  _hero.FlipH = dirToTarget.X < 0;
	  _currentDir = "right";
	}
	else
	{
	  _hero.FlipH = false;
	  _currentDir = dirToTarget.Y > 0 ? "down" : "up";
	}

	_hero.Play($"Attack_{_currentDir}");
	_hero.SpeedScale = APS;

	if (_currentTarget.HasMethod("TakeDamage"))
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

  private void UpdateDirection(Vector2 inputDir)
  {
	if (inputDir.Y > 0) _currentDir = "down";
	else if (inputDir.Y < 0) _currentDir = "up";
	else if (inputDir.X != 0) _currentDir = "right";

	if (inputDir.X < 0) _hero.FlipH = true;
	else if (inputDir.X > 0) _hero.FlipH = false;
  }

  private void UpdateAnimations(Vector2 inputDir)
  {
	string action = (inputDir.Length() > 0) ? "Run" : "Idle";
	_hero.Play($"{action}_{_currentDir}");
  }

  private Node2D GetClosestEnemy()
  {
	var enemies = GetTree().GetNodesInGroup("Enemies");
	Node2D closest = null;
	float minDistance = float.MaxValue;

	foreach (Node node in enemies)
	{
	  if (node is Node2D enemy && IsInstanceValid(enemy))
	  {
		float distSq = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
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
