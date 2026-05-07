using System.Collections.Generic;
using System.Text.Json.Serialization;

public class StatRange
{
    [JsonPropertyName("min")]
    public float Min {get; set;}
    [JsonPropertyName("max")]
    public float Max {get; set;}
}
public class ItemTemplate
{
    [JsonPropertyName("id")]
    public string ID {get; set;} = "";

    [JsonPropertyName("name")]
    public string Name {get; set;} = "";

    [JsonPropertyName("category")]
    public string Category {get; set;} = "";
    [JsonPropertyName("slot")]
    public string Slot {get; set; } = "";

    [JsonPropertyName("tier")]
    public int Tier {get; set;}

    [JsonPropertyName("price_base")]
    public decimal PriceBase {get; set;}

    [JsonPropertyName("icon_path")]
    public string IconPath {get; set;} = "";

    [JsonPropertyName("stats")]
    public Dictionary<string, StatRange> Stats {get; set;} = new Dictionary<string, StatRange>(); 

}