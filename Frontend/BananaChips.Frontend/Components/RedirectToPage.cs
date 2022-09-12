using Microsoft.AspNetCore.Components;

namespace BananaChips.Frontend.Components;

public class RedirectToPage : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public string Destination { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.NavigateTo(Destination);
    }
}