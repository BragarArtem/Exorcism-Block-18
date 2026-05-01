using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int SaveVersion { get; set; } = 1;
    public int Gold { get; set; } = 0;
    public List <int> BestRunScore {get; set;} = new List<int>();
    public List <string> UnlockedEncyclopediaIds {get; set;} = new List<string>();
}
