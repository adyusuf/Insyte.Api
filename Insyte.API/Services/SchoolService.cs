using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class SchoolService : ISchoolService
{
    private readonly InsyteDbContext _db;
    private readonly IConfiguration _config;

    public SchoolService(InsyteDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<PagedResult<SchoolDto>> GetAllAsync(string? search, int page, int pageSize)
    {
        var query = _db.Schools.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(s => s.Name.Contains(search) || (s.City != null && s.City.Contains(search)));

        var total = await query.CountAsync();

        var schoolIds = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => s.Id)
            .ToListAsync();

        var schools = await _db.Schools
            .Where(s => schoolIds.Contains(s.Id))
            .Include(s => s.Advisors)
            .Include(s => s.Users)
            .Include(s => s.Videos)
            .ToListAsync();

        var items = schoolIds
            .Select(id => schools.First(s => s.Id == id))
            .Select(s => new SchoolDto(
                s.Id, s.Name, s.Address, s.City, s.Phone, s.Email, s.IsActive, s.CreatedAt,
                s.Advisors.Count,
                s.Users.Count(u => u.Role == UserRole.Teacher),
                s.Videos.Count))
            .ToList();

        return new PagedResult<SchoolDto>(items, total, page, pageSize);
    }

    public async Task<SchoolDto?> GetByIdAsync(Guid id)
    {
        var school = await _db.Schools
            .Include(s => s.Advisors)
            .Include(s => s.Users)
            .Include(s => s.Videos)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (school == null) return null;

        return new SchoolDto(
            school.Id, school.Name, school.Address, school.City, school.Phone, school.Email,
            school.IsActive, school.CreatedAt,
            school.Advisors.Count, school.Users.Count(u => u.Role == UserRole.Teacher), school.Videos.Count);
    }

    public async Task<SchoolDto> CreateAsync(
        string name, string? address, string? city, string? phone, string? email,
        string? website, string? schoolType, string? institutionType, string? liseType,
        string? educationSystem, double? latitude, double? longitude, IFormFile? logo)
    {
        var logoPath = null as string;

        if (logo != null && logo.Length > 0)
        {
            var uploadPath = _config["Upload:LogoPath"]!;
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(logo.FileName)}";
            logoPath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(logoPath, FileMode.Create);
            await logo.CopyToAsync(stream);
        }

        var schoolTypeEnum = !string.IsNullOrEmpty(schoolType) && Enum.TryParse<SchoolType>(schoolType, out var st) ? st : SchoolType.Ilkokul;
        var institutionTypeEnum = !string.IsNullOrEmpty(institutionType) && Enum.TryParse<InstitutionType>(institutionType, out var it) ? it : InstitutionType.Devlet;
        var liseTypeEnum = null as LiseType?;
        if (schoolTypeEnum == SchoolType.Lise && !string.IsNullOrEmpty(liseType) && Enum.TryParse<LiseType>(liseType, out var lt))
            liseTypeEnum = lt;
        var educationSystemEnum = null as EducationSystem?;
        if (!string.IsNullOrEmpty(educationSystem) && Enum.TryParse<EducationSystem>(educationSystem, out var es))
            educationSystemEnum = es;

        var school = new School
        {
            Name = name,
            Address = address,
            City = city,
            Phone = phone,
            Email = email,
            Website = website,
            LogoPath = logoPath,
            Latitude = latitude,
            Longitude = longitude,
            SchoolType = schoolTypeEnum,
            InstitutionType = institutionTypeEnum,
            LiseType = liseTypeEnum,
            EducationSystem = educationSystemEnum
        };

        _db.Schools.Add(school);
        await _db.SaveChangesAsync();

        return new SchoolDto(school.Id, school.Name, school.Address, school.City, school.Phone, school.Email, school.IsActive, school.CreatedAt, 0, 0, 0);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Guid id, UpdateSchoolRequest request)
    {
        var school = await _db.Schools.FindAsync(id);
        if (school == null) return (false, "Okul bulunamadı");

        if (request.Name != null) school.Name = request.Name;
        if (request.Address != null) school.Address = request.Address;
        if (request.City != null) school.City = request.City;
        if (request.Phone != null) school.Phone = request.Phone;
        if (request.Email != null) school.Email = request.Email;
        if (request.IsActive.HasValue) school.IsActive = request.IsActive.Value;
        school.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id)
    {
        var school = await _db.Schools.FindAsync(id);
        if (school == null) return (false, "Okul bulunamadı");

        school.IsActive = false;
        school.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Advisors
    public async Task<List<SchoolTeacherDto>> GetAdvisorsAsync(Guid schoolId)
    {
        return await _db.SchoolAdvisors
            .Where(sa => sa.SchoolId == schoolId)
            .Include(sa => sa.User)
            .Select(sa => new SchoolTeacherDto(sa.Id, sa.UserId, sa.User.FirstName, sa.User.LastName, sa.User.Email, sa.User.Role, sa.AssignedAt))
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error)> AssignAdvisorAsync(Guid schoolId, Guid userId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı");

        if (await _db.SchoolAdvisors.AnyAsync(sa => sa.SchoolId == schoolId && sa.UserId == userId))
            return (false, "Bu danışman zaten atanmış");

        _db.SchoolAdvisors.Add(new SchoolAdvisor { SchoolId = schoolId, UserId = userId });
        await _db.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> RemoveAdvisorAsync(Guid schoolId, Guid advisorId)
    {
        var sa = await _db.SchoolAdvisors.FirstOrDefaultAsync(sa => sa.SchoolId == schoolId && sa.Id == advisorId);
        if (sa == null) return (false, "Danışman ataması bulunamadı");

        _db.SchoolAdvisors.Remove(sa);
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Teachers
    public async Task<List<SchoolTeacherDto>> GetTeachersAsync(Guid schoolId)
    {
        return await _db.SchoolUsers
            .Where(su => su.SchoolId == schoolId)
            .Include(su => su.User)
            .Select(su => new SchoolTeacherDto(su.Id, su.UserId, su.User.FirstName, su.User.LastName, su.User.Email, su.Role, su.AssignedAt))
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error)> AssignTeacherAsync(Guid schoolId, Guid userId, UserRole role)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı");

        if (await _db.SchoolUsers.AnyAsync(su => su.SchoolId == schoolId && su.UserId == userId))
            return (false, "Bu kullanıcı zaten atanmış");

        _db.SchoolUsers.Add(new SchoolUser { SchoolId = schoolId, UserId = userId, Role = role });
        await _db.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> RemoveTeacherAsync(Guid schoolId, Guid teacherId)
    {
        var su = await _db.SchoolUsers.FirstOrDefaultAsync(su => su.SchoolId == schoolId && su.Id == teacherId);
        if (su == null) return (false, "Öğretmen ataması bulunamadı");

        _db.SchoolUsers.Remove(su);
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Email Config
    public async Task<(bool Exists, List<EmailConfigDto> Configs)> GetEmailConfigsAsync(Guid schoolId)
    {
        var configs = await _db.EmailConfigs
            .Where(ec => ec.SchoolId == schoolId)
            .Select(ec => new EmailConfigDto(ec.Id, ec.SchoolId, ec.RecipientEmail, ec.RecipientName, ec.RecipientType, ec.IsActive))
            .ToListAsync();

        return (true, configs);
    }

    public async Task<(bool Success, string? Error)> AddEmailConfigAsync(Guid schoolId, CreateEmailConfigRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı");

        var config = new EmailConfig
        {
            SchoolId = schoolId,
            RecipientEmail = request.RecipientEmail,
            RecipientName = request.RecipientName,
            RecipientType = request.RecipientType
        };

        _db.EmailConfigs.Add(config);
        await _db.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> RemoveEmailConfigAsync(Guid schoolId, Guid configId)
    {
        var config = await _db.EmailConfigs.FirstOrDefaultAsync(ec => ec.SchoolId == schoolId && ec.Id == configId);
        if (config == null) return (false, "E-posta yapılandırması bulunamadı");

        _db.EmailConfigs.Remove(config);
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
