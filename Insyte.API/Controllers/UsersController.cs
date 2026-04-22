using System.Security.Claims;
using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class UsersController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public UsersController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Users.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.Email, u.FirstName, u.LastName, u.Role, u.IsActive, u.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<UserDto>>(true, new PagedResult<UserDto>(items, total, page, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound(new ApiError(false, "Kullanıcı bulunamadı"));

        return Ok(new ApiResponse<UserDto>(true, new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest(new ApiError(false, "Bu e-posta adresi zaten kullanılıyor"));

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id },
            new ApiResponse<UserDto>(true, new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound(new ApiError(false, "Kullanıcı bulunamadı"));

        if (request.Email != null)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Email && u.Id != id))
                return BadRequest(new ApiError(false, "Bu e-posta adresi zaten kullanılıyor"));
            user.Email = request.Email;
        }
        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.Role.HasValue) user.Role = request.Role.Value;
        if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;
        if (request.Password != null) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<UserDto>(true, new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (id == currentUserId)
            return BadRequest(new ApiError(false, "Kendi hesabınızı silemezsiniz"));

        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound(new ApiError(false, "Kullanıcı bulunamadı"));

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Kullanıcı deaktif edildi"));
    }
}
