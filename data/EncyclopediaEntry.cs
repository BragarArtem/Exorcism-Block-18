using System.Text.Json.Serialization;
public class EnemyStats
{
    [JsonPropertyName("hp")] public float HP {get; set;}
    [JsonPropertyName("damage")] public float Damage {get; set;}
    [JsonPropertyName("critical_damage")] public float CriticalDamage {get; set;}
    [JsonPropertyName("speed")] public float Speed {get; set;}
}
public class EncyclopediaEntry
{
    [JsonPropertyName("id")] public string ID {get; set;}
    [JsonPropertyName("name")] public string Name {get; set;}
    [JsonPropertyName("icon_path")] public string IconPath {get; set;}
    [JsonPropertyName("is_unlocked")] public float IsUnlocked {get; set;}
    [JsonPropertyName("lore")] public string Lore {get; set;}
    [JsonPropertyName("stats")] public EnemyStats Stats {get; set;}
}
