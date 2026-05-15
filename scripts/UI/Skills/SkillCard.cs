using Godot;
public partial class SkillCard : Panel
{
    [Export] private TextureRect _icon;
    [Export] private Label _name;
    [Export] private Button _selectButton;
    private string _id;

    private void OnSelectPressed()
    {
        var skillManager = GetNode<SkillManager>("/root/SkillManager"); 
        skillManager.SelectedSkill(_id);
    }
    public void Setup(SkillTemplate template)
    {
        _id = template.ID;
        _name.Text = template.Name; 
        if (!string.IsNullOrEmpty(template.IconPath))
        {
            var texture = ResourceLoader.Load<Texture2D>(template.IconPath);
            if (texture != null)
                _icon.Texture = texture;
        }
        _selectButton.Pressed += OnSelectPressed;
    }
}