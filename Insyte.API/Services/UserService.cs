using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class UserService : IUserService
{
    private readonly InsyteDbContext _db;

    public UserService(InsyteDbContext db) => _db = db;

    public async Task<PagedResult<UserDto>> GetAllAsync(string? search, int page, int pageSize)
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

        return new PagedResult<UserDto>(items, total, page, pageSize);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt);
    }

    public async Task<(bool Success, string? Error, UserDto? User)> CreateAsync(CreateUserRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            return (false, "Bu e-posta adresi zaten kullanılıyor", null);

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

        return (true, null, new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt));
    }

    public async Task<(bool Success, string? Error, UserDto? User)> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return (false, "Kullanıcı bulunamadı", null);

        if (request.Email != null)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Email && u.Id != id))
                return (false, "Bu e-posta adresi zaten kullanılıyor", null);
            user.Email = request.Email;
        }
        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.Role.HasValue) user.Role = request.Role.Value;
        if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;
        if (request.Password != null) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, null, new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role, user.IsActive, user.CreatedAt));
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid currentUserId)
    {
        if (id == currentUserId)
            return (false, "Kendi hesabınızı silemezsiniz");

        var user = await _db.Users.FindAsync(id);
        if (user == null) return (false, "Kullanıcı bulunamadı");

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
