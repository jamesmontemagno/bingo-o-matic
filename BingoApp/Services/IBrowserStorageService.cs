namespace BingoApp.Services;

public interface IBrowserStorageService
{
    ValueTask InitializeAsync(string storeName);
    ValueTask<T?> GetItemAsync<T>(string key);
    ValueTask<IEnumerable<T>> GetAllItemsAsync<T>();
    ValueTask SetItemAsync<T>(string key, T item);
    ValueTask SetItemsAsync<T>(IDictionary<string, T> items);
    ValueTask RemoveItemAsync(string key);
    ValueTask ClearAsync();
}
