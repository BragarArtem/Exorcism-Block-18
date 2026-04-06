using Godot;
using System;


public partial class HitBox : Area2D
{
	[Export] public int Damage = 1;
private CollisionShape2D _collisionShape;
private Timer _disableTimer;

public override void _Ready()
{
	_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	_disableTimer = GetNode<Timer>("DisableHitBoxTimer");
	}
private void tempdisable()
{
	_collisionShape.SetDeferred("disabled", true);
	_disableTimer.Start();
}
private void OnDisableHitBoxTimerTimeout()
	{
		_collisionShape.SetDeferred("disable", false);
	}
}
