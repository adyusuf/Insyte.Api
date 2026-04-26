using System.Security.Claims;
using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly InsyteDbContext _db;

    public AuthController(IAuthService authService, InsyteDbContext db)
    {
        _authService = authService;
        _db = db;
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, error, response) = await _authService.LoginAsync(request.Email, request.Password);
        if (!success)
            return Unauthorized(new ApiError(false, error!));

        return Ok(new ApiResponse<LoginResponse>(true, response));
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest request)
    {
        var (success, error, accessToken) = _authService.Refresh(request.RefreshToken);
        if (!success)
            return Unauthorized(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, new { AccessToken = accessToken }));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = GetCurrentUserId();
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt);
        return Ok(new ApiResponse<UserDto>(true, userDto));
    }
}
