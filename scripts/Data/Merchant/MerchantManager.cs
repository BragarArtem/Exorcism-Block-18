using System;
using Godot;
public partial class MerchantManager : Node
{
    private SaveManager _saveManager;
    private ItemFactory _itemFactory;

    public override void _Ready()
    {
        _saveManager = GetNode<SaveManager>("/root/SaveManager");
        _itemFactory = GetNode<ItemFactory>("/root/ItemFactory");
        if(_saveManager.CurrentSaveData.MerchantStock.Count == 0)
        {
            GenerateStock();
        }
    }
    private void GenerateStock()
    {
        _saveManager.CurrentSaveData.MerchantStock.Clear();
        for(int i = 0; i < 4; i++)
        {
            var item = _itemFactory.CreateItem("short_sword_t1");
            _saveManager.CurrentSaveData.MerchantStock.Add(item);
        }
        _saveManager.SaveGame(_saveManager.CurrentSaveData);
    }
}