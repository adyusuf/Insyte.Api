using System.Security.Claims;
using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Error, LoginResponse? Response)> LoginAsync(string email, string password);
    (bool Success, string? Error, string? AccessToken) Refresh(string refreshToken);
    ClaimsPrincipal? ValidateToken(string token);
    string GenerateToken(Guid userId, string email, string role, int expirationMinutes);
}
