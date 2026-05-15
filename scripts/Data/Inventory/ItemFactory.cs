using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;
public partial class ItemFactory : Node
{
    private List<ItemTemplate> _itemTemplates = new List<ItemTemplate>();
    private List<TalismanTemplate> _talismanTemplates = new List<TalismanTemplate>();
    private readonly Random _rng = new Random();
    public override void _Ready()
    {
        if (FileAccess.FileExists("res://data/InventorySystem/ItemTemplates.json"))
        {
            try
            {
                using var file = FileAccess.Open("res://data/InventorySystem/ItemTemplates.json", FileAccess.ModeFlags.Read);
                string json = file.GetAsText();
                _itemTemplates = JsonSerializer.Deserialize<List<ItemTemplate>>(json);
            } catch(Exception ex)
            {
                GD.PrintErr($"Error ({ex}) occured during opening ItemTemplates.json");
            }
        }
        if(FileAccess.FileExists("res://data/InventorySystem/TalismanTemplates.json"))
        {
            try
            {
                using var file = FileAccess.Open("res://data/InventorySystem/TalismanTemplates.json", FileAccess.ModeFlags.Read);
                string json = file.GetAsText();
                _talismanTemplates = JsonSerializer.Deserialize<List<TalismanTemplate>>(json);
            } catch(Exception ex)
            {
                GD.PrintErr($"Error ({ex}) occured during opening TalismanTemplates.json");
            }
        }   
    }
    
    public ItemInstance CreateItem(string TemplateID)
    {
        var template = _itemTemplates.FirstOrDefault(t => t.ID == TemplateID);
        if(template != null)
        {
            var instance = new ItemInstance();
            float statSum = 0f;
            foreach(var stat in template.Stats)
            {
                float value = (float)(_rng.NextDouble() * (stat.Value.Max - stat.Value.Min) + stat.Value.Min);
                instance.Stats[stat.Key] = value;
                statSum += value;
            }
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(template.Name);
            foreach(var stat in instance.Stats)
            {
                sb.AppendLine($"{stat.Key}: {stat.Value:F2}");  
            }
            instance.Description = sb.ToString();
            instance.TemplateID = TemplateID;
            instance.Price = template.PriceBase * ((decimal)statSum / template.Stats.Count);
            return instance;
            
        }
        else
        {
            GD.PrintErr($"Template{TemplateID} not found");
            return null;
        }
    }
    public TalismanInstance CreateTalisman(string TemplateID)
    {
        var template = _talismanTemplates.FirstOrDefault(t => t.ID == TemplateID);
        if(template != null)
        {
            var instance = new TalismanInstance();
            float statSum = 0f;
            foreach(var stat in template.Effect.Stats)
            {
                float value = (float)(_rng.NextDouble() * (stat.Value.Max - stat.Value.Min) + stat.Value.Min);
                instance.Stats[stat.Key] = value;
                statSum += value;
            }
            instance.TemplateID = TemplateID;
            instance.Price = template.PriceBase * ((decimal)statSum / template.Effect.Stats.Count);
            return instance;
            
        }
        else
        {
            GD.PrintErr($"Template{TemplateID} not found");
            return null;
        }
    }
    public ItemTemplate GetItemTemplate(string templateID)
    {
        return _itemTemplates.FirstOrDefault(t=> t.ID == templateID);

    }
    public TalismanTemplate GetTalismanTemplate(string templateID)
    {
        return _talismanTemplates.FirstOrDefault(t=> t.ID == templateID);
    }
}

