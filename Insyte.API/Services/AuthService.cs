using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Insyte.API.Services;

public class AuthService : IAuthService
{
    private readonly InsyteDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(InsyteDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<(bool Success, string? Error, LoginResponse? Response)> LoginAsync(string email, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

        // Timing attack koruması: kullanıcı bulunamasa da BCrypt çalıştır
        var passwordHash = user?.PasswordHash ?? BCrypt.Net.BCrypt.HashPassword("dummy-to-prevent-timing-attack");
        var passwordValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);

        if (user == null || !passwordValid)
            return (false, "Geçersiz e-posta veya şifre", null);

        var accessToken = GenerateToken(user.Id, user.Email, user.Role.ToString(),
            int.Parse(_config["Jwt:AccessTokenExpirationMinutes"]!));
        var refreshToken = GenerateToken(user.Id, user.Email, user.Role.ToString(),
            int.Parse(_config["Jwt:RefreshTokenExpirationDays"]!) * 24 * 60);

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt);
        return (true, null, new LoginResponse(accessToken, refreshToken, userDto));
    }

    public (bool Success, string? Error, string? AccessToken) Refresh(string refreshToken)
    {
        var principal = ValidateToken(refreshToken);
        if (principal == null)
            return (false, "Geçersiz refresh token", null);

        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var email = principal.FindFirst(ClaimTypes.Email)!.Value;
        var role = principal.FindFirst(ClaimTypes.Role)!.Value;

        var accessToken = GenerateToken(userId, email, role,
            int.Parse(_config["Jwt:AccessTokenExpirationMinutes"]!));

        return (true, null, accessToken);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true
            }, out _);
        }
        catch
        {
            return null;
        }
    }

    public string GenerateToken(Guid userId, string email, string role, int expirationMinutes)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
