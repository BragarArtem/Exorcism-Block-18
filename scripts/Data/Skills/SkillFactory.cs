using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public partial class SkillFactory : Node
{
    private List<SkillTemplate> _skillTemplates = new List<SkillTemplate>();
    private Dictionary<string, SkillTemplate> _templatesById = new Dictionary<string, SkillTemplate>();
    public override void _Ready()
    {
         if (FileAccess.FileExists("res://data/SkillsSystem/SkillTemplates.json"))
        {
            try
            {
                using var file = FileAccess.Open("res://data/SkillsSystem/SkillTemplates.json", FileAccess.ModeFlags.Read);
                string json = file.GetAsText();
                _skillTemplates = JsonSerializer.Deserialize<List<SkillTemplate>>(json);
            } catch(Exception path)
            {
                GD.PrintErr($"SkillTemplates.json not found at {path}");
            }
        }
    }
    public SkillTemplate GetTemplate(string id)
    {
        var template = _skillTemplates.FirstOrDefault(t => t.ID == id);
        if (template != null)
        {
            return template;
        }

        return null;
    }
        public List<SkillTemplate> GetAllTemplates() => _skillTemplates;

}