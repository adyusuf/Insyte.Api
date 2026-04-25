using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Insyte.API.DTOs;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly InsyteDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(InsyteDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        // Timing attack koruması: kullanıcı bulunamasa da BCrypt çalıştır
        var passwordHash = user?.PasswordHash ?? BCrypt.Net.BCrypt.HashPassword("dummy-to-prevent-timing-attack");
        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, passwordHash);

        if (user == null || !passwordValid)
            return Unauthorized(new ApiError(false, "Geçersiz e-posta veya şifre"));

        var accessToken = GenerateToken(user.Id, user.Email, user.Role.ToString(),
            int.Parse(_config["Jwt:AccessTokenExpirationMinutes"]!));
        var refreshToken = GenerateToken(user.Id, user.Email, user.Role.ToString(),
            int.Parse(_config["Jwt:RefreshTokenExpirationDays"]!) * 24 * 60);

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt);
        return Ok(new ApiResponse<LoginResponse>(true, new LoginResponse(accessToken, refreshToken, userDto)));
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest request)
    {
        var principal = ValidateToken(request.RefreshToken);
        if (principal == null)
            return Unauthorized(new ApiError(false, "Geçersiz refresh token"));

        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var email = principal.FindFirst(ClaimTypes.Email)!.Value;
        var role = principal.FindFirst(ClaimTypes.Role)!.Value;

        var accessToken = GenerateToken(userId, email, role,
            int.Parse(_config["Jwt:AccessTokenExpirationMinutes"]!));

        return Ok(new ApiResponse<object>(true, new { AccessToken = accessToken }));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt);
        return Ok(new ApiResponse<UserDto>(true, userDto));
    }

    private string GenerateToken(Guid userId, string email, string role, int expirationMinutes)
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

    private ClaimsPrincipal? ValidateToken(string token)
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
}
