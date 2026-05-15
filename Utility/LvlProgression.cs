using Godot;
using System;

public partial class LvlProgression : Node
{
	public int CalculateXpForLevel(int level)
	{
        if (level <= 0) 
		return 0;
        return (int)Math.Pow(4, level);
    }
}
	

