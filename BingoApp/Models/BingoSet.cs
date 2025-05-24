namespace BingoApp.Models;

public record BingoSet
{
    public required string Name { get; init; }
    public required string[] Items { get; init; }
}
