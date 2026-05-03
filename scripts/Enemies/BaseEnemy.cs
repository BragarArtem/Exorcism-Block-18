using Godot;
using System;
using System.Transactions;

public partial class BaseEnemy : CharacterBody2D
{

[Export] public float Speed = 120.0f;
[Export] public float Health = 100.0f;
[Export] public float Damage = 0;

protected float currentHealth;
protected Node2D player;

    public override void _Ready()
    {
        currentHealth = Health;
        player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        currentHealth = Health * DifficultySettings.HPMultiplier;
        Damage = Damage * DifficultySettings.DamageMultiplier;
    
}
public override void _PhysicsProcess(double delta)
    {
        if (player == null) return;

        Vector2 direction = GlobalPosition.DirectionTo(player.GlobalPosition);
        Velocity = direction * Speed;
        MoveAndSlide();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
        protected virtual void Die()
        {
            QueueFree();
    }
}