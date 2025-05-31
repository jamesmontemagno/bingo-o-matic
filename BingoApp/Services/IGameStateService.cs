using BingoApp.Models;

namespace BingoApp.Services;

/// <summary>
/// Service for managing bingo game state persistence across browser sessions
/// </summary>
public interface IGameStateService
{
    /// <summary>
    /// Saves the current Standard Bingo game state to local storage
    /// </summary>
    /// <param name="calledNumbers">List of numbers that have been called</param>
    /// <param name="currentNumber">The current number that was last drawn</param>
    Task SaveStandardGameStateAsync(IEnumerable<int> calledNumbers, int? currentNumber);

    /// <summary>
    /// Loads the saved Standard Bingo game state from local storage
    /// </summary>
    /// <returns>The saved game state, or null if no saved state exists</returns>
    Task<StandardBingoGameState?> LoadStandardGameStateAsync();

    /// <summary>
    /// Clears the saved Standard Bingo game state from local storage
    /// </summary>
    Task ClearStandardGameStateAsync();

    /// <summary>
    /// Saves the current Custom Bingo game state to local storage
    /// </summary>
    /// <param name="calledItems">List of items that have been called</param>
    /// <param name="availableItems">List of items that are still available</param>
    /// <param name="selectedSetName">Name of the selected bingo set</param>
    /// <param name="currentItem">The current item that was last drawn</param>
    Task SaveCustomGameStateAsync(IEnumerable<string> calledItems, IEnumerable<string> availableItems, string? selectedSetName, string? currentItem);

    /// <summary>
    /// Loads the saved Custom Bingo game state from local storage
    /// </summary>
    /// <returns>The saved game state, or null if no saved state exists</returns>
    Task<CustomBingoGameState?> LoadCustomGameStateAsync();

    /// <summary>
    /// Clears the saved Custom Bingo game state from local storage
    /// </summary>
    Task ClearCustomGameStateAsync();

    /// <summary>
    /// Prompts the user to resume a previous game
    /// </summary>
    /// <returns>True if the user wants to resume, false otherwise</returns>
    Task<bool> PromptResumeGameAsync();
}