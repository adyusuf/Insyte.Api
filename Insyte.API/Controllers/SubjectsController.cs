using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/subjects")]
[Authorize]
public class SubjectsController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public SubjectsController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var query = _db.Subjects.Where(s => s.SchoolId == schoolId);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(s => s.Name.Contains(search) || s.Branch.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SubjectDto(s.Id, s.SchoolId, s.Name, s.Branch, s.Level, s.Description, s.WeeklyHours, s.IsActive, s.CreatedAt, s.UpdatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<SubjectDto>>(true, new PagedResult<SubjectDto>(items, total, page, pageSize)));
    }

    [HttpGet("{subjectId}")]
    public async Task<IActionResult> GetById(Guid schoolId, Guid subjectId)
    {
        var subject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId && s.SchoolId == schoolId);
        if (subject == null) return NotFound(new ApiError(false, "Ders bulunamadı"));

        var dto = new SubjectDto(subject.Id, subject.SchoolId, subject.Name, subject.Branch, subject.Level, subject.Description, subject.WeeklyHours, subject.IsActive, subject.CreatedAt, subject.UpdatedAt);
        return Ok(new ApiResponse<SubjectDto>(true, dto));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create(Guid schoolId, [FromBody] CreateSubjectRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var subject = new Subject
        {
            SchoolId = schoolId,
            Name = request.Name,
            Branch = request.Branch,
            Level = request.Level,
            Description = request.Description,
            WeeklyHours = request.WeeklyHours
        };

        _db.Subjects.Add(subject);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { schoolId, subjectId = subject.Id },
            new ApiResponse<SubjectDto>(true, new SubjectDto(subject.Id, subject.SchoolId, subject.Name, subject.Branch, subject.Level, subject.Description, subject.WeeklyHours, subject.IsActive, subject.CreatedAt, subject.UpdatedAt)));
    }

    [HttpPut("{subjectId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid schoolId, Guid subjectId, [FromBody] UpdateSubjectRequest request)
    {
        var subject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId && s.SchoolId == schoolId);
        if (subject == null) return NotFound(new ApiError(false, "Ders bulunamadı"));

        if (request.Name != null) subject.Name = request.Name;
        if (request.Branch != null) subject.Branch = request.Branch;
        if (request.Level != null) subject.Level = request.Level;
        if (request.Description != null) subject.Description = request.Description;
        if (request.WeeklyHours.HasValue) subject.WeeklyHours = request.WeeklyHours.Value;
        if (request.IsActive.HasValue) subject.IsActive = request.IsActive.Value;
        subject.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Ders güncellendi"));
    }

    [HttpDelete("{subjectId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Delete(Guid schoolId, Guid subjectId)
    {
        var subject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId && s.SchoolId == schoolId);
        if (subject == null) return NotFound(new ApiError(false, "Ders bulunamadı"));

        subject.IsActive = false;
        subject.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Ders deaktif edildi"));
    }
}
