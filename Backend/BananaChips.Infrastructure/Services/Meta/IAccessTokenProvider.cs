using BananaChips.Domain.Entities;

namespace BananaChips.Infrastructure.Services.Meta;

public interface IAccessTokenProvider
{
    (string token, DateTime expirationDate) GenerateAccessToken(User user);  
}