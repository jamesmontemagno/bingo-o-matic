// Dynamic JavaScript loader helper for Blazor WebAssembly
// Handles proper loading and initialization of external libraries

class DynamicLoader {
    constructor() {
        this.loadedScripts = new Map();
        this.loadPromises = new Map();
    }

    /**
     * Dynamically loads a script and ensures it's properly initialized
     * @param {string} src - The script source URL
     * @param {Function} verifyLoaded - Optional function to verify the script loaded correctly
     * @returns {Promise} Promise that resolves when script is loaded and verified
     */
    async loadScript(src, verifyLoaded = null) {
        // If already loaded, return immediately
        if (this.loadedScripts.has(src)) {
            return Promise.resolve();
        }

        // If currently loading, return the existing promise
        if (this.loadPromises.has(src)) {
            return this.loadPromises.get(src);
        }

        // Create new loading promise
        const loadPromise = new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = src;
            script.type = 'text/javascript';
            
            script.onload = () => {
                // If verification function provided, use it
                if (verifyLoaded && !verifyLoaded()) {
                    reject(new Error(`Script loaded but verification failed: ${src}`));
                    return;
                }
                
                this.loadedScripts.set(src, true);
                this.loadPromises.delete(src);
                resolve();
            };
            
            script.onerror = () => {
                this.loadPromises.delete(src);
                reject(new Error(`Failed to load script: ${src}`));
            };
            
            document.head.appendChild(script);
        });

        this.loadPromises.set(src, loadPromise);
        return loadPromise;
    }

    /**
     * Loads jsPDF library and ensures it's available on window.jspdf
     * @returns {Promise} Promise that resolves when jsPDF is ready
     */
    async loadJsPDF() {
        await this.loadScript('./js/jspdf.umd.min.js', () => {
            // Verify jsPDF is available
            return window.jspdf && typeof window.jspdf.jsPDF === 'function';
        });
        return window.jspdf;
    }

    /**
     * Loads confetti library and ensures it's available
     * @returns {Promise} Promise that resolves when confetti is ready
     */
    async loadConfetti() {
        await this.loadScript('./js/confetti.browser.js', () => {
            // Verify confetti is available globally
            return typeof window.confetti === 'function';
        });
        return window.confetti;
    }

    /**
     * Loads print-cards.js after ensuring jsPDF is available
     * @returns {Promise} Promise that resolves when print-cards is ready
     */
    async loadPrintCards() {
        // First ensure jsPDF is loaded
        await this.loadJsPDF();
        
        // Then load print-cards.js
        await this.loadScript('./js/print-cards.js', () => {
            // Verify generatePDF function is available
            return typeof window.generatePDF === 'function';
        });
    }

    /**
     * Safely loads multiple scripts in sequence
     * @param {Array} scripts - Array of {src, verify} objects
     * @returns {Promise} Promise that resolves when all scripts are loaded
     */
    async loadScriptsSequentially(scripts) {
        for (const script of scripts) {
            await this.loadScript(script.src, script.verify);
        }
    }

    /**
     * Checks if a script is already loaded
     * @param {string} src - The script source URL
     * @returns {boolean} True if the script is loaded
     */
    isLoaded(src) {
        return this.loadedScripts.has(src);
    }

    /**
     * Clears the loaded scripts cache (useful for testing)
     */
    clearCache() {
        this.loadedScripts.clear();
        this.loadPromises.clear();
    }
}

// Create global instance
window.dynamicLoader = new DynamicLoader();

// Helper functions for common scenarios
window.loadJsPDFForPrinting = async function() {
    try {
        await window.dynamicLoader.loadPrintCards();
        return JSON.parse(JSON.stringify({ success: true, error: null }));
    } catch (error) {
        console.error('Failed to load PDF libraries:', error);
        return JSON.parse(JSON.stringify({ success: false, error: error.message }));
    }
};

window.loadConfettiForAnimation = async function() {
    try {
        await window.dynamicLoader.loadConfetti();
        return JSON.parse(JSON.stringify({ success: true, error: null }));
    } catch (error) {
        console.error('Failed to load confetti library:', error);
        return JSON.parse(JSON.stringify({ success: false, error: error.message }));
    }
};

// For debugging purposes
window.dynamicLoaderDebug = {
    getLoadedScripts: () => Array.from(window.dynamicLoader.loadedScripts.keys()),
    getPendingLoads: () => Array.from(window.dynamicLoader.loadPromises.keys()),
    clearCache: () => window.dynamicLoader.clearCache()
};
