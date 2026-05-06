using System;
using System.Collections.Generic;
public class ItemInstance
{
    public string TemplateID {get; set;} = "";
    public string InstanceID {get; set;} = Guid.NewGuid().ToString();
    public Dictionary<string, float> ItemStats {get;set;} = new Dictionary<string, float>();
    public decimal Price {get; set;} = 0;
    public bool IsEquipped {get;set;} = false;
        public float GetStat(string key)
    {
        if (ItemStats.ContainsKey(key))
        {
            return ItemStats[key];
        }
        else
        {
            return 0f;
        }
    }
}
