using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CouncilsController : ControllerBase
{
    private readonly InsyteDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CouncilsController(InsyteDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    // Extract user ID from JWT claims
    private async Task<Guid?> GetUserIdAsync()
    {
        var emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ??
                        _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;

        if (string.IsNullOrEmpty(emailClaim))
            return null;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
        return user?.Id;
    }

    // Get user's school ID from their SchoolUsers or SchoolAdvisors
    private async Task<Guid?> GetUserSchoolIdAsync(Guid userId)
    {
        var schoolUser = await _db.SchoolUsers
            .Where(su => su.UserId == userId)
            .Select(su => su.SchoolId)
            .FirstOrDefaultAsync();

        if (schoolUser != Guid.Empty) return schoolUser;

        var advisorSchool = await _db.SchoolAdvisors
            .Where(sa => sa.UserId == userId)
            .Select(sa => sa.SchoolId)
            .FirstOrDefaultAsync();

        return advisorSchool != Guid.Empty ? advisorSchool : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var query = _db.Councils.Where(c => c.SchoolId == schoolId).AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search) || (c.Description != null && c.Description.Contains(search)));

        var total = await query.CountAsync();

        var councilIds = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => c.Id)
            .ToListAsync();

        var councils = await _db.Councils
            .Where(c => councilIds.Contains(c.Id))
            .Include(c => c.Members)
            .ToListAsync();

        var items = councilIds
            .Select(id => councils.First(c => c.Id == id))
            .Select(c => new CouncilDto(
                c.Id, c.Name, c.Description, c.SchoolId, c.IsActive, c.Members.Count, c.CreatedAt))
            .ToList();

        return Ok(new ApiResponse<PagedResult<CouncilDto>>(true, new PagedResult<CouncilDto>(items, total, page, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var council = await _db.Councils
            .Where(c => c.Id == id && c.SchoolId == schoolId)
            .Include(c => c.Members)
            .FirstOrDefaultAsync();

        if (council == null)
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        var dto = new CouncilDto(
            council.Id, council.Name, council.Description, council.SchoolId, council.IsActive, council.Members.Count, council.CreatedAt);

        return Ok(new ApiResponse<CouncilDto>(true, dto));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create([FromBody] CreateCouncilRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var council = new Council
        {
            Name = request.Name,
            Description = request.Description,
            SchoolId = schoolId.Value
        };

        _db.Councils.Add(council);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = council.Id },
            new ApiResponse<CouncilDto>(true, new CouncilDto(council.Id, council.Name, council.Description, council.SchoolId, council.IsActive, 0, council.CreatedAt)));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCouncilRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var council = await _db.Councils
            .FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == schoolId);

        if (council == null)
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        if (request.Name != null) council.Name = request.Name;
        if (request.Description != null) council.Description = request.Description;
        if (request.IsActive.HasValue) council.IsActive = request.IsActive.Value;
        council.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Kurul güncellendi"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var council = await _db.Councils
            .FirstOrDefaultAsync(c => c.Id == id && c.SchoolId == schoolId);

        if (council == null)
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        council.IsActive = false;
        council.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Kurul deaktif edildi"));
    }

    // --- Members ---
    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetMembers(Guid id)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        if (!await _db.Councils.AnyAsync(c => c.Id == id && c.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        var members = await _db.CouncilMembers
            .Where(cm => cm.CouncilId == id)
            .Include(cm => cm.User)
            .Select(cm => new CouncilMemberDto(
                cm.Id, cm.CouncilId, cm.UserId, cm.User!.FirstName, cm.User!.LastName, cm.User!.Email, cm.Role, cm.AssignedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<CouncilMemberDto>>(true, members));
    }

    [HttpPost("{id}/members")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddCouncilMemberRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        if (!await _db.Councils.AnyAsync(c => c.Id == id && c.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        if (await _db.CouncilMembers.AnyAsync(cm => cm.CouncilId == id && cm.UserId == request.UserId))
            return BadRequest(new ApiError(false, "Bu kullanıcı zaten kurulda üye"));

        var member = new CouncilMember
        {
            CouncilId = id,
            UserId = request.UserId,
            Role = request.Role
        };

        _db.CouncilMembers.Add(member);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Üye eklendi"));
    }

    [HttpDelete("{id}/members/{memberId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberId)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        if (!await _db.Councils.AnyAsync(c => c.Id == id && c.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        var member = await _db.CouncilMembers
            .FirstOrDefaultAsync(cm => cm.CouncilId == id && cm.Id == memberId);

        if (member == null)
            return NotFound(new ApiError(false, "Üyelik bulunamadı"));

        _db.CouncilMembers.Remove(member);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Üye kaldırıldı"));
    }

    [HttpPut("{id}/members/{memberId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> UpdateMember(Guid id, Guid memberId, [FromBody] UpdateMemberRoleRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        if (!await _db.Councils.AnyAsync(c => c.Id == id && c.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Kurul bulunamadı"));

        var member = await _db.CouncilMembers
            .FirstOrDefaultAsync(cm => cm.CouncilId == id && cm.Id == memberId);

        if (member == null)
            return NotFound(new ApiError(false, "Üyelik bulunamadı"));

        if (!string.IsNullOrEmpty(request.Role))
            member.Role = request.Role;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Üyelik güncellendi"));
    }
}
