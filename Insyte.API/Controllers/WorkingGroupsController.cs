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
public class WorkingGroupsController : ControllerBase
{
    private readonly InsyteDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkingGroupsController(InsyteDbContext db, IHttpContextAccessor httpContextAccessor)
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

        var query = _db.WorkingGroups.Where(wg => wg.SchoolId == schoolId).AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(wg => wg.Name.Contains(search) || (wg.Description != null && wg.Description.Contains(search)));

        var total = await query.CountAsync();

        var workingGroupIds = await query
            .OrderByDescending(wg => wg.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(wg => wg.Id)
            .ToListAsync();

        var groups = await _db.WorkingGroups
            .Where(wg => workingGroupIds.Contains(wg.Id))
            .Include(wg => wg.Members)
            .ToListAsync();

        var items = workingGroupIds
            .Select(id => groups.First(wg => wg.Id == id))
            .Select(wg => new WorkingGroupDto(
                wg.Id, wg.Name, wg.Description, wg.SchoolId, wg.IsActive, wg.Members.Count, wg.CreatedAt))
            .ToList();

        return Ok(new ApiResponse<PagedResult<WorkingGroupDto>>(true, new PagedResult<WorkingGroupDto>(items, total, page, pageSize)));
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

        var group = await _db.WorkingGroups
            .Where(wg => wg.Id == id && wg.SchoolId == schoolId)
            .Include(wg => wg.Members)
            .FirstOrDefaultAsync();

        if (group == null)
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        var dto = new WorkingGroupDto(
            group.Id, group.Name, group.Description, group.SchoolId, group.IsActive, group.Members.Count, group.CreatedAt);

        return Ok(new ApiResponse<WorkingGroupDto>(true, dto));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create([FromBody] CreateWorkingGroupRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var group = new WorkingGroup
        {
            Name = request.Name,
            Description = request.Description,
            SchoolId = schoolId.Value
        };

        _db.WorkingGroups.Add(group);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = group.Id },
            new ApiResponse<WorkingGroupDto>(true, new WorkingGroupDto(group.Id, group.Name, group.Description, group.SchoolId, group.IsActive, 0, group.CreatedAt)));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkingGroupRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        var group = await _db.WorkingGroups
            .FirstOrDefaultAsync(wg => wg.Id == id && wg.SchoolId == schoolId);

        if (group == null)
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        if (request.Name != null) group.Name = request.Name;
        if (request.Description != null) group.Description = request.Description;
        if (request.IsActive.HasValue) group.IsActive = request.IsActive.Value;
        group.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Çalışma grubu güncellendi"));
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

        var group = await _db.WorkingGroups
            .FirstOrDefaultAsync(wg => wg.Id == id && wg.SchoolId == schoolId);

        if (group == null)
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        group.IsActive = false;
        group.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Çalışma grubu deaktif edildi"));
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

        if (!await _db.WorkingGroups.AnyAsync(wg => wg.Id == id && wg.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        var members = await _db.WorkingGroupMembers
            .Where(wgm => wgm.WorkingGroupId == id)
            .Include(wgm => wgm.User)
            .Select(wgm => new WorkingGroupMemberDto(
                wgm.Id, wgm.WorkingGroupId, wgm.UserId, wgm.User!.FirstName, wgm.User!.LastName, wgm.User!.Email, wgm.Role, wgm.AssignedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<WorkingGroupMemberDto>>(true, members));
    }

    [HttpPost("{id}/members")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddWorkingGroupMemberRequest request)
    {
        var userId = await GetUserIdAsync();
        if (userId == null)
            return Unauthorized(new ApiError(false, "Kullanıcı kimliği bulunamadı"));

        var schoolId = await GetUserSchoolIdAsync(userId.Value);
        if (schoolId == null)
            return BadRequest(new ApiError(false, "Kullanıcı bir okula atanmamış"));

        if (!await _db.WorkingGroups.AnyAsync(wg => wg.Id == id && wg.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        if (await _db.WorkingGroupMembers.AnyAsync(wgm => wgm.WorkingGroupId == id && wgm.UserId == request.UserId))
            return BadRequest(new ApiError(false, "Bu kullanıcı zaten grupta üye"));

        var member = new WorkingGroupMember
        {
            WorkingGroupId = id,
            UserId = request.UserId,
            Role = request.Role
        };

        _db.WorkingGroupMembers.Add(member);
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

        if (!await _db.WorkingGroups.AnyAsync(wg => wg.Id == id && wg.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        var member = await _db.WorkingGroupMembers
            .FirstOrDefaultAsync(wgm => wgm.WorkingGroupId == id && wgm.Id == memberId);

        if (member == null)
            return NotFound(new ApiError(false, "Üyelik bulunamadı"));

        _db.WorkingGroupMembers.Remove(member);
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

        if (!await _db.WorkingGroups.AnyAsync(wg => wg.Id == id && wg.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Çalışma grubu bulunamadı"));

        var member = await _db.WorkingGroupMembers
            .FirstOrDefaultAsync(wgm => wgm.WorkingGroupId == id && wgm.Id == memberId);

        if (member == null)
            return NotFound(new ApiError(false, "Üyelik bulunamadı"));

        if (!string.IsNullOrEmpty(request.Role))
            member.Role = request.Role;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Üyelik güncellendi"));
    }
}

public record UpdateMemberRoleRequest(string? Role);
