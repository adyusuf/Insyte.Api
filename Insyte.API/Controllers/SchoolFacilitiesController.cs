using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/facilities")]
[Authorize]
public class SchoolFacilitiesController : BaseController
{
    private readonly ISchoolDetailsService _detailsService;

    public SchoolFacilitiesController(ISchoolDetailsService detailsService) => _detailsService = detailsService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        var (schoolExists, facilities) = await _detailsService.GetFacilitiesAsync(schoolId);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<List<SchoolFacilityDto>>(true, facilities));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolFacilityRequest request)
    {
        var (success, error, facility) = await _detailsService.AddFacilityAsync(schoolId, request.Facility);
        if (!success)
        {
            if (error == "Okul bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<SchoolFacilityDto>(true, facility, "Fiziksel imkan eklendi"));
    }

    [HttpDelete("{facilityId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid facilityId)
    {
        var (success, error) = await _detailsService.RemoveFacilityAsync(schoolId, facilityId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Fiziksel imkan kaldırıldı"));
    }
}
