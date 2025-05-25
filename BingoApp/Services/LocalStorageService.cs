using System.Text.Json;
using Microsoft.JSInterop;

namespace BingoApp.Services;

public class LocalStorageService : ILocalBrowserStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageType = "localStorage";

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<T?> GetItemAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string?>(
            $"{StorageType}.getItem",
            key);

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
        if (item == null)
        {
            await RemoveItemAsync(key);
            return;
        }

        var json = JsonSerializer.Serialize(item);
        await _jsRuntime.InvokeVoidAsync(
            $"{StorageType}.setItem",
            key, json);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync(
            $"{StorageType}.removeItem",
            key);
    }

    public async ValueTask ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync(
            $"{StorageType}.clear");
    }
}
