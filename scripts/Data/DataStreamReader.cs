using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;

public static class DataStreamReader
{
    public static async IAsyncEnumerable<EncyclopediaEntry> StreamEncyclopediaAsync(string resPath, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string absolutePath = ProjectSettings.GlobalizePath(resPath);
        using var fileStream = new System.IO.FileStream(absolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        using var streamReader = new StreamReader(fileStream);
        string line;
        var buffer = new System.Text.StringBuilder();
        int braceDepth = 0;
        bool insideEntry = false;

        while((line = await streamReader.ReadLineAsync()) != null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach(char c in line)
            {
                if (c == '{')
                {
                    braceDepth ++; 
                    if(braceDepth == 1)
                    {
                        insideEntry = true;
                    }
                
                }
                if (c == '}')
                {
                    braceDepth--;
                }

            }
            if (insideEntry)
            {
                buffer.AppendLine(line);
            }
            if (insideEntry && braceDepth == 0)
            {
                string json = buffer.ToString().Trim().TrimEnd(',');
                EncyclopediaEntry entry = null;
                try
                {
                  entry = JsonSerializer.Deserialize<EncyclopediaEntry>(json);

                } catch(Exception e)
                {
                    GD.PrintErr($"Parse error: {e.Message}");
                }
                if (entry != null)
                {
                    yield return entry;
                }  
                buffer.Clear();
                insideEntry = false;
            }
        }
    }
}
