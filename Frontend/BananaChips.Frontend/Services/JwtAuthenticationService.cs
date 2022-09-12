using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using BananaChips.Frontend.Constants;
using BananaChips.Frontend.GraphQL.Operations;
using BananaChips.Frontend.GraphQL.Operations.Mutations;
using BananaChips.Frontend.GraphQL.Requests;
using BananaChips.Frontend.GraphQL.Responses;
using BananaChips.Frontend.Services.Meta;
using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;

namespace BananaChips.Frontend.Services;

public class JwtAuthenticationService : IAuthenticationService
{
    private readonly IBackendConnector _backendConnector;
    private readonly ILocalStorageService _localStorageService;
    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    private const string AuthenticationScheme = "Bearer";

    public JwtAuthenticationService(IBackendConnector backendConnector, ILocalStorageService localStorageService)
    {
        _backendConnector = backendConnector;
        _localStorageService = localStorageService;
    }
    public async Task<ClaimsPrincipal?> GetClaimsPrincipal()
    {
        try
        {
            var storedToken = await GetToken();
            if (storedToken == null)
                return null;
            var claimsPrincipal = TokenHandler.ValidateToken(storedToken, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = true,
                SignatureValidator = (token, _) => new JwtSecurityToken(token),
            }, out _);
            return claimsPrincipal;
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> GetToken()
    {
        var token = await _localStorageService.GetItemAsStringAsync(LocalStorageKeysConstants.Token);
        return token;
    }

    public async Task<string?> GetAuthorizationHeaderValueAsync()
    {
        var token = await GetToken();
        return token != null ? $"{AuthenticationScheme} {token}" : null;
    }

    public async Task<string?> Authenticate(string email, string password)
    {
        try
        {
            var request = new LoginMutation.Request(email, password);
            var result =
                await _backendConnector.SendMutationAsync<LoginMutation.Response.Wrapper>(request.Query, request.OperationName,
                    request);
            return !result.IsSuccess ? null : result.Data.Login.AccessToken;
        }
        catch
        {
            return null;
        }
    }
}