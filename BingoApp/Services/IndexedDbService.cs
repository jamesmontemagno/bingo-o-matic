using System.Text.Json;
using Microsoft.JSInterop;

namespace BingoApp.Services;

public class IndexedDbService : IBrowserStorageService, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;
    private const string DatabaseName = "BingoData";
    private const int InitialDatabaseVersion = 1;
    private bool _initialized = false;
    private SemaphoreSlim _initializationLock = new(1, 1);
    private string? _storeName;

    public IndexedDbService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask InitializeAsync(string storeName)
    {
        if (string.IsNullOrEmpty(storeName))
            throw new ArgumentException("Store name cannot be empty", nameof(storeName));

        await _initializationLock.WaitAsync();
        try
        {
            _storeName = storeName;
            await EnsureInitializedAsync();
        }
        catch (Exception)
        {
            _initialized = false;
            _module = null;
            throw;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    private async ValueTask EnsureInitializedAsync()
    {
        if (!_initialized || _module is null || string.IsNullOrEmpty(_storeName))
        {
            if (string.IsNullOrEmpty(_storeName))
                throw new InvalidOperationException("StoreName must be set before calling any operations. Call InitializeAsync first.");

            try
            {
                // Load the IndexedDB helper JavaScript module
                _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", "./js/indexedDbInterop.js");

                // Initialize the database with initial version
                await _module.InvokeVoidAsync("initDb", DatabaseName, _storeName, InitialDatabaseVersion);

                _initialized = true;
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"IndexedDB initialization error: {ex.Message}");
                throw;
            }
        }
    }

    public async ValueTask<T?> GetItemAsync<T>(string key)
    {
        await EnsureInitializedAsync();

        if (_module is null || string.IsNullOrEmpty(_storeName))
            return default;

        var json = await _module.InvokeAsync<string?>(
            "getItem", DatabaseName, _storeName, key);

        if (string.IsNullOrEmpty(json))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            // If deserialization fails, return default value
            return default;
        }
    }

    public async ValueTask<IEnumerable<T>> GetAllItemsAsync<T>()
    {
        await EnsureInitializedAsync();

        if (_module is null || string.IsNullOrEmpty(_storeName))
            return Array.Empty<T>();

        var json = await _module.InvokeAsync<string?>(
            "getAllItems", DatabaseName, _storeName);

        if (string.IsNullOrEmpty(json))
            return Array.Empty<T>();

        try
        {
            var items = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (items == null)
                return Array.Empty<T>();

            return items.Values
                .Select(v => JsonSerializer.Deserialize<T>(v))
                .Where(item => item != null)
                .Cast<T>();
        }
        catch
        {
            // If deserialization fails, return empty array
            return Array.Empty<T>();
        }
    }

    public async ValueTask SetItemAsync<T>(string key, T item)
    {
        await EnsureInitializedAsync();

        if (_module is null || string.IsNullOrEmpty(_storeName))
            return;

        if (item == null)
        {
            await RemoveItemAsync(key);
            return;
        }

        var json = JsonSerializer.Serialize(item);
        await _module.InvokeVoidAsync(
            "setItem", DatabaseName, _storeName, key, json);
    }

    public async ValueTask SetItemsAsync<T>(IDictionary<string, T> items)
    {
        await EnsureInitializedAsync();

        if (_module is null || string.IsNullOrEmpty(_storeName))
            return;

        var serializedItems = items.ToDictionary(
            kvp => kvp.Key,
            kvp => JsonSerializer.Serialize(kvp.Value));

        await _module.InvokeVoidAsync(
            "setItems", DatabaseName, _storeName, serializedItems);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await EnsureInitializedAsync();

        if (_module is null || string.IsNullOrEmpty(_storeName))
            return;

        await _module.InvokeVoidAsync(
            "removeItem", DatabaseName, _storeName, key);
    }

    public async ValueTask ClearAsync()
    {
        await EnsureInitializedAsync();

        if (_module is null || string.IsNullOrEmpty(_storeName))
            return;

        await _module.InvokeVoidAsync(
            "clearStore", DatabaseName, _storeName);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}
