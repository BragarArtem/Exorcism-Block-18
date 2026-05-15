using Godot;
using System;

public partial class SpawnLogic : Node2D
{
	private PackedScene _enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
	private RandomNumberGenerator _rng = new RandomNumberGenerator();
	public override void _Ready()
{

	_rng.Randomize();
	}
	private void OnEnemyTimerTimeout()
	{

	var PathFollow = GetNode<PathFollow2D>("../Player/Path2D/PathFollow2D");
	PathFollow.ProgressRatio = _rng.Randf();
	var enemyInstance = _enemyScene.Instantiate<Node2D>();
	enemyInstance.GlobalPosition = PathFollow.GlobalPosition;

	if (enemyInstance is Goblin goblin)
	{
	goblin.Connect(Goblin.SignalName.Died, new Callable (this, MethodName.OnEnemyDied));
	}
	GetParent().AddChild(enemyInstance);

	}
    // test method to handle enemy death signal
	private void OnEnemyDied(float xpValue)
	{
	GD.Print($"Enemy died, XP awarded: {xpValue}");
	}
}
