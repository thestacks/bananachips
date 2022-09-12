namespace BananaChips.Application.Models.Session;

public record TokenResponse(string AccessToken, int ExpiresIn);