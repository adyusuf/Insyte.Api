using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/activities")]
[Authorize]
public class SchoolActivitiesController : BaseController
{
    private readonly ISchoolDetailsService _detailsService;

    public SchoolActivitiesController(ISchoolDetailsService detailsService) => _detailsService = detailsService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        var (schoolExists, activities) = await _detailsService.GetActivitiesAsync(schoolId);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<List<SchoolActivityDto>>(true, activities));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolActivityRequest request)
    {
        var (success, error, activity) = await _detailsService.AddActivityAsync(schoolId, request.Activity);
        if (!success)
        {
            if (error == "Okul bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<SchoolActivityDto>(true, activity, "Aktivite eklendi"));
    }

    [HttpDelete("{activityId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid activityId)
    {
        var (success, error) = await _detailsService.RemoveActivityAsync(schoolId, activityId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Aktivite kaldırıldı"));
    }
}
