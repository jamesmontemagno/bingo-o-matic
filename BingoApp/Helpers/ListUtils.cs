using System;
using System.Collections.Generic;

namespace BingoApp.Helpers
{
    public static class ListUtils
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Shuffles a list in place using the Fisher-Yates algorithm
        /// </summary>
        public static void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
