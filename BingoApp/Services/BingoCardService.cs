using BingoApp.Models;

namespace BingoApp.Services;

public class BingoCardService
{
    private readonly Random _random = new();
    private readonly List<string[][]> _generatedCards = new();
    private const int MaxAttempts = 1000; // Prevent infinite loops
    private const int GridSize = 5;

    public IEnumerable<string[][]> GenerateCards(BingoSet set, int count)
    {
        if (set?.Items == null || set.Items.Length == 0)
        {
            throw new ArgumentException("Bingo set must contain items", nameof(set));
        }

        if (count <= 0)
        {
            throw new ArgumentException("Count must be greater than 0", nameof(count));
        }

        var cards = new List<string[][]>();
        _generatedCards.Clear();

        for (int i = 0; i < count; i++)
        {
            string[][] newCard;
            int attempts = 0;
            bool isUnique;

            do
            {
                newCard = GenerateCard(set.Items.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
                isUnique = !IsCardDuplicate(newCard);
                attempts++;

                if (attempts >= MaxAttempts)
                {
                    throw new InvalidOperationException($"Unable to generate unique card after {MaxAttempts} attempts. The item set may be too small to generate {count} unique cards.");
                }

            } while (!isUnique);

            _generatedCards.Add(newCard);
            cards.Add(newCard);
        }
        return cards;
    }

    private bool IsCardDuplicate(string[][] newCard)
    {
        if (newCard == null || newCard.Length != GridSize || newCard.Any(row => row == null || row.Length != GridSize))
        {
            throw new ArgumentException("Invalid card format", nameof(newCard));
        }

        foreach (var existingCard in _generatedCards)
        {
            bool isDuplicate = true;
            for (int row = 0; row < 5 && isDuplicate; row++)
            {
                for (int col = 0; col < 5 && isDuplicate; col++)
                {
                    if (existingCard[row][col] != newCard[row][col])
                    {
                        isDuplicate = false;
                    }
                }
            }
            if (isDuplicate)
            {
                return true;
            }
        }
        return false;
    }

    private string[][] GenerateCard(string[] items)
    {
        // Create a 5x5 grid
        var card = new string[GridSize][];
        for (int i = 0; i < GridSize; i++)
        {
            card[i] = new string[GridSize];
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

                // Shuffle the available items for better randomization
                for (int i = availableItems.Count - 1; i > 0; i--)
                {
                    int j = _random.Next(i + 1);
                    (availableItems[j], availableItems[i]) = (availableItems[i], availableItems[j]);
                }

                // Take the first item after shuffling
                card[row][col] = availableItems[0];
                availableItems.RemoveAt(0);
            }
        }

        return card;
    }
}
