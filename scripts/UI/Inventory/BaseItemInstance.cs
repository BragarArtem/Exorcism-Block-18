using System;
using System.Collections.Generic;
using Godot;
public partial class BaseItemInstance : GodotObject
{
    public string TemplateID {get; set;} = "";
    public string InstanceID {get; set;} = Guid.NewGuid().ToString();
    public Dictionary<string, float> Stats {get; set;} = new Dictionary<string, float>();
    public decimal Price {get; set;} = 0;
    public bool IsEquipped {get; set;} = false;
    public float GetStat(string key)
    {
        if (Stats.ContainsKey(key))
        {
            return Stats[key];
        }
        else
        {
            return 0f;
        }
    }
}
public partial class ItemInstance : BaseItemInstance{}
public partial class TalismanInstance : BaseItemInstance{}