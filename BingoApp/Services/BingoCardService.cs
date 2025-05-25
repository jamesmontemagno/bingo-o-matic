using BingoApp.Models;

namespace BingoApp.Services;

public class BingoCardService
{
    private readonly Random _random = new();

    public IEnumerable<string[][]> GenerateCards(BingoSet set, int count)
    {
        var cards = new List<string[][]>();
        for (int i = 0; i < count; i++)
        {
            cards.Add(GenerateCard(set.Items));
        }
        return cards;
    }

    private string[][] GenerateCard(string[] items)
    {
        // Create a 5x5 grid
        var card = new string[5][];
        for (int i = 0; i < 5; i++)
        {
            card[i] = new string[5];
        }

        // Make a copy of items to shuffle
        var availableItems = items.ToList();

        // Fill the card
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                // Center is free
                if (row == 2 && col == 2)
                {
                    card[row][col] = "FREE";
                    continue;
                }

                // Get random item
                if (availableItems.Count == 0)
                {
                    // If we run out of items, reuse them
                    availableItems = items.ToList();
                }
                int index = _random.Next(availableItems.Count);
                card[row][col] = availableItems[index];
                availableItems.RemoveAt(index);
            }
        }

        return card;
    }
}
