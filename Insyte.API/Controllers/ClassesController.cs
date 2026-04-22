using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/classes")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public ClassesController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var query = _db.Classes.Where(c => c.SchoolId == schoolId);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.Level)
            .ThenBy(c => c.Type)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClassDto(c.Id, c.SchoolId, c.Name, c.Level, c.Type, c.Description, c.IsActive, c.CreatedAt, c.UpdatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<ClassDto>>(true, new PagedResult<ClassDto>(items, total, page, pageSize)));
    }

    [HttpGet("{classId}")]
    public async Task<IActionResult> GetById(Guid schoolId, Guid classId)
    {
        var classroom = await _db.Classes.FirstOrDefaultAsync(c => c.Id == classId && c.SchoolId == schoolId);
        if (classroom == null) return NotFound(new ApiError(false, "Sınıf bulunamadı"));

        var dto = new ClassDto(classroom.Id, classroom.SchoolId, classroom.Name, classroom.Level, classroom.Type, classroom.Description, classroom.IsActive, classroom.CreatedAt, classroom.UpdatedAt);
        return Ok(new ApiResponse<ClassDto>(true, dto));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create(Guid schoolId, [FromBody] CreateClassRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var classroom = new Class
        {
            SchoolId = schoolId,
            Name = request.Name,
            Level = request.Level,
            Type = request.Type,
            Description = request.Description
        };

        _db.Classes.Add(classroom);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { schoolId, classId = classroom.Id },
            new ApiResponse<ClassDto>(true, new ClassDto(classroom.Id, classroom.SchoolId, classroom.Name, classroom.Level, classroom.Type, classroom.Description, classroom.IsActive, classroom.CreatedAt, classroom.UpdatedAt)));
    }

    [HttpPut("{classId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid schoolId, Guid classId, [FromBody] UpdateClassRequest request)
    {
        var classroom = await _db.Classes.FirstOrDefaultAsync(c => c.Id == classId && c.SchoolId == schoolId);
        if (classroom == null) return NotFound(new ApiError(false, "Sınıf bulunamadı"));

        if (request.Name != null) classroom.Name = request.Name;
        if (request.Level.HasValue) classroom.Level = request.Level.Value;
        if (request.Type != null) classroom.Type = request.Type;
        if (request.Description != null) classroom.Description = request.Description;
        if (request.IsActive.HasValue) classroom.IsActive = request.IsActive.Value;
        classroom.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Sınıf güncellendi"));
    }

    [HttpDelete("{classId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Delete(Guid schoolId, Guid classId)
    {
        var classroom = await _db.Classes.FirstOrDefaultAsync(c => c.Id == classId && c.SchoolId == schoolId);
        if (classroom == null) return NotFound(new ApiError(false, "Sınıf bulunamadı"));

        classroom.IsActive = false;
        classroom.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Sınıf deaktif edildi"));
    }
}
