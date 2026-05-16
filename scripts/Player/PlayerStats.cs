using Godot;

public partial class Player
{
    public void ApplyStatBonus ( string stat, float value)
    {
    switch (stat)
    {
        case "max_hp" :
        MaxHP += value;
        _currentHP += value;
        _hud?.UpdateHP(_currentHP, MaxHP);
        break;

        case "move_speed" :
        Speed += value;
        break;

        case "attack_speed" :
        APS += value;
        break;
    }
    }
}