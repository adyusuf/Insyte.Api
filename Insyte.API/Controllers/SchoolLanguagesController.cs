using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/languages")]
[Authorize]
public class SchoolLanguagesController : BaseController
{
    private readonly ISchoolDetailsService _detailsService;

    public SchoolLanguagesController(ISchoolDetailsService detailsService) => _detailsService = detailsService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        var (schoolExists, languages) = await _detailsService.GetLanguagesAsync(schoolId);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<List<SchoolLanguageDto>>(true, languages));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolLanguageRequest request)
    {
        var (success, error, language) = await _detailsService.AddLanguageAsync(schoolId, request.Language);
        if (!success)
        {
            if (error == "Okul bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<SchoolLanguageDto>(true, language, "Dil eklendi"));
    }

    [HttpDelete("{languageId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid languageId)
    {
        var (success, error) = await _detailsService.RemoveLanguageAsync(schoolId, languageId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Dil kaldırıldı"));
    }
}
