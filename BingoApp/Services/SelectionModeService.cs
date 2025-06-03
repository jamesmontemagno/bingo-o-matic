using BingoApp.Models;

namespace BingoApp.Services
{
    public class SelectionModeService
    {
        private const string STORAGE_KEY = "item_selection_mode";
        private readonly ILocalBrowserStorageService _storage;
        
        public event Action<ItemSelectionMode>? SelectionModeChanged;
        private ItemSelectionMode _currentMode = ItemSelectionMode.Random;
        public bool IsInitialized { get; private set; }

        public SelectionModeService(ILocalBrowserStorageService storage)
        {
            _storage = storage;
        }

        public async Task InitializeAsync()
        {
            if (IsInitialized)
                return;

            try
            {
                var savedMode = await _storage.GetItemAsync<string>(STORAGE_KEY);
                if (Enum.TryParse<ItemSelectionMode>(savedMode, out var mode))
                {
                    _currentMode = mode;
                }
            }
            catch
            {
                // Fallback to default if loading fails
                _currentMode = ItemSelectionMode.Random;
            }
            finally
            {
                IsInitialized = true;
            }
        }

        public async Task<ItemSelectionMode> GetSelectionModeAsync()
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }
            return _currentMode;
        }

        public async Task SetSelectionModeAsync(ItemSelectionMode mode)
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }

            if (_currentMode != mode)
            {
                _currentMode = mode;
                await _storage.SetItemAsync(STORAGE_KEY, mode.ToString());
                SelectionModeChanged?.Invoke(mode);
            }
        }
    }
}
