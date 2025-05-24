using Microsoft.AspNetCore.Components;

namespace BingoApp.Features.CustomBingo.Components;

public partial class BingoBoard : ComponentBase
{
    [Parameter]
    public string[] Items { get; set; } = Array.Empty<string>();

    private string[] shuffledItems = Array.Empty<string>();
    private bool[] selectedCells = new bool[25];
    private Random random = new Random();

    protected override void OnParametersSet()
    {
        if (Items.Length > 0)
        {
            shuffledItems = Items.OrderBy(x => random.Next()).Take(25).ToArray();
            selectedCells = new bool[25];
        }
    }

    private void ToggleCell(int index)
    {
        selectedCells[index] = !selectedCells[index];
        StateHasChanged();
    }
}
