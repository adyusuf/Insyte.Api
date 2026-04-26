using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/classes")]
[Authorize]
public class ClassesController : BaseController
{
    private readonly IClassService _classService;

    public ClassesController(IClassService classService) => _classService = classService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (schoolExists, result) = await _classService.GetAllAsync(schoolId, search, page, pageSize);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<PagedResult<ClassDto>>(true, result));
    }

    [HttpGet("{classId}")]
    public async Task<IActionResult> GetById(Guid schoolId, Guid classId)
    {
        var classroom = await _classService.GetByIdAsync(classId, schoolId);
        if (classroom == null) return NotFound(new ApiError(false, "Sınıf bulunamadı"));

        return Ok(new ApiResponse<ClassDto>(true, classroom));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create(Guid schoolId, [FromBody] CreateClassRequest request)
    {
        var (success, error, classroom) = await _classService.CreateAsync(schoolId, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return CreatedAtAction(nameof(GetById), new { schoolId, classId = classroom!.Id },
            new ApiResponse<ClassDto>(true, classroom));
    }

    [HttpPut("{classId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid schoolId, Guid classId, [FromBody] UpdateClassRequest request)
    {
        var (success, error) = await _classService.UpdateAsync(classId, schoolId, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Sınıf güncellendi"));
    }

    [HttpDelete("{classId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Delete(Guid schoolId, Guid classId)
    {
        var (success, error) = await _classService.DeleteAsync(classId, schoolId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Sınıf deaktif edildi"));
    }
}
