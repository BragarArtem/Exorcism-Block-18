using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int SaveVersion { get; set; } = 1;
    private decimal _gold;
    public decimal Gold { get => _gold; set => Math.Round(value, 2); }
    public List <int> BestRunScore {get; set;} = new List<int>();
    public List <string> UnlockedEncyclopediaIds {get; set;} = new List<string>();
    public string Difficulty {get; set;} = "Cursed";
    public List<ItemInstance> Inventory {get; set;} = new List<ItemInstance>();
    public Dictionary<string, ItemInstance> EquipedItems = new Dictionary<string, ItemInstance>();
}
