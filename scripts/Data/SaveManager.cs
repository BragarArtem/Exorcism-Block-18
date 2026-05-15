using Godot;
using System;
using System.Text.Json;

public partial class SaveManager : Node
{
	private const string SaveFilePath = "user://SaveData.json";
	public SaveData CurrentSaveData { get; private set; } = new SaveData();
	public override void _Ready()
	{
		Logger.Configure(Logger.LogLevel.Debug, logToFile: true);
		CurrentSaveData = LoadGame();
		DifficultySettings.Apply(CurrentSaveData.Difficulty);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("save_game"))
		{
			SaveGame(CurrentSaveData);
		}
	}
	public void SaveGame(SaveData data)
	{
		try
		{
			var options  = new JsonSerializerOptions { WriteIndented = true };
			string json = JsonSerializer.Serialize(data, options);
			
			using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
			if (file != null)
			{
				file.StoreString(json);
				Logger.Log("Game saved successfully", Logger.LogLevel.Info);
			}

			else
				{
					Logger.Log($"Failed to open save file for writing : {FileAccess.GetOpenError()}", Logger.LogLevel.Error);
				}
			}
			
		catch (Exception ex)
		{
			Logger.Log("Failed to save game: " + ex.Message, Logger.LogLevel.Error);
		}
	}

	public SaveData LoadGame()
	{
		try
		{
			
			
			if (!FileAccess.FileExists(SaveFilePath))
			{
				Logger.Log("No save file found", Logger.LogLevel.Debug);
				return new SaveData();
			}
			using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
			if (file == null)
			{
				Logger.Log($"Failed to open save file for reading: {FileAccess.GetOpenError()}", Logger.LogLevel.Error);
				return new SaveData();

			}
			string json = file.GetAsText();
			SaveData data = JsonSerializer.Deserialize<SaveData>(json);

			if (data == null)
			{
			
				Logger.Log($"Loaded data is null. Returning default save data.", Logger.LogLevel.Error);
				return new SaveData();
			}
			
			Logger.Log("Game loaded successfully", Logger.LogLevel.Info);
			return data;
		}
		catch (Exception ex)
		{
			Logger.Log("Failed to load the game: " + ex.Message, Logger.LogLevel.Error);
			return new SaveData();
		}
	}
}
