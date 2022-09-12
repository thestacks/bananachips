using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BananaChips.Domain.Entities;
using BananaChips.Infrastructure.Services.Meta;
using BananaChips.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ClaimTypes = Common.Authentication.AuthenticationConstants.ClaimTypes;

namespace BananaChips.Infrastructure.Services;

public class JwtAccessTokenManager : IAccessTokenProvider
{
    private readonly AuthenticationSettings _authenticationSettings;

    public JwtAccessTokenManager(IOptions<AuthenticationSettings> authenticationOptions)
    {
        _authenticationSettings = authenticationOptions.Value;
    }

    public (string token, DateTime expirationDate) GenerateAccessToken(User user)
    {
        var expirationDate = DateTime.UtcNow.AddMinutes(_authenticationSettings.TokenLifetime);
        var claims = new List<Claim>
        {
            new(ClaimTypes.UserId, user.Id, ClaimValueTypes.String),
            new(ClaimTypes.Email, user.Email, ClaimValueTypes.String),
            new(ClaimTypes.FirstName, user.FirstName, ClaimValueTypes.String),
            new(ClaimTypes.LastName, user.LastName, ClaimValueTypes.String),
            new(ClaimTypes.FullName, user.FullName, ClaimValueTypes.String),
        };
        var token = BuildTokenWithClaims(claims, expirationDate);
        return (token, expirationDate);
    }

    private string BuildTokenWithClaims(ICollection<Claim> claims, DateTime expirationDate)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _authenticationSettings.TokenIssuer,
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.TokenSecretKey)),
                    SecurityAlgorithms.HmacSha256),
            Expires = expirationDate
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}