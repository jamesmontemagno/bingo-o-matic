namespace BingoApp.Models;

/// <summary>
/// Represents the saved state of a Custom Bingo game
/// </summary>
public class CustomBingoGameState
{
    /// <summary>
    /// List of items that have been called
    /// </summary>
    public List<string> CalledItems { get; set; } = new();
    
    /// <summary>
    /// List of items that are still available to be called
    /// </summary>
    public List<string> AvailableItems { get; set; } = new();
    
    /// <summary>
    /// Name of the selected bingo set
    /// </summary>
    public string? SelectedSetName { get; set; }
    
    /// <summary>
    /// The current item that was last drawn (if any)
    /// </summary>
    public string? CurrentItem { get; set; }
    
    /// <summary>
    /// Timestamp when the game state was saved
    /// </summary>
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}