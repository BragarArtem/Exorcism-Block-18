using System;
using Godot;

public partial class MerchantController : TextureRect
{
    private SaveManager _saveManager;
    private MerchantManager _merchantManager;
    private InventoryManager _inventoryManager; 
    private ItemFactory _itemFactory;
    private Label _goldLabel;
    private VBoxContainer _itemsContainer;
    private Button _buyButton;
    private BaseItemInstance _selectedItem;
    private PackedScene _itemCard = GD.Load<PackedScene>("res://scences/ItemCard.tscn");

    public override void _Ready()
    {
        _saveManager = GetNode<SaveManager>("/root/SaveManager");
        _merchantManager = GetNode<MerchantManager>("/root/MerchantManager");
        _inventoryManager = GetNode<InventoryManager>("/root/InventoryManager");
        _itemFactory = GetNode<ItemFactory>("/root/ItemFactory");
        _goldLabel = GetNode<Label>("GoldLabel");
        _itemsContainer = GetNode<VBoxContainer>("ItemStock");
        _buyButton = GetNode<Button>("BuyButton");
        _buyButton.Pressed += OnBuyPressed;
        RefreshShop();
        RefreshGoldLabel();
    }
    private void RefreshShop()
    {
        foreach(var child in _itemsContainer.GetChildren())
        {
            child.QueueFree();
        }
        foreach(var item in _merchantManager.GetStock())
        {
            var card = _itemCard.Instantiate<ItemCardController>();
            var template = _itemFactory.GetItemTemplate(item.TemplateID);
            string iconPath = template?.IconPath ?? "";

            _itemsContainer.AddChild(card);
            card.Setup(item, iconPath);
            card.Clicked += (instanceID) =>
            {
                _selectedItem = _merchantManager.GetStock().Find(i => i.InstanceID == instanceID);
            };
        }
    }
    private void RefreshGoldLabel()
    {
        _goldLabel.Text = $"gold: {_saveManager.CurrentSaveData.Gold}";
    }
    private void OnBuyPressed()
    {
        if(_selectedItem == null) return;
        if(_saveManager.CurrentSaveData.Gold < _selectedItem.Price)
        {
            _goldLabel.Text = "Not enough gold";
            return;
        }
        _saveManager.CurrentSaveData.Gold -= _selectedItem.Price;
        _inventoryManager.AddItem(_selectedItem);
        _saveManager.CurrentSaveData.MerchantStock.Remove(_selectedItem);
        _selectedItem = null;
        _saveManager.SaveGame(_saveManager.CurrentSaveData);
        RefreshShop();
        RefreshGoldLabel();
    }
        
}