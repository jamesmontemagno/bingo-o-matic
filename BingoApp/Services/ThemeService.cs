using System;
using Microsoft.JSInterop;
using BingoApp.Models;

namespace BingoApp.Services
{
    public class ThemeService : IAsyncDisposable
    {
        private readonly ILocalBrowserStorageService _storageService;
        private readonly IJSRuntime _jsRuntime;
        private const string ThemeKey = "theme-preference";
        private const string ThemeModeKey = "theme-mode-preference";
        private IJSObjectReference? _themeModule;
        private DotNetObjectReference<ThemeService>? _dotNetRef;

        public event Action<bool>? ThemeChanged;
        public event Action<ThemeMode>? ThemeModeChanged;

        public ThemeService(ILocalBrowserStorageService storageService, IJSRuntime jsRuntime)
        {
            _storageService = storageService;
            _jsRuntime = jsRuntime;
        }

        public bool IsDarkMode { get; private set; }
        public ThemeMode CurrentThemeMode { get; private set; } = ThemeMode.Light;

        public async Task InitializeThemeAsync()
        {
            // Load the theme module
            _themeModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/themeDetector.js");
            
            // Try to load the new theme mode format first
            var storedThemeMode = await _storageService.GetItemAsync<string>(ThemeModeKey);
            
            if (!string.IsNullOrEmpty(storedThemeMode) && Enum.TryParse<ThemeMode>(storedThemeMode, out var themeMode))
            {
                CurrentThemeMode = themeMode;
            }
            else
            {
                // Fall back to the legacy boolean format for backward compatibility
                var legacyTheme = await _storageService.GetItemAsync<bool?>(ThemeKey);
                if (legacyTheme.HasValue)
                {
                    CurrentThemeMode = legacyTheme.Value ? ThemeMode.Dark : ThemeMode.Light;
                    // Migrate to new format
                    await SetThemeModeAsync(CurrentThemeMode);
                }
                else
                {
                    // Default to light mode as specified in requirements
                    CurrentThemeMode = ThemeMode.Light;
                    await SetThemeModeAsync(CurrentThemeMode);
                }
            }

            await ApplyTheme();
            
            // Set up system theme change listener if in auto mode
            if (CurrentThemeMode == ThemeMode.Auto)
            {
                await SetupSystemThemeListener();
            }
        }

        public async Task SetThemeModeAsync(ThemeMode themeMode)
        {
            var previousMode = CurrentThemeMode;
            CurrentThemeMode = themeMode;
            
            // Save the new theme mode
            await _storageService.SetItemAsync(ThemeModeKey, themeMode.ToString());
            
            // Clean up old listener if switching away from auto
            if (previousMode == ThemeMode.Auto && themeMode != ThemeMode.Auto)
            {
                await CleanupSystemThemeListener();
            }
            
            // Set up new listener if switching to auto
            if (themeMode == ThemeMode.Auto && previousMode != ThemeMode.Auto)
            {
                await SetupSystemThemeListener();
            }
            
            await ApplyTheme();
            ThemeModeChanged?.Invoke(themeMode);
        }

        // Kept for backward compatibility
        public async Task ToggleThemeAsync()
        {
            var newMode = CurrentThemeMode switch
            {
                ThemeMode.Light => ThemeMode.Dark,
                ThemeMode.Dark => ThemeMode.Auto,
                ThemeMode.Auto => ThemeMode.Light,
                _ => ThemeMode.Light
            };
            
            await SetThemeModeAsync(newMode);
        }

        private async Task ApplyTheme()
        {
            bool shouldUseDarkMode;
            
            switch (CurrentThemeMode)
            {
                case ThemeMode.Dark:
                    shouldUseDarkMode = true;
                    break;
                case ThemeMode.Auto:
                    shouldUseDarkMode = _themeModule != null && 
                        await _themeModule.InvokeAsync<bool>("ThemeDetector.prefersDarkMode");
                    break;
                case ThemeMode.Light:
                default:
                    shouldUseDarkMode = false;
                    break;
            }
            
            IsDarkMode = shouldUseDarkMode;
            await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-bs-theme", IsDarkMode ? "dark" : "light");
            ThemeChanged?.Invoke(IsDarkMode);
        }

        private async Task SetupSystemThemeListener()
        {
            if (_themeModule != null)
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                await _themeModule.InvokeVoidAsync("ThemeDetector.addSystemThemeListener", _dotNetRef);
            }
        }

        private async Task CleanupSystemThemeListener()
        {
            if (_themeModule != null && _dotNetRef != null)
            {
                await _themeModule.InvokeVoidAsync("ThemeDetector.removeSystemThemeListener", _dotNetRef);
                _dotNetRef?.Dispose();
                _dotNetRef = null;
            }
        }

        [JSInvokable]
        public async Task OnSystemThemeChanged(bool isDark)
        {
            if (CurrentThemeMode == ThemeMode.Auto)
            {
                await ApplyTheme();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await CleanupSystemThemeListener();
            
            if (_themeModule != null)
            {
                await _themeModule.DisposeAsync();
            }
        }
    }
}