using Godot;
using System;

public partial class LvlProgression : Node
{
	[Export] public int baseXp = 10;
	[Export] public float xpMultiplier = 1.2f;  

	public int CalculateXpForLevel(int level)
	{
		if (level <= 1)
		{
			return 0; 
		}

		 double xp = baseXp * Math.Pow(level - 1, 2) * xpMultiplier;
		return (int) Math.Max(xp, 0); 
	}
}
