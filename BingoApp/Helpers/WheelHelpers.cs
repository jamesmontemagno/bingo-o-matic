using System;
using System.Linq;
using System.Collections.Generic;

namespace BingoApp.Helpers
{
    public static class WheelHelpers
    {
        private static readonly string[] wheelColors = new[]
        {
            "#E74C3C", // Red
            "#3498DB", // Blue  
            "#2ECC71", // Green
            "#F39C12", // Orange
            "#9B59B6", // Purple
            "#1ABC9C", // Teal
            "#E67E22", // Carrot Orange
            "#8E44AD", // Wisteria Purple
            "#16A085", // Dark Turquoise
            "#C0392B"  // Dark Red
        };

        public static string GetWheelSegmentColor(int index, int totalSegments)
        {
            // For better visual separation, ensure adjacent segments have different colors
            // Also ensure first and last segments never match when there are enough segments
            
            // If we have fewer segments than colors, space them out
            if (totalSegments <= wheelColors.Length)
            {
                // Calculate spacing to distribute colors evenly
                var spacing = Math.Max(1, wheelColors.Length / totalSegments);
                var colorIndex = (index * spacing) % wheelColors.Length;
                
                // Special handling to ensure first and last segments don't match when possible
                if (totalSegments > 2 && index == totalSegments - 1) // Last segment
                {
                    var firstSegmentColorIndex = 0; // First segment always uses index 0 with spacing
                    if (colorIndex == firstSegmentColorIndex)
                    {
                        // Move to next available color that's different from first segment
                        colorIndex = (colorIndex + spacing) % wheelColors.Length;
                    }
                }
                
                return wheelColors[colorIndex];
            }
            else
            {
                // More segments than colors - use alternating pattern to avoid adjacency
                // This creates a pattern like: 0,2,4,1,3,5,0,2,4... which maximizes separation
                var colorIndex = (index * 2) % wheelColors.Length;
                if (index >= wheelColors.Length / 2)
                {
                    colorIndex = ((index - wheelColors.Length / 2) * 2 + 1) % wheelColors.Length;
                }
                
                // Additional protection for first/last segment matching
                if (index == totalSegments - 1) // Last segment
                {
                    var firstSegmentColorIndex = 0; // First segment uses index 0
                    if (colorIndex == firstSegmentColorIndex)
                    {
                        // Find an alternative color that's different from first segment and previous segment
                        var previousSegmentIndex = totalSegments - 2;
                        var previousColorIndex = (previousSegmentIndex * 2) % wheelColors.Length;
                        if (previousSegmentIndex >= wheelColors.Length / 2)
                        {
                            previousColorIndex = ((previousSegmentIndex - wheelColors.Length / 2) * 2 + 1) % wheelColors.Length;
                        }
                        
                        // Try to find a color that's different from both first and previous segments
                        for (int offset = 1; offset < wheelColors.Length; offset++)
                        {
                            var alternativeIndex = (colorIndex + offset) % wheelColors.Length;
                            if (alternativeIndex != firstSegmentColorIndex && alternativeIndex != previousColorIndex)
                            {
                                colorIndex = alternativeIndex;
                                break;
                            }
                        }
                    }
                }
                
                return wheelColors[colorIndex];
            }
        }

        public static string GetDynamicFontSize(IEnumerable<string> items)
        {
            // Calculate font size based on number of items and length of longest item
            var itemCount = items.Count();
            var maxLength = items.Any() ? items.Max(s => s.Length) : 0;
            
            // If more than 50 items, keep original smaller sizes
            if (itemCount > 50)
            {
                if (maxLength > 15)
                {
                    if (itemCount <= 3) return "1.0rem";
                    if (itemCount <= 8) return "0.9rem";
                    if (itemCount <= 15) return "0.8rem";
                    return "0.7rem";
                }
                else if (maxLength > 10)
                {
                    if (itemCount <= 3) return "1.2rem";
                    if (itemCount <= 8) return "1.0rem";
                    if (itemCount <= 15) return "0.9rem";
                    if (itemCount <= 25) return "0.8rem";
                    return "0.7rem";
                }
                else
                {
                    if (itemCount <= 3) return "1.4rem";
                    if (itemCount <= 8) return "1.2rem";
                    if (itemCount <= 15) return "1.0rem";
                    if (itemCount <= 25) return "0.9rem";
                    if (itemCount <= 35) return "0.8rem";
                    return "0.7rem";
                }
            }
            
            // Increased sizes by 1.5x for <= 50 items
            if (maxLength > 15)
            {
                // Very conservative sizes for very long text
                if (itemCount <= 3) return "1.5rem";
                if (itemCount <= 8) return "1.35rem";
                if (itemCount <= 15) return "1.2rem";
                return "1.05rem";
            }
            else if (maxLength > 10)
            {
                // Conservative sizes for long text
                if (itemCount <= 3) return "1.8rem";
                if (itemCount <= 8) return "1.5rem";
                if (itemCount <= 15) return "1.35rem";
                if (itemCount <= 25) return "1.2rem";
                return "1.05rem";
            }
            else
            {
                // Normal sizing for shorter text
                if (itemCount <= 3) return "2.1rem";
                if (itemCount <= 8) return "1.8rem";
                if (itemCount <= 15) return "1.5rem";
                if (itemCount <= 25) return "1.35rem";
                if (itemCount <= 35) return "1.2rem";
                return "1.05rem";
            }
        }

        public static (double translateX, double translateY) GetDynamicTextPosition(IEnumerable<string> items)
        {
            // Position text with more conservative inward placement
            var itemCount = items.Count();
            
            // Calculate position with more conservative inward placement
            // The wheel radius is approximately 250px (half of min 500px wheel size)
            var baseRadius = 200; // Further reduced to keep text well inside wheel
            
            // Adjust text distance from center based on number of items and text length
            var maxLength = items.Any() ? items.Max(s => s.Length) : 0;
            double textDistanceRatio;
            
            // More conservative placement for longer text
            if (maxLength > 15)
            {
                textDistanceRatio = 0.60; // Very conservative for very long text
            }
            else if (maxLength > 10)
            {
                textDistanceRatio = 0.65; // Conservative for long text
            }
            else
            {
                // For normal length text, still be more conservative than before
                if (itemCount <= 3)
                {
                    textDistanceRatio = 0.70; // Much more inward for large segments
                }
                else if (itemCount <= 8)
                {
                    textDistanceRatio = 0.75; // More inward for medium segments
                }
                else if (itemCount <= 15)
                {
                    textDistanceRatio = 0.80; // Slightly more inward for smaller segments
                }
                else
                {
                    textDistanceRatio = 0.85; // Still keep some safe distance from edge
                }
            }
            
            var translateX = baseRadius * textDistanceRatio;
            
            // Y translation - keep text centered with more offset for longer text
            double translateY;
            if (maxLength > 15)
            {
                translateY = -20; // Larger offset for very long text
            }
            else if (maxLength > 10)
            {
                translateY = -18; // Large offset for long text
            }
            else if (itemCount <= 5)
            {
                translateY = -15; // Normal offset for few items
            }
            else if (itemCount <= 15)
            {
                translateY = -12; // Slightly smaller offset
            }
            else
            {
                translateY = -10; // Minimal offset for many items
            }
            
            return (translateX, translateY);
        }
    }
}
