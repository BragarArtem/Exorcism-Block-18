using Godot;
using System;

public partial class DifficultyController : TextureRect
{
	private string _selectedDifficulty = "Cursed";
	public override void _Ready()
	{
		if (HasNode("/root/SaveManager"))
		{
			var saveManager = GetNode<SaveManager>("/root/SaveManager");
			_selectedDifficulty = saveManager.CurrentSaveData.Difficulty;
		}
		SetupBtn("HollowDifficultyButton", "Hollow");
		SetupBtn("CursedDifficultyButton", "Cursed");
		SetupBtn("AbyssalDifficultyButton", "Abyssal");
		SetupBtn("EldritchDifficultyButton", "Eldritch");
		SetupBtn("ForsakenDifficultyButton", "Forsaken");
		var confirmButton = GetNode<TextureButton>("ConfirmDifficultyButton");
		confirmButton.Pressed +=OnConfirmPressed;
		confirmButton.MouseEntered += () => confirmButton.Modulate = new Color(1.2f,1.2f,1.2f);
		confirmButton.MouseExited += () => confirmButton.Modulate = new Color(1f,1f,1f);
		ResetAllButtons();
	}
	private void SetupBtn(string nodeName, string difficulty)
	{
		var btn = GetNode<TextureButton>(nodeName);
		btn.Pressed += () => OnDifficultySelected(difficulty);
		btn.MouseEntered += () => btn.Modulate = new Color(1.2f,1.2f,1.2f);
		btn.MouseExited += () => {btn.Modulate = (_selectedDifficulty == difficulty)? new Color(1f,1f,1f): new Color(0.6f,0.6f,0.6f);};
	}
	private void ResetAllButtons()
	{
		GetNode<TextureButton>("HollowDifficultyButton").Modulate = new Color(0.6f,0.6f,0.6f);
		GetNode<TextureButton>("CursedDifficultyButton").Modulate = new Color(0.6f,0.6f,0.6f);
		GetNode<TextureButton>("AbyssalDifficultyButton").Modulate = new Color(0.6f,0.6f,0.6f);
		GetNode<TextureButton>("EldritchDifficultyButton").Modulate = new Color(0.6f,0.6f,0.6f);
		GetNode<TextureButton>("ForsakenDifficultyButton").Modulate = new Color(0.6f,0.6f,0.6f);
	}
	private void OnDifficultySelected(string difficulty)
	{
		_selectedDifficulty = difficulty;
		ResetAllButtons();
		GetNode<TextureButton>($"{difficulty}DifficultyButton").Modulate = new Color(1f,1f,1f);
	}
	private void OnConfirmPressed()
	{
		DifficultySettings.Apply(_selectedDifficulty);
		if (HasNode("/root/SaveManager"))
		{
			var saveManager = GetNode<SaveManager>("/root/SaveManager");
			saveManager.CurrentSaveData.Difficulty = _selectedDifficulty;
			saveManager.SaveGame(saveManager.CurrentSaveData);
		}
	}
}
