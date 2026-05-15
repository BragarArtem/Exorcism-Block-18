using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class SkillManager : Node
{
    [Signal] public delegate void SkillSelectedEventHandler(string id);
    [Signal] public delegate void AllSlotsFilledEventHandler();
    [Signal] public delegate void SkillOffersReadyEventHandler(string[] offeredIds); 
    [Signal] public delegate void SkillOfferCancelledEventHandler();
private SkillFactory _skillFactory;
private SaveManager _saveManager;
private int MaxSkillSlots = 6;
private int SkillOffer = 3;
private static int [] GoldDropValue = {25, 50, 100};
private Dictionary<string, SkillFactory.SkillInstance> _activeSkills = new();
private List<string> _currentOffers = new();
public override void _Ready()
    {
        
        _skillFactory = GetNode<SkillFactory>("/root/SkillFactory");
        _saveManager = GetNode<SaveManager>("/root/SaveManager");
    }

    public void SkillOffers()
    {
        if (_activeSkills.Count >= MaxSkillSlots)
        {
        return;
        }
        _currentOffers = GenerateOffers(SkillOffer);
        if (_currentOffers.Count == 0) {
            return;
        }
        EmitSignal(SignalName.SkillOffersReady, _currentOffers.ToArray());
    }

    public bool SelectedSkill (string id)
    {
        if (!_currentOffers.Contains(id))
        {
            GD.PrintErr($"Skill {id} is not in current offers.");
            return false;
        }
        bool added = TryAddSkill(id);
        if (added)
        {
        _currentOffers.Clear();
        EmitSignal(SignalName.SkillOfferCancelled);
        }
        return added;
    }
    public bool TryAddSkill (string id)
    {
        if (_activeSkills.ContainsKey(id))
        {
            GD.PrintErr($"Skill {id} already used ");
            return false;
        }
    
        if (_activeSkills.Count >= MaxSkillSlots)
        {
            GD.Print("Max skill slots reached");
            return false;
        }
        var template = _skillFactory.GetTemplate(id);
        if (template == null)
            return false;

        _activeSkills[id] = new SkillFactory.SkillInstance(template);
        EmitSignal(SignalName.SkillSelected, id);
        if (_activeSkills.Count == MaxSkillSlots)
        EmitSignal(SignalName.AllSlotsFilled);
            return true;
    }
    private List<string> GenerateOffers(int count)
    {
        var allIds = _skillFactory.GetAllTemplates();
        var available = allIds
        .Where(t => !_activeSkills.ContainsKey(t.ID))
        .OrderBy (_ => GD.Randi())
        .Take(count)
        .Select(t=> t.ID)
        .ToList();
        return available;
    }
    public SkillTemplate GetTemplate(string id) => _skillFactory.GetTemplate(id);
}

            
        
    
