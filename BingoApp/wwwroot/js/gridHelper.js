// Grid helper functions for dynamic sizing
window.GridHelper = {
    // Get viewport dimensions
    getViewportSize: function() {
        return {
            width: window.innerWidth,
            height: window.innerHeight
        };
    },
    
    // Calculate optimal grid layout based on item count and dimensions
    calculateGrid: function(itemCount, minItemSize) {
        const viewport = this.getViewportSize();
        
        // Use 10% padding around everything as requested
        const paddingPercent = 0.1;
        const availableWidth = viewport.width * (1 - paddingPercent);
        const availableHeight = viewport.height * (1 - paddingPercent);
        
        // Calculate gap as a small percentage of available space
        const gridGap = Math.min(8, Math.max(3, Math.min(availableWidth, availableHeight) * 0.008));
        
        // For very few items, use optimized layouts with larger items
        if (itemCount <= 4) {
            const cols = 2, rows = 2;
            const itemWidth = Math.floor((availableWidth - (gridGap * (cols - 1))) / cols);
            const itemHeight = Math.floor((availableHeight - (gridGap * (rows - 1))) / rows);
            return { columns: cols, rows: rows, itemWidth: itemWidth, itemHeight: itemHeight, itemSize: Math.min(itemWidth, itemHeight) };
        }
        if (itemCount <= 6) {
            const cols = 2, rows = 3;
            const itemWidth = Math.floor((availableWidth - (gridGap * (cols - 1))) / cols);
            const itemHeight = Math.floor((availableHeight - (gridGap * (rows - 1))) / rows);
            return { columns: cols, rows: rows, itemWidth: itemWidth, itemHeight: itemHeight, itemSize: Math.min(itemWidth, itemHeight) };
        }
        if (itemCount <= 9) {
            const cols = 3, rows = 3;
            const itemWidth = Math.floor((availableWidth - (gridGap * (cols - 1))) / cols);
            const itemHeight = Math.floor((availableHeight - (gridGap * (rows - 1))) / rows);
            return { columns: cols, rows: rows, itemWidth: itemWidth, itemHeight: itemHeight, itemSize: Math.min(itemWidth, itemHeight) };
        }
        if (itemCount <= 12) {
            const cols = 3, rows = 4;
            const itemWidth = Math.floor((availableWidth - (gridGap * (cols - 1))) / cols);
            const itemHeight = Math.floor((availableHeight - (gridGap * (rows - 1))) / rows);
            return { columns: cols, rows: rows, itemWidth: itemWidth, itemHeight: itemHeight, itemSize: Math.min(itemWidth, itemHeight) };
        }
        if (itemCount <= 16) {
            const cols = 4, rows = 4;
            const itemWidth = Math.floor((availableWidth - (gridGap * (cols - 1))) / cols);
            const itemHeight = Math.floor((availableHeight - (gridGap * (rows - 1))) / rows);
            return { columns: cols, rows: rows, itemWidth: itemWidth, itemHeight: itemHeight, itemSize: Math.min(itemWidth, itemHeight) };
        }
        
        // Start with fewer columns to make items bigger
        // More aggressive reduction factor to create larger grid items
        let columns = Math.ceil(Math.sqrt(itemCount) * 0.7);
        columns = Math.max(columns, 2); // Ensure at least two columns
        let rows = Math.ceil(itemCount / columns);
        
        // Adjust for screen aspect ratio
        const screenRatio = availableWidth / availableHeight;
        
        // Adapt grid to screen shape, prioritizing larger items
        if (screenRatio > 1.5) {
            // For wide screens, optimize to better use width while keeping items large
            columns = Math.ceil(columns * 1.1);
            rows = Math.ceil(itemCount / columns);
        } else if (screenRatio < 0.8) {
            // For tall screens, use even fewer columns to make items wider
            columns = Math.max(2, Math.floor(columns * 0.65));
            rows = Math.ceil(itemCount / columns);
        }
        
        // Calculate theoretical item sizes - properly accounting for gaps
        let bestItemWidth = Math.floor((availableWidth - (gridGap * (columns - 1))) / columns);
        let bestItemHeight = Math.floor((availableHeight - (gridGap * (rows - 1))) / rows);
        let bestColumns = columns;
        let bestRows = rows;
        
        // Try more options to maximize total item area (not just square size)
        // Test more column variations to find optimal layout
        for (let i = -2; i <= 2; i++) {
            const testColumns = columns + i;
            // Skip invalid configurations
            if (testColumns < 2) continue; // Ensure minimum of 2 columns
            
            const testRows = Math.ceil(itemCount / testColumns);
            
            // Skip configurations with too many rows
            if (testRows > 8) continue; // Lower max rows to ensure larger items
            
            const testItemWidth = Math.floor((availableWidth - (gridGap * (testColumns - 1))) / testColumns);
            const testItemHeight = Math.floor((availableHeight - (gridGap * (testRows - 1))) / testRows);
            
            // Calculate total item area to find the configuration that maximizes space usage
            const testArea = testItemWidth * testItemHeight;
            const currentBestArea = bestItemWidth * bestItemHeight;
            
            // Update if this configuration gives more area per item
            if (testArea > currentBestArea) {
                bestItemWidth = testItemWidth;
                bestItemHeight = testItemHeight;
                bestColumns = testColumns;
                bestRows = testRows;
            }
        }
        
        // Final size calculation
        const result = {
            columns: bestColumns,
            rows: bestRows,
            itemWidth: Math.max(minItemSize, bestItemWidth),
            itemHeight: Math.max(minItemSize, bestItemHeight),
            itemSize: Math.max(minItemSize, Math.min(bestItemWidth, bestItemHeight)) // Keep for backward compatibility
        };
        
        return result;
    },
    
    // Add resize listener to update grid when window size changes
    setupResizeListener: function(dotnetReference) {
        let resizeTimeout;
        
        window.addEventListener('resize', function() {
            // Debounce the resize event
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(function() {
                // Call the .NET method to recalculate grid
                dotnetReference.invokeMethodAsync('HandleWindowResize');
            }, 250);
        });
        
        // Return true to indicate success
        return true;
    }
};
