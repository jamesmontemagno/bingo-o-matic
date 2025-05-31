namespace BingoApp.Models;

/// <summary>
/// Represents the available theme modes for the application
/// </summary>
public enum ThemeMode
{
    /// <summary>
    /// Light theme mode
    /// </summary>
    Light = 0,
    
    /// <summary>
    /// Dark theme mode
    /// </summary>
    Dark = 1,
    
    /// <summary>
    /// Auto theme mode that follows system preference
    /// </summary>
    Auto = 2
}