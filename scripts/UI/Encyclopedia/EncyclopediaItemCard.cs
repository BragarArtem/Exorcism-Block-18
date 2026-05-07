using Godot;

public partial class EncyclopediaItemCard : Panel
{
    [Export] public TextureRect ItemIcon;
    [Export] public Label ItemName;
    private const string UnknownIconPath = "res://sprites/Icons/cultist.svg";
    public void Setup(EncyclopediaEntry entry)
    {
        if(entry.IsUnlocked == false)
        {
            ItemName.Text = "???";
            ItemIcon.Texture = GD.Load<Texture2D>(UnknownIconPath);
        }
        else
        {
            ItemName.Text = entry.Name;
            ItemIcon.Texture = GD.Load<Texture2D>(entry.IconPath);
        }
    }
}
