using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class ClassService : IClassService
{
    private readonly InsyteDbContext _db;

    public ClassService(InsyteDbContext db) => _db = db;

    public async Task<(bool SchoolExists, PagedResult<ClassDto>? Result)> GetAllAsync(Guid schoolId, string? search, int page, int pageSize)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

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

        return (true, new PagedResult<ClassDto>(items, total, page, pageSize));
    }

    public async Task<ClassDto?> GetByIdAsync(Guid id, Guid schoolId)
    {
        var classroom = await _db.Classes.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == schoolId);
        if (classroom == null) return null;

        return new ClassDto(classroom.Id, classroom.SchoolId, classroom.Name, classroom.Level, classroom.Type, classroom.Description, classroom.IsActive, classroom.CreatedAt, classroom.UpdatedAt);
    }

    public async Task<(bool Success, string? Error, ClassDto? Class)> CreateAsync(Guid schoolId, CreateClassRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null);

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

        return (true, null, new ClassDto(classroom.Id, classroom.SchoolId, classroom.Name, classroom.Level, classroom.Type, classroom.Description, classroom.IsActive, classroom.CreatedAt, classroom.UpdatedAt));
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Guid id, Guid schoolId, UpdateClassRequest request)
    {
        var classroom = await _db.Classes.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == schoolId);
        if (classroom == null) return (false, "Sınıf bulunamadı");

        if (request.Name != null) classroom.Name = request.Name;
        if (request.Level.HasValue) classroom.Level = request.Level.Value;
        if (request.Type != null) classroom.Type = request.Type;
        if (request.Description != null) classroom.Description = request.Description;
        if (request.IsActive.HasValue) classroom.IsActive = request.IsActive.Value;
        classroom.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid schoolId)
    {
        var classroom = await _db.Classes.FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == schoolId);
        if (classroom == null) return (false, "Sınıf bulunamadı");

        classroom.IsActive = false;
        classroom.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
