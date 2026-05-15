using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
	private Label _equipLabel;
	private TextureButton _sellButton;
	private TextureButton _closeButton;
	private ItemCardController _currentCard;
	private BaseItemInstance _currentItem;
	private Dictionary<string, TextureButton> _equipSlots = new Dictionary<string, TextureButton>();
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
		_equipLabel = GetNode<Label>("InventoryGrid/ItemActionPanel/EquipButton/EquipLabel");
		_sellButton = GetNode<TextureButton>("InventoryGrid/ItemActionPanel/SellButton");
		_closeButton = GetNode<TextureButton>("InventoryGrid/ItemActionPanel/CloseButton");
		_equipButton.Pressed += OnEquipPressed;
		_sellButton.Pressed += OnSellPressed;
		_closeButton.Pressed += OnClosePressed;
		_equipSlots["Helmet"] = GetNode<TextureButton>("EquipmentPanel/HelmetSlot");
		_equipSlots["Armour"] = GetNode<TextureButton>("EquipmentPanel/ArmourSlot");
		_equipSlots["Gloves"] = GetNode<TextureButton>("EquipmentPanel/GlovesSlot");
		_equipSlots["Boots"] = GetNode<TextureButton>("EquipmentPanel/BootsSlot");
		_equipSlots["Weapon"] = GetNode<TextureButton>("EquipmentPanel/WeaponSlot");
		_equipSlots["Talisman"] = GetNode<TextureButton>("EquipmentPanel/TalismanSlot");
		_equipSlots["Necklace"] = GetNode<TextureButton>("EquipmentPanel/NecklaceSlot");
		_equipSlots["Ring1"] = GetNode<TextureButton>("EquipmentPanel/RingSlot1");
		_equipSlots["Ring2"] = GetNode<TextureButton>("EquipmentPanel/RingSlot2");
		foreach(var slot in _equipSlots)
		{
			var slotName = slot.Key;
			slot.Value.Pressed += () =>
			{
				if (_saveManager.CurrentSaveData.EquippedItems.ContainsKey(slotName))
				{
					var item = _saveManager.CurrentSaveData.EquippedItems[slotName];
					OnCardClicked(item.InstanceID);
				}
			};
		}
		RefreshInventory();
		RefreshEquipSlots();
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
	public void RefreshEquipSlots()
	{
		foreach(var slot in _saveManager.CurrentSaveData.EquippedItems)
		{
			if (_equipSlots.ContainsKey(slot.Key))
			{
				var itemTemplate = _itemFactory.GetItemTemplate(slot.Value.TemplateID);
				var talismanTemplate = _itemFactory.GetTalismanTemplate(slot.Value.TemplateID);
				string iconPath = itemTemplate?.IconPath ?? talismanTemplate?.IconPath;
				if (!string.IsNullOrEmpty(iconPath))
				{
					var texture = GD.Load<Texture2D>(iconPath);
					var equipedItem = _equipSlots[slot.Key].GetNode<TextureRect>("Item");
					equipedItem.Texture = texture;
					equipedItem.Visible = true;
				}
			}
		}
	}
	public void OnCardClicked(string instanceID)
	{
		_currentItem = _saveManager.CurrentSaveData.Inventory.Find(i => i.InstanceID == instanceID) 
			?? _saveManager.CurrentSaveData.EquippedItems.Values.FirstOrDefault(i => i.InstanceID == instanceID);
		_inventorySortPanel.Visible = false;
		_itemActionPanel.Visible = true;
		if (_currentItem.IsEquipped)
		{
			_equipLabel.Text = "Unequip";
		}
		else
		{
			_equipLabel.Text = "Equip";
		}
	}
	public void OnEquipPressed()
	{
		if (!_currentItem.IsEquipped)
		{
			_invManager.EquipItem(_currentItem);
		}
		else
		{
			_invManager.UnequipItem(_currentItem);
		}
		RefreshInventory();
		RefreshEquipSlots();
		OnClosePressed();
	}
	public void OnSellPressed()
	{
		_invManager.SellItem(_currentItem);
		RefreshInventory();
		OnClosePressed();
	}
	public void OnClosePressed()
	{
		_currentItem = null;
		_itemActionPanel.Visible = false;
		_inventorySortPanel.Visible = true;
	}
}
