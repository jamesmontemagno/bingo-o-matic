namespace BingoApp.Models;

/// <summary>
/// Represents the saved state of a Standard Bingo game
/// </summary>
public class StandardBingoGameState
{
    /// <summary>
    /// List of numbers that have been called
    /// </summary>
    public List<int> CalledNumbers { get; set; } = new();
    
    /// <summary>
    /// The current number that was last drawn (if any)
    /// </summary>
    public int? CurrentNumber { get; set; }
    
    /// <summary>
    /// Timestamp when the game state was saved
    /// </summary>
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}