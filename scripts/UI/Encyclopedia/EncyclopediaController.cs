using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class EncyclopediaController : Control
{
    [Export] public Control EnemyList;
    private PackedScene _cardScene;
    private CancellationTokenSource _cts;
    public override void _Ready()
    {
        _cardScene = GD.Load<PackedScene>("res://scences/EncyclopediaItemCard.tscn");
        _ = LoadEntriesAsync();
    }
    private async Task LoadEntriesAsync()
    {
        _cts = new CancellationTokenSource();
        var entries = new System.Collections.Generic.List<EncyclopediaEntry>();
        
        await foreach (var entry in DataStreamReader.StreamEncyclopediaAsync("res://data/Encyclopedia.json", _cts.Token))
        {
            entries.Add(entry);
        }
        var saveManager = GetNode<SaveManager>("/root/SaveManager");
        foreach (var entry in entries)
        {
            entry.IsUnlocked = saveManager.CurrentSaveData.UnlockedEncyclopediaIds.Contains(entry.ID);
            var card = _cardScene.Instantiate<EncyclopediaItemCard>();
            EnemyList.AddChild(card);
            card.Setup(entry);
        }
    }
    public override void _ExitTree()
    {
        _cts?.Cancel();
    }
}