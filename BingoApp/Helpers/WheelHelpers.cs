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
        }        public static string GetDynamicFontSize(IEnumerable<string> items)
        {
            // Calculate font size based on number of items and length of longest item
            var itemCount = items.Count();
            var maxLength = items.Any() ? items.Max(s => s.Length) : 0;
            
            // Use larger font sizes to improve readability
            if (itemCount > 50)
            {
                if (maxLength > 15)
                {
                    if (itemCount <= 3) return "1.2rem";
                    if (itemCount <= 8) return "1.1rem";
                    if (itemCount <= 15) return "1.0rem";
                    return "0.9rem";
                }
                else if (maxLength > 10)
                {
                    if (itemCount <= 3) return "1.4rem";
                    if (itemCount <= 8) return "1.2rem";
                    if (itemCount <= 15) return "1.1rem";
                    if (itemCount <= 25) return "1.0rem";
                    return "0.9rem";
                }
                else
                {
                    if (itemCount <= 3) return "1.6rem";
                    if (itemCount <= 8) return "1.4rem";
                    if (itemCount <= 15) return "1.2rem";
                    if (itemCount <= 25) return "1.1rem";
                    if (itemCount <= 35) return "1.0rem";
                    return "0.9rem";
                }
            }
            
            // Increased sizes for better visibility
            if (maxLength > 15)
            {
                // Larger sizes for very long text
                if (itemCount <= 3) return "1.8rem";
                if (itemCount <= 8) return "1.6rem";
                if (itemCount <= 15) return "1.4rem";
                return "1.2rem";
            }
            else if (maxLength > 10)
            {
                // Larger sizes for long text
                if (itemCount <= 3) return "2.0rem";
                if (itemCount <= 8) return "1.8rem";
                if (itemCount <= 15) return "1.6rem";
                if (itemCount <= 25) return "1.4rem";
                return "1.2rem";
            }
            else
            {
                // Larger sizes for shorter text
                if (itemCount <= 3) return "2.4rem";
                if (itemCount <= 8) return "2.1rem";
                if (itemCount <= 15) return "1.8rem";
                if (itemCount <= 25) return "1.6rem";
                if (itemCount <= 35) return "1.4rem";
                return "1.2rem";
            }
        }public static (double translateX, double translateY) GetDynamicTextPosition(IEnumerable<string> items)
        {
            // Position text closer to the edge of the wheel
            var itemCount = items.Count();
            
            // Calculate position with placement closer to the edge
            // The wheel radius is now responsive based on viewport
            var baseRadius = 250; // Base radius for positioning calculations
            
            // Adjust text distance from center based on number of items and text length
            var maxLength = items.Any() ? items.Max(s => s.Length) : 0;
            double textDistanceRatio;
            
            // Position text closer to the edge but still prevent text cutoff
            if (maxLength > 15)
            {
                textDistanceRatio = 0.70; // Move closer to edge but still be careful with very long text
            }
            else if (maxLength > 10)
            {
                textDistanceRatio = 0.75; // Position longer text closer to edge
            }
            else
            {
                // For normal length text, place closer to the edge
                if (itemCount <= 3)
                {
                    textDistanceRatio = 0.80; // Large segments can have text quite close to edge
                }
                else if (itemCount <= 8)
                {
                    textDistanceRatio = 0.82; // Move closer to edge for medium segments
                }
                else if (itemCount <= 15)
                {
                    textDistanceRatio = 0.85; // Even closer for smaller segments
                }
                else
                {
                    textDistanceRatio = 0.87; // Closest to edge for many small segments
                }
            }
            
            var translateX = baseRadius * textDistanceRatio;
            
            // Y translation - keep text centered with more offset for longer text
            double translateY;
            if (maxLength > 15)
            {
                translateY = -25; // Larger offset for very long text
            }
            else if (maxLength > 10)
            {
                translateY = -22; // Large offset for long text
            }
            else if (itemCount <= 5)
            {
                translateY = -18; // Normal offset for few items
            }
            else if (itemCount <= 15)
            {
                translateY = -15; // Slightly smaller offset
            }
            else
            {
                translateY = -12; // Minimal offset for many items
            }
            
            return (translateX, translateY);
        }
    }
}
