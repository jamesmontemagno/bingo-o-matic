namespace BingoApp.Services;

public interface IBrowserStorageService
{
    ValueTask<T?> GetItemAsync<T>(string key);
    ValueTask SetItemAsync<T>(string key, T item);
    ValueTask RemoveItemAsync(string key);
    ValueTask ClearAsync();
}
