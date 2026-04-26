using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class SubjectService : ISubjectService
{
    private readonly InsyteDbContext _db;

    public SubjectService(InsyteDbContext db) => _db = db;

    public async Task<(bool SchoolExists, PagedResult<SubjectDto>? Result)> GetAllAsync(Guid schoolId, string? search, int page, int pageSize)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

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

        return (true, new PagedResult<SubjectDto>(items, total, page, pageSize));
    }

    public async Task<SubjectDto?> GetByIdAsync(Guid id, Guid schoolId)
    {
        var subject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId);
        if (subject == null) return null;

        return new SubjectDto(subject.Id, subject.SchoolId, subject.Name, subject.Branch, subject.Level, subject.Description, subject.WeeklyHours, subject.IsActive, subject.CreatedAt, subject.UpdatedAt);
    }

    public async Task<(bool Success, string? Error, SubjectDto? Subject)> CreateAsync(Guid schoolId, CreateSubjectRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null);

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

        return (true, null, new SubjectDto(subject.Id, subject.SchoolId, subject.Name, subject.Branch, subject.Level, subject.Description, subject.WeeklyHours, subject.IsActive, subject.CreatedAt, subject.UpdatedAt));
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Guid id, Guid schoolId, UpdateSubjectRequest request)
    {
        var subject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId);
        if (subject == null) return (false, "Ders bulunamadı");

        if (request.Name != null) subject.Name = request.Name;
        if (request.Branch != null) subject.Branch = request.Branch;
        if (request.Level != null) subject.Level = request.Level;
        if (request.Description != null) subject.Description = request.Description;
        if (request.WeeklyHours.HasValue) subject.WeeklyHours = request.WeeklyHours.Value;
        if (request.IsActive.HasValue) subject.IsActive = request.IsActive.Value;
        subject.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid schoolId)
    {
        var subject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId);
        if (subject == null) return (false, "Ders bulunamadı");

        subject.IsActive = false;
        subject.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
