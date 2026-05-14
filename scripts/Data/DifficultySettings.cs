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
                    HPMultiplier = 4f; DamageMultiplier = 4f; SpeedMultiplier = 1f;  GoldMultiplier = 0.4f;
                }break;
            case "Abyssal":
                {
                    HPMultiplier = 16f; DamageMultiplier = 16f; SpeedMultiplier = 1.5f; GoldMultiplier = 1.5f;
                }break;
            case "Eldritch":
                {
                    HPMultiplier = 64f; DamageMultiplier = 64f; SpeedMultiplier = 2f; GoldMultiplier = 3f; 
                }break;
            case "Forsaken":
                {
                    HPMultiplier = 252f; DamageMultiplier = 252f; SpeedMultiplier = 2.5f; GoldMultiplier = 9f;
                }break;
        }
    }
}