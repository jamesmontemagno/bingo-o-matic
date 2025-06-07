using System;

namespace BingoApp.Helpers
{
    public static class ColorHelpers
    {
        /// <summary>
        /// A collection of dark theme colors for UI elements
        /// </summary>
        public static readonly string[] DarkColors = new[]
        {
            "#1E40AF", // Deep Blue
            "#065F46", // Forest Green
            "#831843", // Dark Pink
            "#3730A3", // Indigo
            "#7E22CE", // Purple
            "#9D174D", // Raspberry
            "#B45309", // Burnt Orange
            "#064E3B", // Dark Emerald
            "#134E4A", // Dark Teal
            "#450A0A", // Dark Red
            "#2563EB", // Medium Blue
            "#0891B2", // Cyan
            "#4338CA", // Medium Indigo
            "#7C3AED", // Medium Purple
            "#C026D3", // Medium Pink
            "#E11D48", // Medium Red
            "#EA580C", // Medium Orange
            "#65A30D", // Medium Green
            "#0D9488", // Medium Teal
            "#6D28D9", // Medium Violet
            "#BE185D", // Medium Magenta
            "#92400E", // Brown
            "#166534", // Medium Forest
            "#075985"  // Ocean Blue
        };

        /// <summary>
        /// Gets a random dark color
        /// </summary>
        /// <param name="random">Random number generator</param>
        /// <param name="previousColor">Previous color to avoid (optional)</param>
        /// <returns>A random dark color that differs from the previous color</returns>
        public static string GetRandomDarkColor(Random random, string previousColor = "")
        {
            if (string.IsNullOrEmpty(previousColor) || DarkColors.Length <= 1)
            {
                return DarkColors[random.Next(DarkColors.Length)];
            }

            string newColor;
            do
            {
                newColor = DarkColors[random.Next(DarkColors.Length)];
            } while (newColor == previousColor);
            
            return newColor;
        }
    }
}
