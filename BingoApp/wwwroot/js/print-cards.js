async function generatePDF(cards, dotNetHelper) {
    try {
        // Initialize PDF (A4 size)
        const { jsPDF } = window.jspdf;
        const pdf = new jsPDF({
            orientation: 'portrait',
            unit: 'mm',
            format: 'a4'
        });
        
        const pageWidth = pdf.internal.pageSize.width;
        const pageHeight = pdf.internal.pageSize.height;
        const margin = 15; // 15mm margins
        
        // Calculate grid size with more accurate measurements
        const gridSize = pageWidth - (margin * 2);
        const cellSize = gridSize / 5; // Each cell is 1/5 of the grid
        const cellPadding = 2; // 2mm padding inside cells
        const startX = margin;
        const titleY = margin + 10;
        const gridStartY = margin + 30; // Increased to give more space below title
        
        // Theme colors defined in RGB format
        const themeColors = {
            'theme-color-blue': { bg: [230, 242, 255], border: [153, 204, 255] },
            'theme-color-green': { bg: [230, 255, 242], border: [153, 255, 204] },
            'theme-color-purple': { bg: [242, 230, 255], border: [204, 153, 255] },
            'theme-color-orange': { bg: [255, 242, 230], border: [255, 204, 153] },
            'theme-color-pink': { bg: [255, 230, 242], border: [255, 153, 204] },
            'theme-confetti': { bg: [255, 255, 255], border: [255, 215, 0] }, // Gold border for confetti theme
            'none': { bg: [255, 255, 255], border: [0, 0, 0] }
        };
        
        // Function to fetch and cache Unsplash images
        async function getUnsplashImage(theme) {
            const searchTerm = unsplashThemes[theme];
            if (!searchTerm) return null;
            
            try {
                // Use Unsplash Source API for simplicity (no API key required)
                // This returns a random image based on search terms
                const imageUrl = `https://source.unsplash.com/800x600/?${encodeURIComponent(searchTerm)}`;
                
                // Create a promise to load the image
                return new Promise((resolve, reject) => {
                    const img = new Image();
                    img.crossOrigin = 'anonymous'; // Enable CORS
                    img.onload = () => resolve(img);
                    img.onerror = () => reject(new Error('Failed to load image'));
                    img.src = imageUrl;
                });
            } catch (error) {
                console.warn(`Failed to load Unsplash image for theme ${theme}:`, error);
                return null;
            }
        }
        
        // Process each card with progress updates
        for (let cardIndex = 0; cardIndex < cards.length; cardIndex++) {
            const card = cards[cardIndex];
            
            // Update progress
            if (dotNetHelper) {
                await dotNetHelper.invokeMethodAsync('UpdateProgress', 
                    Math.round((cardIndex / cards.length) * 100));
            }
            
            if (cardIndex > 0) {
                pdf.addPage();
            }
            
            // Apply theme background and border
            const theme = card.theme || 'none';
            const themeConfig = themeColors[theme] || themeColors['none'];
            
            // Fill the background with theme color
            pdf.setFillColor(themeConfig.bg[0], themeConfig.bg[1], themeConfig.bg[2]);
            pdf.rect(0, 0, pageWidth, pageHeight, 'F');
            
            // Add a decorative border
            if (theme !== 'none') {
                const borderColor = themeConfig.border;
                pdf.setDrawColor(borderColor[0], borderColor[1], borderColor[2]);
                
                // Draw outer themed border
                pdf.setDrawColor(borderColor[0], borderColor[1], borderColor[2]);
                pdf.setLineWidth(1.5);
                pdf.rect(margin - 5, titleY - 15, gridSize + 10, gridSize + 55, 'D');
                
                // Add some decorations based on the theme
                if (theme.includes('birthday') || theme.includes('party') || theme.includes('confetti')) {
                    // Draw confetti with transparency
                    const decorColors = [
                        [255, 105, 180], // Hot pink
                        [0, 191, 255],   // Deep sky blue
                        [255, 215, 0],   // Gold
                        [50, 205, 50]    // Lime green
                    ];
                    
                    // Set transparency for confetti
                    pdf.setGState(new pdf.GState({opacity: 0.3}));
                    
                    for (let i = 0; i < 20; i++) {
                        const colorIndex = i % decorColors.length;
                        const color = decorColors[colorIndex];
                        const x = Math.random() * (pageWidth - 10) + 5;
                        const y = Math.random() * (pageHeight - 10) + 5;
                        const size = Math.random() * 5 + 2;
                        
                        pdf.setFillColor(color[0], color[1], color[2]);
                        
                        if (Math.random() > 0.5) {
                            pdf.circle(x, y, size, 'F');
                        } else {
                            pdf.rect(x, y, size, size, 'F');
                        }
                    }
                    
                    // Reset opacity for other elements
                    pdf.setGState(new pdf.GState({opacity: 1.0}));
                }
            }
            
            // Add title and card number
            pdf.setFontSize(24);
            pdf.setFont('helvetica', 'bold');
            pdf.text(card.title, pageWidth / 2, titleY, { align: 'center' });
            
            pdf.setFontSize(16);
            const cardNumber = `Card #${cardIndex + 1}`;
            pdf.text(cardNumber, pageWidth / 2, titleY + 10, { align: 'center' });
            
            // Draw grid
            pdf.setLineWidth(0.5);
            
            // Draw cells
            pdf.setFont('helvetica', 'normal');
            card.grid.forEach((row, rowIndex) => {
                row.forEach((cell, colIndex) => {
                    const x = startX + (colIndex * cellSize);
                    const y = gridStartY + (rowIndex * cellSize);
                    
                    // Draw cell border
                    pdf.rect(x, y, cellSize, cellSize);
                    
                    // Add text with proper sizing and positioning
                    const text = cell;
                    
                    // Determine appropriate font size based on text length - larger base sizes
                    let fontSize = 16; // Increased base font size
                    
                    // Fill background for FREE space
                    if (rowIndex === 2 && colIndex === 2) {
                        pdf.setFillColor(230, 230, 250); // Light lavender color
                        pdf.rect(x, y, cellSize, cellSize, 'F');
                        pdf.setFillColor(0, 0, 0);
                        pdf.setFont('helvetica', 'bold');
                        // Force larger font size for FREE space
                        fontSize = 20;
                        pdf.setFontSize(fontSize);
                    } else {
                        pdf.setFont('helvetica', 'normal');
                    }
                    if (text.length > 20) fontSize = 12;
                    else if (text.length > 10) fontSize = 14;
                    else if (text.length <= 5) fontSize = 18; // Extra large for short text
                    
                    pdf.setFontSize(fontSize);
                    
                    // Calculate available space in cell
                    const textWidth = cellSize - (cellPadding * 2);
                    
                    // Try to fit text with current font size
                    let lines = pdf.splitTextToSize(text, textWidth);
                    
                    // If text splits into too many lines, reduce font size
                    const maxLines = 4;
                    while (lines.length > maxLines && fontSize > 8) {
                        fontSize -= 1;
                        pdf.setFontSize(fontSize);
                        lines = pdf.splitTextToSize(text, textWidth);
                    }
                    
                    // If text fits on one line, try to make it larger
                    if (lines.length === 1 && text.length > 2) {
                        let testSize = fontSize;
                        let canIncrease = true;
                        
                        // Try increasing until we hit the width limit
                        while (canIncrease && testSize < 24) { // Maximum size cap at 24pt
                            testSize += 1;
                            pdf.setFontSize(testSize);
                            const testLines = pdf.splitTextToSize(text, textWidth);
                            if (testLines.length > 1 || pdf.getStringUnitWidth(text) * testSize / (72/25.4) > textWidth) {
                                canIncrease = false;
                                testSize -= 1;
                            }
                        }
                        
                        // Set the optimized font size
                        fontSize = testSize;
                        pdf.setFontSize(fontSize);
                        lines = pdf.splitTextToSize(text, textWidth);
                    }
                    
                    // Calculate vertical position to center text
                    const lineHeight = fontSize * 0.352778; // Convert pt to mm
                    const totalTextHeight = lines.length * lineHeight;
                    
                    // Center text vertically within cell
                    const cellCenter = y + (cellSize / 2);
                    const textStartY = cellCenter - (totalTextHeight / 2) + (lineHeight / 2);
                    
                    // Draw each line of text centered in the cell
                    lines.forEach((line, lineIndex) => {
                        const yPos = textStartY + (lineIndex * lineHeight);
                        const xPos = x + (cellSize / 2);
                        pdf.text(line, xPos, yPos, { align: 'center' });
                    });
                    
                    // Reset for next cell
                    pdf.setFillColor(0, 0, 0);
                });
            });
        }
        
        // Final progress update
        if (dotNetHelper) {
            await dotNetHelper.invokeMethodAsync('UpdateProgress', 100);
        }
        
        // Save the PDF
        const today = new Date();
        const dateStr = today.toISOString().split('T')[0]; // Format: YYYY-MM-DD
        const cleanTitle = cards[0].title.toLowerCase().replace(/[^a-z0-9]/g, '-');
        pdf.save(`${cleanTitle}-bingo-cards-${dateStr}.pdf`);
        
        return { success: true };
    } catch (error) {
        console.error('Error generating PDF:', error);
        return {
            success: false,
            error: error.message || 'An error occurred while generating the PDF'
        };
    }
}
