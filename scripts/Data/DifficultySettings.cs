using System;
using Godot;
public static class DifficultySettings
{
    public static float HPMultiplier;
    public static float DamageMultiplier;
    public static float GoldMultiplier;
    public static void Apply(string difficulty)
    {
        switch (difficulty)
        {
            case "Hollow":
                {
                    HPMultiplier = 0.75f; DamageMultiplier = 0.75f; GoldMultiplier = 0.5f;
                }break;
            case "Cursed":
                {
                    HPMultiplier = 1f; DamageMultiplier = 1f; GoldMultiplier = 1f;
                }break;
            case "Abyssal":
                {
                    HPMultiplier = 2f; DamageMultiplier = 2f; GoldMultiplier = 1.5f;
                }break;
            case "Eldritch":
                {
                    HPMultiplier = 5f; DamageMultiplier = 5f; GoldMultiplier = 3f;
                }break;
            case "Forsaken":
                {
                    HPMultiplier = 10f; DamageMultiplier = 10f; GoldMultiplier = 5f;
                }break;
        }
    }
}