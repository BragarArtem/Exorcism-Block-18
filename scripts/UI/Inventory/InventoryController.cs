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
	private string[] _merchantIdlePhrases =
	{
		"Adventurer, dont put too much faith in the scraps you pull from dead creatures. I've got cleaner steel, warmer armour and boots, that won't fall appart after second clash",
		"Still alive? Huh. Then fortune hasn't abandoned you just yet. Come take a look - fresh blades, sturdy leather, and a few things worth carrying through this cursed land",
		"You've seen enough rotten gear out there already. Stop by the wagon, warm your hands, and i'll show you something better than grave-picked iron",
		"Come closer, traveler. I've got proper steel, thick leather, and supplies worth carrying - and if you found something unusual out there, i'll tell you whethere it's treasure or scrap",
		"You won't survive long wearing graveyard junk. Take a look through my stock, and lay your finds on the table too - these old eyes still know the worth of good gear"
	};
	private string _merchantIdlePhrase;
	private Label _infoLabel;
	public enum SortType {ByType, ByValue, ByStrenght}
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
		_infoLabel = GetNode<Label>("MerchantDialogue/InfoLabel");
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
		GetNode<TextureButton>("InventoryGrid/InventorySortPanel/SortByTypeButton").Pressed += () => RefreshInventory(SortType.ByType);
		GetNode<TextureButton>("InventoryGrid/InventorySortPanel/SortByValueButton").Pressed += () => RefreshInventory(SortType.ByValue);
		GetNode<TextureButton>("InventoryGrid/InventorySortPanel/SortByStrenghtButton").Pressed += () => RefreshInventory(SortType.ByStrenght);
		GetNode<TextureButton>("BackToMenuButton").Pressed += () => OnBackPressed();
		GetNode<TextureButton>("ToMerchantScene/ToMerchantButton").Pressed += () => ToMerchantPressed();

		_merchantIdlePhrase = _merchantIdlePhrases[new Random().Next(_merchantIdlePhrases.Length)];
		_infoLabel.Text = _merchantIdlePhrase;
		
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
		RefreshInventory(SortType.ByType);
		RefreshEquipSlots();
	}
	public void RefreshInventory(SortType sort = SortType.ByType)
	{
		while(_gridCont.GetChildCount() > 0)
		{
			_gridCont.GetChild(0).Free();
		}
		var sortedInventory = sort switch
		{
			SortType.ByType => _saveManager.CurrentSaveData.Inventory.OrderBy(i => _itemFactory.GetItemTemplate(i.TemplateID)?.Category).ToList(),
			SortType.ByValue => _saveManager.CurrentSaveData.Inventory.OrderByDescending(i => i.Price).ToList(),
			SortType.ByStrenght =>	_saveManager.CurrentSaveData.Inventory.OrderByDescending(i => i.GetStat("damage")).ToList(), //would need to change i.GetStat("damage") to the strenght param. of Item in future. I didnt write it down, cause i dont have one, its hard todo it rn, without having full item.stat list;
			_ => _saveManager.CurrentSaveData.Inventory
		};
		foreach(var item in sortedInventory)
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
		foreach(var slot in _equipSlots)
		{
			slot.Value.GetNode<TextureRect>("Item").Visible = false;	
		}
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
		_infoLabel.Text = _currentItem.Description;
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
		_saveManager.SaveGame(_saveManager.CurrentSaveData);
		RefreshInventory();
		RefreshEquipSlots();
		OnClosePressed();
	}
	public void OnSellPressed()
	{

		_invManager.SellItem(_currentItem);
		RefreshInventory();
		OnClosePressed();
		_saveManager.SaveGame(_saveManager.CurrentSaveData);
	}
	public void OnClosePressed()
	{
		_currentItem = null;
		_itemActionPanel.Visible = false;
		_inventorySortPanel.Visible = true;
		_infoLabel.Text = _merchantIdlePhrase;
	}
	public void OnBackPressed()
	{
		TransitionManager.GoTo("res://scences/MainMenu.tscn");
	}
	public void ToMerchantPressed()
	{
		TransitionManager.GoTo("res://scences/Merchant.tscn");
	}
}