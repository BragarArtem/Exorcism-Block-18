using Godot;
using System;
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
[Signal] public delegate void HurtEventHandler(float damage);

private CollisionShape2D _collisionShape;
private Timer _disableTimer;
public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_disableTimer = GetNode<Timer>("DisableTimer");
		AreaEntered += OnEntered;
		_disableTimer.Timeout += () => _collisionShape.SetDeferred("disabled", false);
} 

private void OnEntered(Area2D area)
	{
		if (area.IsInGroup("attack"))
		{
			if(area is AttackZone attack)
			{
			
			float damage = attack.Damage;
			EmitSignal(SignalName.Hurt, damage);

		switch (HurtBoxType)
		{
			case HurtBoxTypeEnum.Cooldown:
			_collisionShape.SetDeferred("disabled", true);
			_disableTimer.Start();
				break;
			
			case HurtBoxTypeEnum.HitOnce:
			if (area.HasMethod("tempdisable"))
            {
            area.Call("tempdisable");
            }
			    break;
		}
	  }
	}
  }
}
