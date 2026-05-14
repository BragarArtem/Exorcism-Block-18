using Godot;
using System;

public partial class ItemCardController : TextureButton
{
    [Signal] public delegate void ClickedEventHandler(BaseItemInstance Item);
    [Export] public TextureRect ItemIcon;
    public BaseItemInstance Item;
    public override void _Ready()
    {   
        Pressed += () => EmitSignal(SignalName.Clicked, Item);
    }
    public void Setup(BaseItemInstance item, string iconPath)
    {
        Item = item;
        ItemIcon.Texture = GD.Load<Texture2D>(iconPath);
    }
}
