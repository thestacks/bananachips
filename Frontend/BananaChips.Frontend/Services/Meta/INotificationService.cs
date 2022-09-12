namespace BananaChips.Frontend.Services.Meta;

public interface INotificationService
{
    void ShowSuccess(string message);
    void ShowError(string message);
}