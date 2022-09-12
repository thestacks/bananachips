namespace Common.Authentication;

public static class AuthenticationConstants
{
    public static class ClaimTypes
    {
        public const string UserId = System.Security.Claims.ClaimTypes.NameIdentifier;
        public const string Email = System.Security.Claims.ClaimTypes.Email;
        public const string FullName = System.Security.Claims.ClaimTypes.Name;
        public const string FirstName = System.Security.Claims.ClaimTypes.GivenName;
        public const string LastName = System.Security.Claims.ClaimTypes.Surname;
        public const string Role = System.Security.Claims.ClaimTypes.Role;
    }
}