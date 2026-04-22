using Insyte.API.DTOs;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public TeachersController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] Guid? schoolId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Users.Where(u => u.Role == UserRole.Teacher);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));

        if (schoolId.HasValue)
            query = query.Where(u => u.SchoolUsers.Any(su => su.SchoolId == schoolId.Value));

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
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Teacher);
        if (user == null) return NotFound(new ApiError(false, "Öğretmen bulunamadı"));

        return Ok(new ApiResponse<UserDto>(true, new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt)));
    }
}
