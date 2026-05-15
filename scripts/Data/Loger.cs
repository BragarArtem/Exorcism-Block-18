using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Godot;

public static class Logger
{
    public enum LogLevel {Info, Debug, Error}
    private static LogLevel _minLevel = LogLevel.Debug;
    private static bool _logToFile = false;
    private static bool _jsonOutput = false;
    private static string _logFilePath = "user://logs.txt";

    public static void Configure(LogLevel minLevel, bool logToFile = false, bool jsonOutput = false)
    {
        _minLevel = minLevel;
        _logToFile = logToFile;
        _jsonOutput = jsonOutput;
    }
    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
        if(level < _minLevel) return;
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string output = _jsonOutput?JsonSerializer.Serialize(new{Level = level.ToString(), timestamp, message}): $"[{level}] {timestamp} | {message}";
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
        var mode = Godot.FileAccess.FileExists(_logFilePath)? Godot.FileAccess.ModeFlags.ReadWrite : Godot.FileAccess.ModeFlags.Write;
        using var file = Godot.FileAccess.Open(_logFilePath, Godot.FileAccess.ModeFlags.ReadWrite);
        if(file != null)
        {
            file.SeekEnd(0);
            file.StoreLine(message);
        }
    }
}