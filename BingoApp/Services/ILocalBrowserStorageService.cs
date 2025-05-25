using System.Threading.Tasks;

namespace BingoApp.Services;

/// <summary>
/// Interface for browser storage that uses localStorage
/// </summary>
public interface ILocalBrowserStorageService
{
    ValueTask<T?> GetItemAsync<T>(string key);
    ValueTask SetItemAsync<T>(string key, T item);
    ValueTask RemoveItemAsync(string key);
    ValueTask ClearAsync();
}
