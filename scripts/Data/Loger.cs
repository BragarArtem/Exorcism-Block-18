using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Godot;
using Godot.NativeInterop;

public static class Logger
{
    public enum LogLevel {Info, Debug, Error}
    private static LogLevel _minLevel = LogLevel.Debug;
    private static bool _logToFile = false;
    private static bool _jsonOutput = false;
    private static string _logFilePath = "user://logs.txt";
    private static int _maxLines = 300;
    private static Func<string, LogLevel, string, string> _formatter = (msg, level, time) => $"[{level}] {time} | {msg}";

    public static void Configure(LogLevel minLevel, bool logToFile = false, bool jsonOutput = false)
    {
        _minLevel = minLevel;
        _logToFile = logToFile;
        _jsonOutput = jsonOutput;
    }
    public static void SetFormatter(Func<string, LogLevel, string, string> formatter)
    {
        _formatter = formatter;
    }
    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
        if(level < _minLevel) return;
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string output = _jsonOutput?JsonSerializer.Serialize(new{Level = level.ToString(), timestamp, message}): _formatter(message, level, timestamp);
        GD.Print(output);
        if(_logToFile) WriteToFile(output);
    }
    public static T LogFunc<T>(string funcName, Func<T> func, LogLevel level = LogLevel.Info, params object[] args)
    {
        if(level < _minLevel) return func();
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        Log($"{funcName} called with args: [{string.Join(", ", args)}]", level);
        var stopwatch = Stopwatch.StartNew();
        try
        {
            T result = func();
            stopwatch.Stop();
            Log($"{funcName} returned: {result} | Time: {stopwatch.ElapsedMilliseconds}ms", level);
            return result;
        }catch(Exception ex)
        {
            stopwatch.Stop();
            Log($"{funcName} threw Exception : {ex.Message}", LogLevel.Error);
            throw;
        }
    }
    public static async System.Threading.Tasks.Task<T> LogFuncAsync<T>(string funcName, Func<System.Threading.Tasks.Task<T>> func, LogLevel level = LogLevel.Info, params object[] args)
    {
        if(level < _minLevel) return await func();
        Log($"{funcName} called with args: [{string.Join(", ", args)}]", level);
        var stopwatch = Stopwatch.StartNew();
        try
        {
            T result = await func();
            stopwatch.Stop();
            Log($"{funcName} returned: {result} | Time: {stopwatch.ElapsedMilliseconds}ms", level);
            return result;
        } catch(Exception ex)
        {
            stopwatch.Stop();
            Log($"{funcName} threw exception: {ex.Message}", LogLevel.Error);
            throw;
        }
    }
    private static void WriteToFile(string message)
    {
        var lines = new System.Collections.Generic.List<string>();
        if (Godot.FileAccess.FileExists(_logFilePath))
        {
            using var readFile = Godot.FileAccess.Open(_logFilePath, Godot.FileAccess.ModeFlags.Read);
            if(readFile != null)
            {
                while (!readFile.EofReached())
                {
                    lines.Add(readFile.GetLine());
                }
            }
            lines.Add(message);
            while(lines.Count > _maxLines)
            {
                lines.RemoveAt(0);
            }
            using var writeFile = Godot.FileAccess.Open(_logFilePath, Godot.FileAccess.ModeFlags.Write);
            if(writeFile != null)
            {
                foreach(var line in lines)
                {
                    writeFile.StoreLine(line);
                }
            }
        }
    }
}