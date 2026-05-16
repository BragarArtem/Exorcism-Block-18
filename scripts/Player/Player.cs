using Godot;
using System.Collections.Generic;

public enum AttackType {Single, Plural}
public partial class Player : CharacterBody2D
{
	//Player parameters
	[Export] public float Speed = 67.0f;
	[Export] public float MaxHP = 100.0f;
	//Attack parameters
	[Export] public float APS = 2.0f;
	[Export] public float Damage = 5.0f;
	[Export] public AttackType CurrentAttackType = AttackType.Single;
	[Export] public float PassiveAPS = 1.0f;
	[Export] public float PassiveDamage = 3.0f;
	[Export] public bool PassiveIsEnabled = true;
	//Private fields
	private AnimatedSprite2D _hero;
	private AttackZone _attackZone;
	private HudController _hud;
	private LvlProgression _lvlProgression;
	private SaveManager _saveManager;
	private string _currentDir = "down";
	private float _activeTimer = 0.0f;
	private float _passiveTimer = 0.0f;
	private bool _isAttacking = false;
	private float _currentHP;
	private SkillManager _skillManager;

	public override void _Ready()
	{

		_saveManager = GetNode<SaveManager>("/root/SaveManager");
		_lvlProgression = GetNode<LvlProgression>("/root/LvlProgression");
		_skillManager = GetNode<SkillManager>("/root/SkillManager");
		_skillManager.SkillSelected += OnSkillSelected;
		
		_hero = GetNode<AnimatedSprite2D>("Hero");
		_hero.AnimationFinished += OnAnimationFinished;
		var hurtBox = GetNode<Area2D>("HurtBox");
		hurtBox.Connect("Hurt", Callable.From<float>(TakeDamage));
		_hud = GetTree().GetFirstNodeInGroup("hud") as HudController;
		_attackZone = GetNode<AttackZone>("AttackZone");
		_attackZone.UpdateConeShape();

		_currentHP = MaxHP;
		_hud?.UpdateHP(_currentHP, MaxHP);
		int initialNeeded = _lvlProgression.CalculateXpForLevel(
		_saveManager.CurrentSaveData.CurrentLevel + 1);
		_hud?.UpdateExp(
		_saveManager.CurrentSaveData.CurrentExp,
		initialNeeded);

	}
	public override void _PhysicsProcess(double delta)
	{
		_activeTimer += (float)delta;
		_passiveTimer += (float)delta;
		Godot.Vector2 directionToMouse = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		HandleMovement(directionToMouse);
		HandleActiveAttack(directionToMouse);
		if(PassiveIsEnabled) HandlePassiveAttack();
	}
	private void HandleMovement(Godot.Vector2 directionToMouse)
	{
		Godot.Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = inputDir * Speed;
		MoveAndSlide();
		if (!_isAttacking)
		{
			UpdateDirection(directionToMouse);
			UpdateAnimations(inputDir);
		}
		_attackZone.RotateTo(directionToMouse);
	}
	private void HandleActiveAttack(Godot.Vector2 dir)
	{
		if(_activeTimer >= 1.0f / APS){_attackZone.ShowReady();} else {_attackZone.ShowCooldown();}
		if(!Input.IsActionPressed("attack") || _isAttacking || _activeTimer < 1.0f / APS) return;
		switch(CurrentAttackType)
		{
			case AttackType.Single: AttackSingle(dir); break;
			case AttackType.Plural: AttackPlural(dir); break;
		}
		_activeTimer = 0.0f;
	}
	private void AttackSingle(Godot.Vector2 dir)
	{
		PlayAttackAnimation(dir);
		var target = GetClosestInCone(dir);
		if (target == null) return;
		target.Call("TakeDamage", (float)Damage);
	}
	private void AttackPlural(Godot.Vector2 dir)
	{
		PlayAttackAnimation(dir);
		var targets = GetAllInCone(dir);
		if (targets.Count == 0) return;
		foreach(var target in targets){if(target.HasMethod("TakeDamage")){target.Call("TakeDamage", (float)Damage);}}	
	}
	private void HandlePassiveAttack()
	{
		if(_passiveTimer < 1.0f / PassiveAPS) return;
		var targets = GetAllInCone(_attackZone.GetForwardDirection());
		foreach(var target in targets){target.Call("TakeDamage", (float)PassiveDamage);}
		_passiveTimer = 0.0f;
	}
	private void PlayAttackAnimation(Godot.Vector2 dir)
	{
		_isAttacking = true;
		if(Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
		{
			_hero.FlipH = dir.X < 0;
			_currentDir = "right";
		}
		else
		{
			_hero.FlipH = false;
			_currentDir = dir.Y > 0 ? "down": "up";
		}
		_hero.Play($"Attack_{_currentDir}");
		_hero.SpeedScale = APS;
	}
	private void OnAnimationFinished()
	{
		if (_hero.Animation.ToString().StartsWith("Attack"))
		{
			_isAttacking = false;
			_hero.SpeedScale = 1.0f;
		}
	}
	private void UpdateDirection(Godot.Vector2 dir)
	{
		if(Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
		{
			_hero.FlipH = dir.X < 0;
			_currentDir = "right";
		}
		else
		{
			_hero.FlipH = false;
			_currentDir = dir.Y > 0 ? "down": "up";
		}
	}
	private void UpdateAnimations(Godot.Vector2 inputDir)
	{
		string action = inputDir.Length() > 0 ? "Run" : "Idle";
		_hero.Play($"{action}_{_currentDir}");
	}
	private Node2D GetClosestInCone(Godot.Vector2 dir)
	{
		Node2D closest = null;
		float minDist = float.MaxValue;
		foreach (var enemy in GetEnemiesInCone(dir))
		{
			float dist = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
			if (dist < minDist){minDist = dist; closest = enemy;}
		}
		return closest;
	}
	private List<Node2D> GetAllInCone(Godot.Vector2 dir)
	{
		return GetEnemiesInCone(dir);
	}
	private List<Node2D> GetEnemiesInCone(Godot.Vector2 dir)
	{
		var result = new List<Node2D>();
		float halfAngle = _attackZone.AttackAngle / 2f;
		float radiusSq = _attackZone.AttackRange * _attackZone.AttackRange;
		foreach(Node node in GetTree().GetNodesInGroup("Enemies"))
		{
			if(node is not CharacterBody2D enemy || !IsInstanceValid(enemy)) continue;
			float distSq = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
			if(distSq > radiusSq) continue;
			Godot.Vector2 toEnemy = (enemy.GlobalPosition - GlobalPosition).Normalized(); 
			float angle = Mathf.RadToDeg(dir.AngleTo(toEnemy));
			if(Mathf.Abs(angle) > halfAngle) continue;
			result.Add(enemy);
		}
		return result;
	}
	public void TakeDamage(float damage)
	{
		_currentHP -= damage;
		_currentHP = Mathf.Clamp(_currentHP, 0, MaxHP);
		_hud?.UpdateHP(_currentHP, MaxHP);
		if(_currentHP <= 0)
		{
			if (HasNode("/root/SaveManager"))
			{
				var saveManager = GetNode<SaveManager>("/root/SaveManager");
				saveManager.CurrentSaveData.Gold += 100;
				saveManager.CurrentSaveData.BestRunScore.Add(500);
				saveManager.CurrentSaveData.UnlockedEncyclopediaIds.Add("goblin");
				saveManager.CurrentSaveData.CurrentExp = 0f;
				saveManager.CurrentSaveData.CurrentLevel = 0;
				var itemFactory = GetNode<ItemFactory>("/root/ItemFactory");
				var testItem = itemFactory.CreateItem("short_sword_t1");
				saveManager.CurrentSaveData.Inventory.Add(testItem);
				saveManager.SaveGame(saveManager.CurrentSaveData);
			}
			GetTree().ChangeSceneToFile("res://scences/DeathScreen.tscn");
		}
	}
	public void GainExp(float amount)
	{
		_saveManager.CurrentSaveData.CurrentExp += amount;
		int neededExp = _lvlProgression.CalculateXpForLevel(_saveManager.CurrentSaveData.CurrentLevel + 1);
		// test for xp bar 
		GD.Print($"XP: {_saveManager.CurrentSaveData.CurrentExp}/{neededExp} | Level: {_saveManager.CurrentSaveData.CurrentLevel}");
		while (_saveManager.CurrentSaveData.CurrentExp >= neededExp)
		{
			_saveManager.CurrentSaveData.CurrentExp -= neededExp;
			_saveManager.CurrentSaveData.CurrentLevel++;
			_skillManager.SkillOffers();
			neededExp = _lvlProgression.CalculateXpForLevel(_saveManager.CurrentSaveData.CurrentLevel + 1);
			// test fo lvl up
			GD.Print($"Level Up!: {_saveManager.CurrentSaveData.CurrentLevel}");
		}
		
			_hud?.UpdateExp (
			_saveManager.CurrentSaveData.CurrentExp,
			neededExp
	);
	}
	private void OnSkillSelected (string id)
	{
	var template = _skillManager.GetTemplate(id);
	foreach (var stat in template.Stats)
	{
		ApplyStatBonus(stat.Key, stat.Value);
		}
	}
}
