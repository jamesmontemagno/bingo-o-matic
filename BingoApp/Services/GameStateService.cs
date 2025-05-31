using Microsoft.JSInterop;
using BingoApp.Models;

namespace BingoApp.Services;

/// <summary>
/// Service for managing bingo game state persistence across browser sessions
/// </summary>
public class GameStateService : IGameStateService
{
    private readonly ILocalBrowserStorageService _localStorageService;
    private readonly IJSRuntime _jsRuntime;
    
    private const string StandardBingoSaveKey = "standard_bingo_game_state";
    private const string CustomBingoSaveKey = "custom_bingo_game_state";

    public GameStateService(ILocalBrowserStorageService localStorageService, IJSRuntime jsRuntime)
    {
        _localStorageService = localStorageService;
        _jsRuntime = jsRuntime;
    }

    public async Task SaveStandardGameStateAsync(IEnumerable<int> calledNumbers, int? currentNumber)
    {
        try
        {
            var calledNumbersList = calledNumbers.ToList();
            if (calledNumbersList.Count > 0) // Only save if there's a game in progress
            {
                var gameState = new StandardBingoGameState
                {
                    CalledNumbers = calledNumbersList,
                    CurrentNumber = currentNumber,
                    SavedAt = DateTime.UtcNow
                };
                await _localStorageService.SetItemAsync(StandardBingoSaveKey, gameState);
            }
        }
        catch
        {
            // Silently fail if saving doesn't work
        }
    }

    public async Task<StandardBingoGameState?> LoadStandardGameStateAsync()
    {
        try
        {
            return await _localStorageService.GetItemAsync<StandardBingoGameState>(StandardBingoSaveKey);
        }
        catch
        {
            return null;
        }
    }

    public async Task ClearStandardGameStateAsync()
    {
        try
        {
            await _localStorageService.RemoveItemAsync(StandardBingoSaveKey);
        }
        catch
        {
            // Silently fail if clearing doesn't work
        }
    }

    public async Task SaveCustomGameStateAsync(IEnumerable<string> calledItems, IEnumerable<string> availableItems, string? selectedSetName, string? currentItem)
    {
        try
        {
            var calledItemsList = calledItems.ToList();
            if (calledItemsList.Count > 0 && selectedSetName != null) // Only save if there's a game in progress
            {
                var gameState = new CustomBingoGameState
                {
                    CalledItems = calledItemsList,
                    AvailableItems = availableItems.ToList(),
                    SelectedSetName = selectedSetName,
                    CurrentItem = currentItem,
                    SavedAt = DateTime.UtcNow
                };
                await _localStorageService.SetItemAsync(CustomBingoSaveKey, gameState);
            }
        }
        catch
        {
            // Silently fail if saving doesn't work
        }
    }

    public async Task<CustomBingoGameState?> LoadCustomGameStateAsync()
    {
        try
        {
            return await _localStorageService.GetItemAsync<CustomBingoGameState>(CustomBingoSaveKey);
        }
        catch
        {
            return null;
        }
    }

    public async Task ClearCustomGameStateAsync()
    {
        try
        {
            await _localStorageService.RemoveItemAsync(CustomBingoSaveKey);
        }
        catch
        {
            // Silently fail if clearing doesn't work
        }
    }

    public async Task<bool> PromptResumeGameAsync()
    {
        return await _jsRuntime.InvokeAsync<bool>("confirm", 
            "A previous game was found. Would you like to resume where you left off?");
    }
}