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
        if (FileAccess.FileExists("res://data/SkillsSystem/Skills.json"))
        {
            try
            {
                using var file = FileAccess.Open("res://data/SkillsSystem/Skills.json", FileAccess.ModeFlags.Read);
                string json = file.GetAsText();
                _skillTemplates = JsonSerializer.Deserialize<List<SkillTemplate>>(json);
            } catch(Exception ex)
            {
                GD.PrintErr($"Skills.json not found at {ex}");
            }
        }
    }
    public class SkillInstance {
    public SkillTemplate Template {get;}
    public int Level {get; private set;} = 1;
    public int MaxLevel = 5;
    public SkillInstance( SkillTemplate template)
        {
            Template = template;
        }
        public void LevelUp()
        {
            if ( Level < MaxLevel)
            Level++;
        }
    }
    public SkillTemplate GetTemplate(string id)
    {
        var template = _skillTemplates.FirstOrDefault(t => t.ID == id);
        if (template != null)
        {
            return template;
        }
        else
        {
            GD.PrintErr($"SkillTemplate {id} not found");
            return null;
            
        }


    }
        public List<SkillTemplate> GetAllTemplates() => _skillTemplates;
}

