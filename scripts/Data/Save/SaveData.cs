using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int SaveVersion { get; set; } = 1;
    private decimal _gold;
    public decimal Gold { get => _gold; set => _gold = Math.Round(value, 2); }
    public List <int> BestRunScore {get; set;} = new List<int>();
    public List <string> UnlockedEncyclopediaIds {get; set;} = new List<string>();
    public string Difficulty {get; set;} = "Cursed";
    public List<BaseItemInstance> Inventory {get; set;} = new List<BaseItemInstance>();
    public Dictionary<string, BaseItemInstance> EquippedItems = new Dictionary<string, BaseItemInstance>();
    public Dictionary<string, int> AcquiredSkills {get; set;} = new Dictionary<string, int>();
    public List<BaseItemInstance> MerchantStock {get; set; } = new List<BaseItemInstance>();
    public DateTime LastMerchantRefresh{get ; set ;} = DateTime.MinValue;
}
