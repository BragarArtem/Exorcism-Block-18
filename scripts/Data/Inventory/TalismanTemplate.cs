using System.Collections.Generic;
using System.Text.Json.Serialization;

public class TalismanEffect
{
    [JsonPropertyName("type")]
    public string Type {get; set;} = "";
    [JsonPropertyName("is_random")]
    public bool IsRandom {get; set;} = false;
    [JsonPropertyName("stats")]
    public Dictionary<string, StatRange> Stats {get; set;} = new Dictionary<string, StatRange>();
}
public class TalismanTemplate
{
    [JsonPropertyName("id")]
    public string ID {get; set;} = "";

    [JsonPropertyName("name")]
    public string Name {get; set;} = "";

    [JsonPropertyName("price_base")]
    public decimal PriceBase {get; set;}

    [JsonPropertyName("icon_path")]
    public string IconPath {get; set;} = "";

    [JsonPropertyName("effect")]
    public TalismanEffect Effect {get; set;}= new TalismanEffect();

}