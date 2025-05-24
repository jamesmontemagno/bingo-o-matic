namespace BingoApp.Models;

public record BingoSet
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string[] Items { get; set; }
}
