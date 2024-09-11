using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;

namespace TaskCardGame.globals;

public partial class Logger : Node
{

    public LogLevel ActiveLogLevel { get; set; } = LogLevel.Debug; // Level that will be written
    
    public void Debug(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        FormatAndLog(message, LogLevel.Debug, filePath, lineNumber);
    }

    public void Info(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        FormatAndLog(message, LogLevel.Info, filePath, lineNumber);
    }
    
    public void Warn(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        FormatAndLog(message, LogLevel.Warn, filePath, lineNumber);
    }
    
    public void Error(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        FormatAndLog(message, LogLevel.Error, filePath, lineNumber);
    }
    
    private void FormatAndLog(string format, LogLevel level, string filePath, int lineNumber)
    {
        if (level < ActiveLogLevel) return;
        
        // Ex: 20:18:35.140 [INFO] ClassName.cs:42 - message
        var className = filePath.Split("\\").Last().Split("/").Last();
        var msg = $"{DateTime.Now:HH:mm:ss.fff} [{level}] {className}:{lineNumber} - {format}";

        switch (level)
        {
            case LogLevel.Debug:
                GD.PrintRich($"[color=DARK_GRAY]{msg}[/color]");
                break;
            case LogLevel.Info:
                GD.Print(msg);
                break;
            case LogLevel.Warn:
                GD.PrintRich($"[color=WHEAT]{msg}[/color]");
                GD.PushWarning(msg);
                break;
            case LogLevel.Error:
                GD.PrintRich($"[color=ORANGE_RED]{msg}[/color]");
                GD.PushError(msg);
                break;
        }
    }
    
    // Singleton logic
    public static Logger Instance { get; private set; }
    public override void _EnterTree()
    {
        if (Instance != null) QueueFree(); // The Singleton is already loaded, kill this instance
        Instance = this;
    }
}

public enum LogLevel
{
    Debug = 0, // Verbose logging for debugging purposes
    Info = 1, // Information about important events
    Warn = 2, // Warnings
    Error = 3 // Errors
}