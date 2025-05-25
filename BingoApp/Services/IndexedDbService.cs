using System.Text.Json;
using Microsoft.JSInterop;

namespace BingoApp.Services;

public class IndexedDbService : IBrowserStorageService, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;
    private const string DatabaseName = "BingoData";
    private const string StoreName = "BingoStore";
    private const int DatabaseVersion = 1;
    private bool _initialized = false;

    public IndexedDbService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private async ValueTask InitializeAsync()
    {
        if (_initialized && _module is not null)
            return;

        // Load the IndexedDB helper JavaScript module
        _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./js/indexedDbInterop.js");

        // Initialize the database
        await _module.InvokeVoidAsync("initDb", DatabaseName, StoreName, DatabaseVersion);

        _initialized = true;
    }

    public async ValueTask<T?> GetItemAsync<T>(string key)
    {
        await InitializeAsync();

        if (_module is null)
            return default;

        var json = await _module.InvokeAsync<string?>(
            "getItem", DatabaseName, StoreName, key);

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

    public async ValueTask SetItemAsync<T>(string key, T item)
    {
        await InitializeAsync();

        if (_module is null)
            return;

        if (item == null)
        {
            await RemoveItemAsync(key);
            return;
        }

        var json = JsonSerializer.Serialize(item);
        await _module.InvokeVoidAsync(
            "setItem", DatabaseName, StoreName, key, json);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await InitializeAsync();

        if (_module is null)
            return;

        await _module.InvokeVoidAsync(
            "removeItem", DatabaseName, StoreName, key);
    }

    public async ValueTask ClearAsync()
    {
        await InitializeAsync();

        if (_module is null)
            return;

        await _module.InvokeVoidAsync(
            "clearStore", DatabaseName, StoreName);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}
