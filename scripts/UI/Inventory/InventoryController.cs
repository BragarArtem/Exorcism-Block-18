using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryController : TextureRect
{
	private InventoryManager _invManager;
	private SaveManager _saveManager;
	private ItemFactory _itemFactory;
	private GridContainer _gridCont;
	private PackedScene _itemCard;
	private TextureRect _itemActionPanel;
	private TextureRect _inventorySortPanel;
	private TextureButton _equipButton;
	private TextureButton _sellButton;
	private TextureButton _closeButton;
	private ItemCardController _currentCard;
	private BaseItemInstance _currentItem;
	private Dictionary<string, TextureButton> _equipedItems = new Dictionary<string, TextureButton>();
	public override void _Ready()
	{
		_invManager = GetNode<InventoryManager>("/root/InventoryManager");
		_saveManager = GetNode<SaveManager>("/root/SaveManager");
		_itemFactory = GetNode<ItemFactory>("/root/ItemFactory");
		_gridCont = GetNode<GridContainer>("InventoryGrid/ScrollContainer/GridContainer");
		_itemCard = GD.Load<PackedScene>("res://scences/ItemCard.tscn");
		_itemActionPanel = GetNode<TextureRect>("InventoryGrid/ItemActionPanel");
		_inventorySortPanel = GetNode<TextureRect>("InventoryGrid/InventorySortPanel");
		_equipButton = GetNode<TextureButton>("InventoryGrid/ItemActionPanel/EquipButton");
		_sellButton = GetNode<TextureButton>("InventoryGrid/ItemActionPanel/SellButton");
		_closeButton = GetNode<TextureButton>("InventoryGrid/ItemActionPanel/CloseButton");
		_equipButton.Pressed += OnEquipPressed;
		_sellButton.Pressed += OnSellPressed;
		_closeButton.Pressed += OnClosePressed;
		_equipedItems["Helmet"] = GetNode<TextureButton>("EquipmentPanel/HelmetSlot");
		_equipedItems["Armour"] = GetNode<TextureButton>("EquipmentPanel/ArmourSlot");
		_equipedItems["Gloves"] = GetNode<TextureButton>("EquipmentPanel/GlovesSlot");
		_equipedItems["Boots"] = GetNode<TextureButton>("EquipmentPanel/BootsSlot");
		_equipedItems["Weapon"] = GetNode<TextureButton>("EquipmentPanel/WeaponSlot");
		_equipedItems["Talisman"] = GetNode<TextureButton>("EquipmentPanel/TalismanSlot");
		_equipedItems["Necklace"] = GetNode<TextureButton>("EquipmentPanel/NecklaceSlot");
		_equipedItems["Ring1"] = GetNode<TextureButton>("EquipmentPanel/RingSlot1");
		_equipedItems["Ring2"] = GetNode<TextureButton>("EquipmentPanel/RingSlot2");
		RefreshInventory();
	}
	public void RefreshInventory()
	{
		var _gridArray = _gridCont.GetChildren();
		foreach(var item in _gridArray)
		{
			item.QueueFree();
		}
		foreach(var item in _saveManager.CurrentSaveData.Inventory)
		{
			var card = _itemCard.Instantiate<ItemCardController>();
			var template = _itemFactory.GetItemTemplate(item.TemplateID);
			_gridCont.AddChild(card);
			card.Setup(item, template?.IconPath ?? "");
			card.Clicked += OnCardClicked;
		}
	}
	public void OnCardClicked(BaseItemInstance item)
	{
		_currentItem = item;
		_inventorySortPanel.Visible = false;
		_itemActionPanel.Visible = true;
	}
	public void OnEquipPressed()
	{
		_invManager.EquipItem(_currentItem);
		RefreshInventory();
	}
	public void OnSellPressed()
	{
		_invManager.SellItem(_currentItem);
		RefreshInventory();
	}
	public void OnClosePressed()
	{
		_currentItem = null;
		_itemActionPanel.Visible = false;
		_inventorySortPanel.Visible = true;
	}
}
