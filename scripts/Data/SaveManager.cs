using Godot;
using System;
using System.Text.Json;

public partial class SaveManager : Node
{
    private const string SaveFilePath = "user://SaveData.json";
    public SaveData CurrentSaveData { get; private set; } = new SaveData();
    public override void _Ready()
    {
        CurrentSaveData = LoadGame();
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
                GD.Print("Game saved successfully.");
            }

            else
                {
                    GD.PrintErr ($"Failed to open save file for writing : {FileAccess.GetOpenError()}");
                }
            }
            
        catch (Exception ex)
        {
            GD.PrintErr("Failed to save game: " + ex.Message);
        }
    }

    public SaveData LoadGame()
    {
        try
        {
            
            
            if (!FileAccess.FileExists(SaveFilePath))
            {
                GD.Print("No save file found. Returning default save data.");
                return new SaveData();
            }
            using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PrintErr($"Failed to open save file for reading: {FileAccess.GetOpenError()}. Returning default save data.");
                return new SaveData();

            }
            string json = file.GetAsText();
            SaveData data = JsonSerializer.Deserialize<SaveData>(json);

            if (data == null)
            {
            
            GD.PrintErr($"Loaded data is null. Returning default save data.");
                return new SaveData();
            }
            
            GD.Print("Game loaded successfully.");
            return data;
        }
        catch (Exception ex)
        {
            GD.PrintErr("Failed to load game: " + ex.Message);
            return new SaveData();
        }
    }
}


