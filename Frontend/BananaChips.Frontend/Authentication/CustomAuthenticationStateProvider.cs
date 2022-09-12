using System.Security.Claims;
using BananaChips.Frontend.Services.Meta;
using Microsoft.AspNetCore.Components.Authorization;

namespace BananaChips.Frontend.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthenticationService _authenticationService;

    public CustomAuthenticationStateProvider(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var claimsPrincipal = await _authenticationService.GetClaimsPrincipal();
        return new AuthenticationState(claimsPrincipal ?? new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void Refresh()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}