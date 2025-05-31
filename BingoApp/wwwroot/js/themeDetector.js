// Theme detection utilities
export const ThemeDetector = {
    _listeners: new Map(),

    // Check if the system prefers dark mode
    prefersDarkMode: () => {
        return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    },

    // Add listener for system theme changes
    addSystemThemeListener: (dotNetRef) => {
        if (window.matchMedia) {
            const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
            const handler = (e) => {
                dotNetRef.invokeMethodAsync('OnSystemThemeChanged', e.matches);
            };
            mediaQuery.addEventListener('change', handler);
            ThemeDetector._listeners.set(dotNetRef, { mediaQuery, handler });
        }
    },

    // Remove listener for system theme changes
    removeSystemThemeListener: (dotNetRef) => {
        const listenerInfo = ThemeDetector._listeners.get(dotNetRef);
        if (listenerInfo) {
            listenerInfo.mediaQuery.removeEventListener('change', listenerInfo.handler);
            ThemeDetector._listeners.delete(dotNetRef);
        }
    }
};