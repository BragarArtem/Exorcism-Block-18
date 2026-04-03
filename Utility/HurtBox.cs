using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;

public partial class HurtBox : Area2D
{
	public enum HurtBoxTypeEnum
	{
	Cooldown,
	HitOnce, 
	DisableHitBox,
}
[Export] public HurtBoxTypeEnum HurtBoxType = HurtBoxTypeEnum.Cooldown;
[Signal] public delegate void HitEventHandler(float damage);

private CollisionShape2D _collisionShape;
private Timer _disableTimer;
public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_disableTimer = GetNode<Timer>("DisableTimer");
	
	}
}
