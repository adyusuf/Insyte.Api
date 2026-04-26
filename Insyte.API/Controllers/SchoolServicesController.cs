using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolServiceEnum = Insyte.Core.Enums.SchoolService;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/services")]
[Authorize]
public class SchoolServicesController : BaseController
{
    private readonly ISchoolDetailsService _detailsService;

    public SchoolServicesController(ISchoolDetailsService detailsService) => _detailsService = detailsService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        var (schoolExists, services) = await _detailsService.GetServicesAsync(schoolId);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<List<SchoolServiceDto>>(true, services));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolServiceRequest request)
    {
        var (success, error, service) = await _detailsService.AddServiceAsync(schoolId, (SchoolServiceEnum)request.Service);
        if (!success)
        {
            if (error == "Okul bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<SchoolServiceDto>(true, service, "Hizmet eklendi"));
    }

    [HttpDelete("{serviceId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid serviceId)
    {
        var (success, error) = await _detailsService.RemoveServiceAsync(schoolId, serviceId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Hizmet kaldırıldı"));
    }
}
