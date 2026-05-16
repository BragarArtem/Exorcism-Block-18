using Godot;
using System;

public partial class LvlProgression : Node
{
    // Export for tests
    [Export] int baseExp = 50;
	public int CalculateXpForLevel(int level)
	{
        if (level < 0) 
		return 0;
        return baseExp * (int)Math.Pow(4, level);
    }
}
	

