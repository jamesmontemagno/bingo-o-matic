using Microsoft.JSInterop;

namespace BingoApp.Services
{
    /// <summary>
    /// Service to handle JavaScript interop for the navigation bar
    /// </summary>
    public class NavbarService
    {
        private readonly IJSRuntime _jsRuntime;

        public NavbarService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Close the navbar dropdown programmatically
        /// </summary>
        public async Task CloseNavbarAsync()
        {
            try 
            {
                await _jsRuntime.InvokeVoidAsync("navbarHelpers.closeNavbar");
            }
            catch
            {
                // Ignore errors if Bootstrap JS isn't loaded yet
            }
        }

        /// <summary>
        /// Toggle the navbar dropdown programmatically
        /// </summary>
        public async Task ToggleNavbarAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("navbarHelpers.toggleNavbar");
            }
            catch
            {
                // Ignore errors if Bootstrap JS isn't loaded yet
            }
        }
    }
}
