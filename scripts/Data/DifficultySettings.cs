using System;
using Godot;
public static class DifficultySettings
{
    public static float HPMultiplier;
    public static float DamageMultiplier;
    public static float GoldMultiplier;
    public static float SpeedMultiplier;
    public static void Apply(string difficulty)
    {
        switch (difficulty)
        {
            case "Hollow":
                {
                    HPMultiplier = 1f; DamageMultiplier = 1f; SpeedMultiplier = 1f; GoldMultiplier = 0.1f;
                }break;
            case "Cursed":
                {
                    HPMultiplier = 3f; DamageMultiplier = 3f; SpeedMultiplier = 1f;  GoldMultiplier = 0.5f;
                }break;
            case "Abyssal":
                {
                    HPMultiplier = 9f; DamageMultiplier = 9f; SpeedMultiplier = 1.5f; GoldMultiplier = 1.5f;
                }break;
            case "Eldritch":
                {
                    HPMultiplier = 27f; DamageMultiplier = 27f; SpeedMultiplier = 2f; GoldMultiplier = 5f; 
                }break;
            case "Forsaken":
                {
                    HPMultiplier = 81f; DamageMultiplier = 81f; SpeedMultiplier = 2.5f; GoldMultiplier = 10f;
                }break;
        }
    }
}