using Godot;
using System;

public partial class Enemy : Node
{
	[Signal] public delegate void DiedEventHandler(float xpValue);
	[Export] public int MaxHP = 10;
	[Export] public float XPValue = 10.0f;
	[Export] public float Damage = 10.0f;
	[Export] private Area2D _hurtBox;
	private float _currentHP;

public override void _Ready()
	{
		_currentHP = MaxHP;
	}

	public void TakeDamage(float damage)
	{
		_currentHP -= damage;
		_currentHP = Mathf.Clamp(_currentHP, 0, MaxHP);
		
		if(_currentHP <= 0)
		{
			Die();
		}
	}
	
	   private void Die()
	{
		EmitSignal(nameof(Died), XPValue);
		QueueFree();
	}
}
