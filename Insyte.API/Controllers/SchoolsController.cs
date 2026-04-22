using System.Security.Claims;
using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchoolsController : ControllerBase
{
    private readonly InsyteDbContext _db;
    private readonly IConfiguration _config;

    public SchoolsController(InsyteDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Schools.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(s => s.Name.Contains(search) || (s.City != null && s.City.Contains(search)));

        var total = await query.CountAsync();

        // Get school IDs for the current page to avoid N+1 query problem
        var schoolIds = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => s.Id)
            .ToListAsync();

        // Get all schools with related data loaded
        var schools = await _db.Schools
            .Where(s => schoolIds.Contains(s.Id))
            .Include(s => s.Advisors)
            .Include(s => s.Users)
            .Include(s => s.Videos)
            .ToListAsync();

        // Maintain order from pagination
        var items = schoolIds
            .Select(id => schools.First(s => s.Id == id))
            .Select(s => new SchoolDto(
                s.Id, s.Name, s.Address, s.City, s.Phone, s.Email, s.IsActive, s.CreatedAt,
                s.Advisors.Count,
                s.Users.Count(u => u.Role == UserRole.Teacher),
                s.Videos.Count))
            .ToList();

        return Ok(new ApiResponse<PagedResult<SchoolDto>>(true, new PagedResult<SchoolDto>(items, total, page, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var school = await _db.Schools
            .Include(s => s.Advisors)
            .Include(s => s.Users)
            .Include(s => s.Videos)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (school == null) return NotFound(new ApiError(false, "Okul bulunamadı"));

        var dto = new SchoolDto(
            school.Id, school.Name, school.Address, school.City, school.Phone, school.Email,
            school.IsActive, school.CreatedAt,
            school.Advisors.Count, school.Users.Count(u => u.Role == UserRole.Teacher), school.Videos.Count);

        return Ok(new ApiResponse<SchoolDto>(true, dto));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAdvisor")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] string name, [FromForm] string? address, [FromForm] string? city,
        [FromForm] string? phone, [FromForm] string? email, [FromForm] string? website, [FromForm] string? schoolType,
        [FromForm] string? institutionType, [FromForm] string? liseType, [FromForm] string? educationSystem,
        [FromForm] double? latitude, [FromForm] double? longitude, [FromForm] IFormFile? logo)
    {
        var logoPath = null as string;

        if (logo != null && logo.Length > 0)
        {
            var uploadPath = _config["Upload:LogoPath"]!;
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(logo.FileName)}";
            logoPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(logoPath, FileMode.Create))
            {
                await logo.CopyToAsync(stream);
            }
        }

        // Parse school type and institution type
        var schoolTypeEnum = !string.IsNullOrEmpty(schoolType) && Enum.TryParse<SchoolType>(schoolType, out var st) ? st : SchoolType.Ilkokul;
        var institutionTypeEnum = !string.IsNullOrEmpty(institutionType) && Enum.TryParse<InstitutionType>(institutionType, out var it) ? it : InstitutionType.Devlet;
        var liseTypeEnum = null as LiseType?;
        if (schoolTypeEnum == SchoolType.Lise && !string.IsNullOrEmpty(liseType) && Enum.TryParse<LiseType>(liseType, out var lt))
        {
            liseTypeEnum = lt;
        }
        var educationSystemEnum = null as EducationSystem?;
        if (!string.IsNullOrEmpty(educationSystem) && Enum.TryParse<EducationSystem>(educationSystem, out var es))
        {
            educationSystemEnum = es;
        }

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

        return CreatedAtAction(nameof(GetById), new { id = school.Id },
            new ApiResponse<SchoolDto>(true, new SchoolDto(school.Id, school.Name, school.Address, school.City, school.Phone, school.Email, school.IsActive, school.CreatedAt, 0, 0, 0)));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSchoolRequest request)
    {
        var school = await _db.Schools.FindAsync(id);
        if (school == null) return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (request.Name != null) school.Name = request.Name;
        if (request.Address != null) school.Address = request.Address;
        if (request.City != null) school.City = request.City;
        if (request.Phone != null) school.Phone = request.Phone;
        if (request.Email != null) school.Email = request.Email;
        if (request.IsActive.HasValue) school.IsActive = request.IsActive.Value;
        school.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Okul güncellendi"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var school = await _db.Schools.FindAsync(id);
        if (school == null) return NotFound(new ApiError(false, "Okul bulunamadı"));

        school.IsActive = false;
        school.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Okul deaktif edildi"));
    }

    // --- Advisors ---
    [HttpGet("{id}/advisors")]
    public async Task<IActionResult> GetAdvisors(Guid id)
    {
        var advisors = await _db.SchoolAdvisors
            .Where(sa => sa.SchoolId == id)
            .Include(sa => sa.User)
            .Select(sa => new SchoolTeacherDto(sa.Id, sa.UserId, sa.User.FirstName, sa.User.LastName, sa.User.Email, sa.User.Role, sa.AssignedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<SchoolTeacherDto>>(true, advisors));
    }

    [HttpPost("{id}/advisors")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AssignAdvisor(Guid id, [FromBody] AssignAdvisorRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == id))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (await _db.SchoolAdvisors.AnyAsync(sa => sa.SchoolId == id && sa.UserId == request.UserId))
            return BadRequest(new ApiError(false, "Bu danışman zaten atanmış"));

        _db.SchoolAdvisors.Add(new SchoolAdvisor { SchoolId = id, UserId = request.UserId });
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Danışman atandı"));
    }

    [HttpDelete("{id}/advisors/{advisorId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RemoveAdvisor(Guid id, Guid advisorId)
    {
        var sa = await _db.SchoolAdvisors.FirstOrDefaultAsync(sa => sa.SchoolId == id && sa.Id == advisorId);
        if (sa == null) return NotFound(new ApiError(false, "Danışman ataması bulunamadı"));

        _db.SchoolAdvisors.Remove(sa);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Danışman kaldırıldı"));
    }

    // --- Teachers / School Users ---
    [HttpGet("{id}/teachers")]
    public async Task<IActionResult> GetTeachers(Guid id)
    {
        var teachers = await _db.SchoolUsers
            .Where(su => su.SchoolId == id)
            .Include(su => su.User)
            .Select(su => new SchoolTeacherDto(su.Id, su.UserId, su.User.FirstName, su.User.LastName, su.User.Email, su.Role, su.AssignedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<SchoolTeacherDto>>(true, teachers));
    }

    [HttpPost("{id}/teachers")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> AssignTeacher(Guid id, [FromBody] AssignTeacherRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == id))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (await _db.SchoolUsers.AnyAsync(su => su.SchoolId == id && su.UserId == request.UserId))
            return BadRequest(new ApiError(false, "Bu kullanıcı zaten atanmış"));

        _db.SchoolUsers.Add(new SchoolUser { SchoolId = id, UserId = request.UserId, Role = request.Role });
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Öğretmen atandı"));
    }

    [HttpDelete("{id}/teachers/{teacherId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> RemoveTeacher(Guid id, Guid teacherId)
    {
        var su = await _db.SchoolUsers.FirstOrDefaultAsync(su => su.SchoolId == id && su.Id == teacherId);
        if (su == null) return NotFound(new ApiError(false, "Öğretmen ataması bulunamadı"));

        _db.SchoolUsers.Remove(su);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Öğretmen kaldırıldı"));
    }

    // --- Email Config ---
    [HttpGet("{id}/email-config")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> GetEmailConfig(Guid id)
    {
        var configs = await _db.EmailConfigs
            .Where(ec => ec.SchoolId == id)
            .Select(ec => new EmailConfigDto(ec.Id, ec.SchoolId, ec.RecipientEmail, ec.RecipientName, ec.RecipientType, ec.IsActive))
            .ToListAsync();

        return Ok(new ApiResponse<List<EmailConfigDto>>(true, configs));
    }

    [HttpPost("{id}/email-config")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> AddEmailConfig(Guid id, [FromBody] CreateEmailConfigRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == id))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var config = new EmailConfig
        {
            SchoolId = id,
            RecipientEmail = request.RecipientEmail,
            RecipientName = request.RecipientName,
            RecipientType = request.RecipientType
        };

        _db.EmailConfigs.Add(config);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "E-posta yapılandırması eklendi"));
    }

    [HttpDelete("{id}/email-config/{configId}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> RemoveEmailConfig(Guid id, Guid configId)
    {
        var config = await _db.EmailConfigs.FirstOrDefaultAsync(ec => ec.SchoolId == id && ec.Id == configId);
        if (config == null) return NotFound(new ApiError(false, "E-posta yapılandırması bulunamadı"));

        _db.EmailConfigs.Remove(config);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "E-posta yapılandırması kaldırıldı"));
    }
}
