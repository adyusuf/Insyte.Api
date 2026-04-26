using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/subjects")]
[Authorize]
public class SubjectsController : BaseController
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService) => _subjectService = subjectService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (schoolExists, result) = await _subjectService.GetAllAsync(schoolId, search, page, pageSize);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<PagedResult<SubjectDto>>(true, result));
    }

    [HttpGet("{subjectId}")]
    public async Task<IActionResult> GetById(Guid schoolId, Guid subjectId)
    {
        var subject = await _subjectService.GetByIdAsync(subjectId, schoolId);
        if (subject == null) return NotFound(new ApiError(false, "Ders bulunamadı"));

        return Ok(new ApiResponse<SubjectDto>(true, subject));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create(Guid schoolId, [FromBody] CreateSubjectRequest request)
    {
        var (success, error, subject) = await _subjectService.CreateAsync(schoolId, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return CreatedAtAction(nameof(GetById), new { schoolId, subjectId = subject!.Id },
            new ApiResponse<SubjectDto>(true, subject));
    }

    [HttpPut("{subjectId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid schoolId, Guid subjectId, [FromBody] UpdateSubjectRequest request)
    {
        var (success, error) = await _subjectService.UpdateAsync(subjectId, schoolId, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Ders güncellendi"));
    }

    [HttpDelete("{subjectId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Delete(Guid schoolId, Guid subjectId)
    {
        var (success, error) = await _subjectService.DeleteAsync(subjectId, schoolId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Ders deaktif edildi"));
    }
}
