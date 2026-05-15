using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
public partial class InventoryManager : Node
{
    private Dictionary<string, BaseItemInstance> _equippedSlots = new Dictionary<string, BaseItemInstance>();
    private SaveManager _saveManager;
    private ItemFactory _itemFactory;
    public override void _Ready()
    {
        _saveManager = GetNode<SaveManager>("/root/SaveManager");
        _itemFactory = GetNode<ItemFactory>("/root/ItemFactory");
    
    }
    public void AddItem(BaseItemInstance item)
    {
        _saveManager.CurrentSaveData.Inventory.Add(item);
    }
    public bool DeleteItem(BaseItemInstance item)
    {
        return _saveManager.CurrentSaveData.Inventory.Remove(item);
    }
    public bool EquipItem(BaseItemInstance item)
    {
        var itemTemplate = _itemFactory.GetItemTemplate(item.TemplateID);
        var talismanTemplate = _itemFactory.GetTalismanTemplate(item.TemplateID);
        string slot = itemTemplate?.Slot ?? talismanTemplate?.Slot;
        if (slot != null)
        {
            if (_equippedSlots.ContainsKey(slot))
            {
                var oldItem = _equippedSlots[slot];
                AddItem(oldItem);
                oldItem.IsEquipped = false;
            }
            _equippedSlots[slot] = item;
            _saveManager.CurrentSaveData.EquippedItems[slot] = item;
            item.IsEquipped = true;
            DeleteItem(item);
            return true;
        }
        return false;
    }
    public bool UnequipItem(BaseItemInstance item)
    {
        var itemTemplate = _itemFactory.GetItemTemplate(item.TemplateID);
        var talismanTemplate = _itemFactory.GetTalismanTemplate(item.TemplateID);
        string slot = itemTemplate?.Slot ?? talismanTemplate?.Slot;
        if (slot != null)
        {
            _equippedSlots.Remove(slot);
            _saveManager.CurrentSaveData.EquippedItems.Remove(slot);
            item.IsEquipped = false;
            AddItem(item);
            return true;
        }
        return false;
    } 
    public decimal SellItem(BaseItemInstance item)
    {
        decimal _itemPrice = item.Price;
        DeleteItem(item);
        _saveManager.CurrentSaveData.Gold += _itemPrice;
        return _itemPrice;
    } 
}