using Godot;
using System;

public partial class Goblin : CharacterBody2D 
{
    [Export] public float Speed = 120.0f;
    [Export] public float StopDistance = 50.0f;
    [Export] public float AttackCooldown = 1.5f;
    [Export] public int Damage = 10;

    private AnimatedSprite2D _animatedSprite; 
    private Node2D _player;
    
    private bool _isAttacking = false;
    private float _attackTimer = 0.0f;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        _animatedSprite = GetNode<AnimatedSprite2D>("Goblin");

        _animatedSprite.AnimationFinished += OnAnimationFinished;
        _animatedSprite.FrameChanged += OnFrameChanged;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null) return;

        _attackTimer += (float)delta;

        float distanceToPlayer = GlobalPosition.DistanceTo(_player.GlobalPosition);
        Vector2 direction = GlobalPosition.DirectionTo(_player.GlobalPosition);

        if (_isAttacking) 
        {
            if (distanceToPlayer > StopDistance + 30.0f)
            {
                _isAttacking = false;
            }
            else
            {
                Velocity = Vector2.Zero;
                MoveAndSlide();
                return;
            }
        }

        if (distanceToPlayer > StopDistance)
        {
            Velocity = direction * Speed;
            _animatedSprite.Play("walk");
            _animatedSprite.FlipH = direction.X < 0; 
        }
        else
        {
            Velocity = Vector2.Zero;
            
            if (_attackTimer >= AttackCooldown)
            {
                StartAttack();
            }
        }

        MoveAndSlide();
    }

    private void StartAttack()
    {
        _isAttacking = true;
        _attackTimer = 0.0f;
        _animatedSprite.Play("attack");
        _animatedSprite.FlipH = (GlobalPosition.DirectionTo(_player.GlobalPosition).X < 0);
    }

    private void OnFrameChanged()
    {
        if (_animatedSprite.Animation == "attack" && _animatedSprite.Frame == 4)
        {
            CheckHit();
        }
    }

    private void CheckHit()
    {
        if (_player == null) return;
        
        float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
        if (distance <= StopDistance + 15.0f) 
        {
            GD.Print("Hit!");
        }
    }

    private void OnAnimationFinished()
    {
        if (_animatedSprite.Animation == "attack")
        {
            _isAttacking = false;
        }
    }
}