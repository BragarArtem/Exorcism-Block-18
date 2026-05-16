using Godot;


public partial class SkillSelectionUI : CanvasLayer
{
	[Export] private HBoxContainer _cardsContainer;
	[Export] private PackedScene _skillCardScene;
	private SkillManager _skillManager;



public override void _Ready()
	{
	_skillManager = GetNode<SkillManager>("/root/SkillManager");

	_skillManager.SkillOffersReady += OnOffersReady;
	_skillManager.SkillOfferCancelled += OnOfferCancelled;
		Hide();
	}

	private void OnOffersReady(string [] ids)
	{
		foreach (Node child in _cardsContainer.GetChildren())
		{
			child.QueueFree();
		}
			
			foreach (var id in ids)
		{
			var template = _skillManager.GetTemplate(id);
			if (template == null) 
			continue;
			var card = _skillCardScene.Instantiate<SkillCard>();
			_cardsContainer.AddChild(card);
			card.Setup(template);
		}
		Show();  
	}
	private void OnOfferCancelled() => Hide();
}
