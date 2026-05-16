using Godot;
public partial class SkillCard : TextureButton
{
	[Export] private TextureRect _icon;
	[Export] private Label _name;
	private string _id;
	[Export] private Label _description;
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Pressed += OnSelectPressed;
	}

	private void OnSelectPressed()
	{
		
		var skillManager = GetNode<SkillManager>("/root/SkillManager"); 
		skillManager.SelectedSkill(_id);
	}
	public void Setup(SkillTemplate template)
	{
		_id = template.ID;
		_name.Text = template.Name; 
		
		var description = "";
		foreach (var stat in template.Stats){
		description += $"+{stat.Value} {stat.Key}\n";
	}
		_description.Text = description;
		
		if (!string.IsNullOrEmpty(template.IconPath))
		{
			var texture = ResourceLoader.Load<Texture2D>(template.IconPath);
			if (texture != null)
				_icon.Texture = texture;
		}
	}
}
