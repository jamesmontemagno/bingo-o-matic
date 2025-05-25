using System;
using Microsoft.JSInterop;

namespace BingoApp.Services
{
    public class ThemeService
    {
        private readonly IBrowserStorageService _storageService;
        private readonly IJSRuntime _jsRuntime;
        private const string ThemeKey = "theme-preference";

        public event Action<bool>? ThemeChanged;

        public ThemeService(IBrowserStorageService storageService, IJSRuntime jsRuntime)
        {
            _storageService = storageService;
            _jsRuntime = jsRuntime;
        }

        public bool IsDarkMode { get; private set; }

        public async Task InitializeThemeAsync()
        {
            var storedTheme = await _storageService.GetItemAsync<bool>(ThemeKey);
            IsDarkMode = storedTheme;
            await ApplyTheme();
        }

        public async Task ToggleThemeAsync()
        {
            IsDarkMode = !IsDarkMode;
            await _storageService.SetItemAsync(ThemeKey, IsDarkMode);
            await ApplyTheme();
        }

        private async Task ApplyTheme()
        {
            await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-bs-theme", IsDarkMode ? "dark" : "light");
            ThemeChanged?.Invoke(IsDarkMode);
        }
    }
}