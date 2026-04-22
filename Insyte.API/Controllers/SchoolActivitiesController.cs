using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/activities")]
[Authorize]
public class SchoolActivitiesController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public SchoolActivitiesController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var activities = await _db.SchoolActivities
            .Where(a => a.SchoolId == schoolId)
            .OrderBy(a => a.Activity)
            .Select(a => new SchoolActivityDto(a.Id, a.SchoolId, a.Activity, a.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<SchoolActivityDto>>(true, activities));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolActivityRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (await _db.SchoolActivities.AnyAsync(a => a.SchoolId == schoolId && a.Activity == request.Activity))
            return BadRequest(new ApiError(false, "Bu aktivite zaten eklenmiş"));

        var activity = new SchoolActivity
        {
            SchoolId = schoolId,
            Activity = request.Activity
        };

        _db.SchoolActivities.Add(activity);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<SchoolActivityDto>(true, new SchoolActivityDto(activity.Id, activity.SchoolId, activity.Activity, activity.CreatedAt), "Aktivite eklendi"));
    }

    [HttpDelete("{activityId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid activityId)
    {
        var activity = await _db.SchoolActivities.FirstOrDefaultAsync(a => a.Id == activityId && a.SchoolId == schoolId);
        if (activity == null)
            return NotFound(new ApiError(false, "Aktivite bulunamadı"));

        _db.SchoolActivities.Remove(activity);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Aktivite kaldırıldı"));
    }
}
