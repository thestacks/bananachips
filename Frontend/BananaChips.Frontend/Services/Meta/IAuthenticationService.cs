using System.Security.Claims;

namespace BananaChips.Frontend.Services.Meta;

public interface IAuthenticationService
{
    Task<ClaimsPrincipal?> GetClaimsPrincipal();
    Task<string?> GetToken();
    Task<string?> GetAuthorizationHeaderValueAsync();
    Task<string?> Authenticate(string email, string password);
}