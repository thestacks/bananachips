using Microsoft.Extensions.Configuration;

namespace BananaChips.Infrastructure.Settings;

public class AuthenticationSettings
{
    public class AdministratorSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public string TokenSecretKey { get; set; }
    public int TokenLifetime { get; set; }
    public string TokenIssuer { get; set; }
    public AdministratorSettings DefaultAdministrator { get; set; }

    public AuthenticationSettings LoadFromConfiguration(IConfiguration configuration)
    {
        TokenSecretKey = configuration.GetValue<string>("Authentication:Tokens:SecretKey");
        TokenLifetime = configuration.GetValue<int>("Authentication:Tokens:Lifetime");
        TokenIssuer = configuration.GetValue<string>("Authentication:Tokens:Issuer");
        DefaultAdministrator =
            configuration.GetSection("Authentication:DefaultAdministrator").Get<AdministratorSettings>();
        return this;
    }
}