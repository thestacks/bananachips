using BananaChips.Frontend.Authentication;
using BananaChips.Frontend.Constants;
using BananaChips.Frontend.Routing;
using BananaChips.Frontend.Services.Meta;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BananaChips.Frontend.Services;

public class LocalStorageSessionManager : ISessionManager
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILocalStorageService _localStorageService;
    private readonly NavigationManager _navigationManager;
    private readonly INotificationService _notificationService;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

    public LocalStorageSessionManager(IAuthenticationService authenticationService,
        ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager, INotificationService notificationService)
    {
        _authenticationService = authenticationService;
        _localStorageService = localStorageService;
        _navigationManager = navigationManager;
        _notificationService = notificationService;
        _authenticationStateProvider = authenticationStateProvider as CustomAuthenticationStateProvider;
    }

    public async Task Login(string email, string password)
    {
        var token = await _authenticationService.Authenticate(email, password);
        if (token == null)
        {
            _notificationService.ShowError("Invalid credentials!");
            return;
        }

        await _localStorageService.SetItemAsStringAsync(LocalStorageKeysConstants.Token, token);
        _authenticationStateProvider.Refresh();
        _navigationManager.NavigateTo(FrontendRoutes.Dashboard);
    }

    public async Task Logout()
    {
        await _localStorageService.RemoveItemAsync(LocalStorageKeysConstants.Token);
        _authenticationStateProvider.Refresh();
        _navigationManager.NavigateTo(FrontendRoutes.Login);
    }
}