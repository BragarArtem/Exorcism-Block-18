using System.Collections.Generic;
using System.Text.Json.Serialization;
public enum SkillType 
{
General,
Class,
Weapon

}
public enum SkillSubtype { None, ShortSword, LongSword, Knight }
public enum SkillUsageType { Active, Passive }
public class SkillTemplate

{
    [JsonPropertyName("name")] 
    public string Name { get; set; } =" ";

    [JsonPropertyName("icon_path")]
    public string IconPath { get; set; }
    
    [JsonPropertyName ("id")]
    public string ID { get; set; } = " ";

    [JsonPropertyName("type")] 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkillType Type {get; set;} 

    [JsonPropertyName("usage_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    
    public SkillUsageType UsageType { get; set; }

    [JsonPropertyName("subtype")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SkillSubtype Subtype { get; set; }

    [JsonPropertyName ("drop_weight")]
    public float DropWeight {get; set;} 

    [JsonPropertyName("stats")]
    public Dictionary<string, float> Stats {get; set;} = new Dictionary<string, float>(); 
}






