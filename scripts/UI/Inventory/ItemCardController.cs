using Godot;
using System;

public partial class ItemCardController : TextureButton
{
    [Signal] public delegate void ClickedEventHandler(string instanceID);
    [Export] public TextureRect ItemIcon;
    public BaseItemInstance Item;
    public override void _Ready()
    {   
        Pressed += () => EmitSignal(SignalName.Clicked, Item.InstanceID);
    }
    public void Setup(BaseItemInstance item, string iconPath)
    {
        Item = item;
        ItemIcon.Texture = GD.Load<Texture2D>(iconPath);
    }
}
