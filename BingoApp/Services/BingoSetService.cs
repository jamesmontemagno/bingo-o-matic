using BingoApp.Models;
using BingoApp.Helpers;

namespace BingoApp.Services;

public class BingoSetService
{
    private readonly List<BingoSet> _sets;

    public BingoSetService()
    {
        _sets = new List<BingoSet>();
        InitializeIfEmpty();
    }

    private void InitializeIfEmpty()
    {
        if (!_sets.Any())
        {
            _sets.AddRange(MockBingoData.GetMockSets());
        }
    }

    public IEnumerable<BingoSet> GetAll() => _sets;

    public BingoSet? GetByName(string name) => _sets.FirstOrDefault(s => s.Name == name);

    public BingoSet? GetById(Guid id) => _sets.FirstOrDefault(s => s.Id == id);

    public void AddSet(BingoSet set)
    {
        if (string.IsNullOrWhiteSpace(set.Name))
            throw new ArgumentException("Set name cannot be empty");
            
        if (_sets.Any(s => s.Name.Equals(set.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A set with this name already exists");

        // Remove duplicates and empty items
        set = set with
        {
            Items = set.Items.Where(i => !string.IsNullOrWhiteSpace(i))
                           .Distinct(StringComparer.OrdinalIgnoreCase)
                           .ToArray()
        };

        if (!set.Items.Any())
            throw new ArgumentException("Set must contain at least one item");

        _sets.Add(set);
    }

    public void UpdateSet(Guid id, BingoSet updatedSet)
    {
        var existingSet = _sets.FirstOrDefault(s => s.Id == id) 
            ?? throw new ArgumentException("Set not found");

        if (existingSet.Name != updatedSet.Name && 
            _sets.Any(s => s.Name.Equals(updatedSet.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("A set with this name already exists");

        // Create a new set with the existing ID
        var newSet = new BingoSet
        {
            Id = id,
            Name = updatedSet.Name,
            Items = updatedSet.Items.Where(i => !string.IsNullOrWhiteSpace(i))
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .ToArray()
        };

        if (!newSet.Items.Any())
            throw new ArgumentException("Set must contain at least one item");

        var index = _sets.IndexOf(existingSet);
        _sets[index] = newSet;
    }

    public void DeleteSet(Guid id)
    {
        var set = _sets.FirstOrDefault(s => s.Id == id)
            ?? throw new ArgumentException("Set not found");
        
        _sets.Remove(set);
    }

    // Keep the name-based methods for backward compatibility
    public void UpdateSet(string originalName, BingoSet updatedSet)
    {
        var existingSet = GetByName(originalName) 
            ?? throw new ArgumentException("Set not found");
        UpdateSet(existingSet.Id, updatedSet);
    }

    public void DeleteSet(string name)
    {
        var set = GetByName(name)
            ?? throw new ArgumentException("Set not found");
        DeleteSet(set.Id);
    }

    public static string[] ParseItems(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Array.Empty<string>();

        // Split by newlines, commas, or semicolons
        return input.Split(new[] { '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(s => s.Trim())
                   .Where(s => !string.IsNullOrWhiteSpace(s))
                   .ToArray();
    }
}
