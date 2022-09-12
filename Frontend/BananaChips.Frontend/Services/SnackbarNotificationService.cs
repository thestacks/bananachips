using BananaChips.Frontend.Services.Meta;
using MudBlazor;

namespace BananaChips.Frontend.Services;

public class SnackbarNotificationService : INotificationService
{
    private readonly ISnackbar _snackbar;

    public SnackbarNotificationService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void ShowSuccess(string message)
    {
        _snackbar.Clear();
        _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        _snackbar.Configuration.RequireInteraction = false;
        _snackbar.Add(message, Severity.Success);
    }

    public void ShowError(string message)
    {
        _snackbar.Clear();
        _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        _snackbar.Configuration.RequireInteraction = false;
        _snackbar.Add(message, Severity.Error);
    }
}