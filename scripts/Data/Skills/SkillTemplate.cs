using System.Collections.Generic;
using System.Text.Json.Serialization;
public enum SkillType 
{
General,
Class,
Weapon,

}
public class SkillTemplate
{
    [JsonPropertyName("name")] 
    public string Name { get; set; } =" ";
    
    [JsonPropertyName ("id")]
    public string ID { get; set; } = " ";

    [JsonPropertyName("type")] 
    public string Type {get; set;} =" ";

    [JsonPropertyName ("drop_weight")]
    public float DropWeight {get; set;} 

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SkillSubtype { None, ShortSword, LongSword, Knight }
    public Dictionary<string, float> Stats {get; set;} = new Dictionary<string, float>(); 
}






