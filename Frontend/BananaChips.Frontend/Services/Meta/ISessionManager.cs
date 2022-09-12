namespace BananaChips.Frontend.Services.Meta;

public interface ISessionManager
{
    Task Login(string email, string password);
    Task Logout();
}