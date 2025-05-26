using BingoApp.Models;
using BingoApp.Helpers;
using System.Text.Json;

namespace BingoApp.Services;

public class BingoSetService : IAsyncDisposable
{
    private readonly List<BingoSet> _sets;
    private readonly IBrowserStorageService _storageService;
    private const string StoreName = "BingoSets";
    private bool _isInitialized = false;
    private bool _IsInitializing = false;
    private SemaphoreSlim _initializationLock = new(1, 1);
    
    public event Action? OnSetsChanged;

    public BingoSetService(IBrowserStorageService storageService)
    {
        _sets = new List<BingoSet>();
        _storageService = storageService;
    }

    public async ValueTask DisposeAsync()
    {
        // Save any pending changes when the service is disposed
        await SaveSetsAsync();
    }

    private async Task InitializeAsync()
    {

        if (_IsInitializing)
            return;

        _IsInitializing = true;

        await _initializationLock.WaitAsync();
        try
        {

            if (_isInitialized)
                return;

            await _storageService.InitializeAsync(StoreName);
            var storedSets = await _storageService.GetAllItemsAsync<BingoSet>();
            
            if (storedSets.Any())
            {
                _sets.Clear();
                _sets.AddRange(storedSets);
            }
            else
            {
                // If no stored data, add mock data
                _sets.AddRange(MockBingoData.GetMockSets().Take(5));
                await SaveSetsAsync();
            }
            
            _isInitialized = true;
        }
        finally
        {
            _IsInitializing = false;
            _initializationLock.Release();
        }
    }
    
    private async Task SaveSetsAsync()
    {
        await InitializeAsync();
        // Convert sets to dictionary with ID as key
        var setsDictionary = _sets.ToDictionary(s => s.Id.ToString(), s => s);
        await _storageService.SetItemsAsync(setsDictionary);
        OnSetsChanged?.Invoke();
    }

    public async Task<IEnumerable<BingoSet>> GetAllAsync()
    {
        await InitializeAsync();
        return _sets;
    }
    
    public IEnumerable<BingoSet> GetAll()
    {
        if (!_isInitialized)
        {
            // This is a synchronous fallback that should be avoided
            // Initialize as empty and then populate asynchronously elsewhere
            return _sets;
        }
        
        return _sets;
    }

    public async Task<BingoSet?> GetByNameAsync(string name)
    {
        await InitializeAsync();
        return _sets.FirstOrDefault(s => s.Name == name);
    }
    
    public BingoSet? GetByName(string name)
    {
        // Synchronous fallback
        if (!_isInitialized)
            return null;
            
        return _sets.FirstOrDefault(s => s.Name == name);
    }

    public async Task<BingoSet?> GetByIdAsync(Guid id)
    {
        await InitializeAsync();
        return _sets.FirstOrDefault(s => s.Id == id);
    }
    
    public BingoSet? GetById(Guid id)
    {
        // Synchronous fallback
        if (!_isInitialized)
            return null;
            
        return _sets.FirstOrDefault(s => s.Id == id);
    }

    public async Task AddSetAsync(BingoSet set)
    {
        await InitializeAsync();
        
        if (string.IsNullOrWhiteSpace(set.Name))
            throw new ArgumentException("Set name cannot be empty");
            
        if (_sets.Any(s => s.Name.Equals(set.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A set with this name already exists");

        // Remove duplicates and empty items
        set = set with
        {
            Id = set.Id == Guid.Empty ? Guid.NewGuid() : set.Id,
            Items = set.Items.Where(i => !string.IsNullOrWhiteSpace(i))
                           .Distinct(StringComparer.OrdinalIgnoreCase)
                           .ToArray()
        };

        if (!set.Items.Any())
            throw new ArgumentException("Set must contain at least one item");

        _sets.Add(set);
        await SaveSetsAsync();
    }
    
    // Keep synchronous version for backward compatibility
    public void AddSet(BingoSet set)
    {
        if (!_isInitialized)
        {
            Task.Run(async () => 
            {
                await InitializeAsync();
                await AddSetAsync(set);
            }).Wait();
            return;
        }
        
        if (string.IsNullOrWhiteSpace(set.Name))
            throw new ArgumentException("Set name cannot be empty");
            
        if (_sets.Any(s => s.Name.Equals(set.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A set with this name already exists");

        // Remove duplicates and empty items
        set = set with
        {
            Id = set.Id == Guid.Empty ? Guid.NewGuid() : set.Id,
            Items = set.Items.Where(i => !string.IsNullOrWhiteSpace(i))
                           .Distinct(StringComparer.OrdinalIgnoreCase)
                           .ToArray()
        };

        if (!set.Items.Any())
            throw new ArgumentException("Set must contain at least one item");

        _sets.Add(set);
        
        // Save asynchronously
        _ = SaveSetsAsync();
    }

    public async Task UpdateSetAsync(Guid id, BingoSet updatedSet)
    {
        await InitializeAsync();
        
        var existingSet = _sets.FirstOrDefault(s => s.Id == id) 
            ?? throw new ArgumentException("Set not found");

        if (existingSet.Name != updatedSet.Name && 
            _sets.Any(s => s.Name.Equals(updatedSet.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A set with this name already exists");

        // Create a new set with the existing ID
        var newSet = new BingoSet
        {
            Id = id,
            Name = updatedSet.Name,
            Items = updatedSet.Items.Where(i => !string.IsNullOrWhiteSpace(i))
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .ToArray()
        };

        if (!newSet.Items.Any())
            throw new ArgumentException("Set must contain at least one item");

        var index = _sets.IndexOf(existingSet);
        _sets[index] = newSet;
        
        await SaveSetsAsync();
    }
    
    // Keep synchronous version for backward compatibility
    public void UpdateSet(Guid id, BingoSet updatedSet)
    {
        if (!_isInitialized)
        {
            Task.Run(async () => 
            {
                await InitializeAsync();
                await UpdateSetAsync(id, updatedSet);
            }).Wait();
            return;
        }
        
        var existingSet = _sets.FirstOrDefault(s => s.Id == id) 
            ?? throw new ArgumentException("Set not found");

        if (existingSet.Name != updatedSet.Name && 
            _sets.Any(s => s.Name.Equals(updatedSet.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A set with this name already exists");

        // Create a new set with the existing ID
        var newSet = new BingoSet
        {
            Id = id,
            Name = updatedSet.Name,
            Items = updatedSet.Items.Where(i => !string.IsNullOrWhiteSpace(i))
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .ToArray()
        };

        if (!newSet.Items.Any())
            throw new ArgumentException("Set must contain at least one item");

        var index = _sets.IndexOf(existingSet);
        _sets[index] = newSet;
        
        // Save asynchronously
        _ = SaveSetsAsync();
    }

    public async Task DeleteSetAsync(Guid id)
    {
        await InitializeAsync();
        
        var set = _sets.FirstOrDefault(s => s.Id == id)
            ?? throw new ArgumentException("Set not found");
        
        _sets.Remove(set);
        await SaveSetsAsync();
    }
    
    // Keep synchronous version for backward compatibility
    public void DeleteSet(Guid id)
    {
        if (!_isInitialized)
        {
            Task.Run(async () => 
            {
                await InitializeAsync();
                await DeleteSetAsync(id);
            }).Wait();
            return;
        }
        
        var set = _sets.FirstOrDefault(s => s.Id == id)
            ?? throw new ArgumentException("Set not found");
        
        _sets.Remove(set);
        
        // Save asynchronously
        _ = SaveSetsAsync();
    }

    // Keep the name-based methods for backward compatibility
    public async Task UpdateSetAsync(string originalName, BingoSet updatedSet)
    {
        var existingSet = await GetByNameAsync(originalName) 
            ?? throw new ArgumentException("Set not found");
        await UpdateSetAsync(existingSet.Id, updatedSet);
    }
    
    public void UpdateSet(string originalName, BingoSet updatedSet)
    {
        var existingSet = GetByName(originalName) 
            ?? throw new ArgumentException("Set not found");
        UpdateSet(existingSet.Id, updatedSet);
    }

    public async Task DeleteSetAsync(string name)
    {
        var set = await GetByNameAsync(name)
            ?? throw new ArgumentException("Set not found");
        await DeleteSetAsync(set.Id);
    }
    
    public void DeleteSet(string name)
    {
        var set = GetByName(name)
            ?? throw new ArgumentException("Set not found");
        DeleteSet(set.Id);
    }

    public static string[] ParseItems(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Array.Empty<string>();

        // Split by newlines, commas, or semicolons
        return input.Split(new[] { '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(s => s.Trim())
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToArray();
    }
}
