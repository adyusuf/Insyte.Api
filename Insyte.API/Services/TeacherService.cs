using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class TeacherService : ITeacherService
{
    private readonly InsyteDbContext _db;

    public TeacherService(InsyteDbContext db) => _db = db;

    public async Task<PagedResult<UserDto>> GetAllAsync(string? search, Guid? schoolId, int page, int pageSize)
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

        return new PagedResult<UserDto>(items, total, page, pageSize);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Teacher);
        if (user == null) return null;

        return new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt);
    }
}
