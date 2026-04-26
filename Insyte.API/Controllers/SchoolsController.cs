using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchoolsController : BaseController
{
    private readonly ISchoolService _schoolService;

    public SchoolsController(ISchoolService schoolService) => _schoolService = schoolService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _schoolService.GetAllAsync(search, page, pageSize);
        return Ok(new ApiResponse<PagedResult<SchoolDto>>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var school = await _schoolService.GetByIdAsync(id);
        if (school == null) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<SchoolDto>(true, school));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAdvisor")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] string name, [FromForm] string? address, [FromForm] string? city,
        [FromForm] string? phone, [FromForm] string? email, [FromForm] string? website,
        [FromForm] string? schoolType, [FromForm] string? institutionType,
        [FromForm] string? liseType, [FromForm] string? educationSystem,
        [FromForm] double? latitude, [FromForm] double? longitude, [FromForm] IFormFile? logo)
    {
        var school = await _schoolService.CreateAsync(
            name, address, city, phone, email, website,
            schoolType, institutionType, liseType, educationSystem,
            latitude, longitude, logo);

        return CreatedAtAction(nameof(GetById), new { id = school.Id },
            new ApiResponse<SchoolDto>(true, school));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSchoolRequest request)
    {
        var (success, error) = await _schoolService.UpdateAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Okul güncellendi"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _schoolService.DeleteAsync(id);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Okul deaktif edildi"));
    }

    // --- Advisors ---
    [HttpGet("{id}/advisors")]
    public async Task<IActionResult> GetAdvisors(Guid id)
    {
        var advisors = await _schoolService.GetAdvisorsAsync(id);
        return Ok(new ApiResponse<List<SchoolTeacherDto>>(true, advisors));
    }

    [HttpPost("{id}/advisors")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AssignAdvisor(Guid id, [FromBody] AssignAdvisorRequest request)
    {
        var (success, error) = await _schoolService.AssignAdvisorAsync(id, request.UserId);
        if (!success)
        {
            if (error == "Okul bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<object>(true, null, "Danışman atandı"));
    }

    [HttpDelete("{id}/advisors/{advisorId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RemoveAdvisor(Guid id, Guid advisorId)
    {
        var (success, error) = await _schoolService.RemoveAdvisorAsync(id, advisorId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Danışman kaldırıldı"));
    }

    // --- Teachers / School Users ---
    [HttpGet("{id}/teachers")]
    public async Task<IActionResult> GetTeachers(Guid id)
    {
        var teachers = await _schoolService.GetTeachersAsync(id);
        return Ok(new ApiResponse<List<SchoolTeacherDto>>(true, teachers));
    }

    [HttpPost("{id}/teachers")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> AssignTeacher(Guid id, [FromBody] AssignTeacherRequest request)
    {
        var (success, error) = await _schoolService.AssignTeacherAsync(id, request.UserId, request.Role);
        if (!success)
        {
            if (error == "Okul bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<object>(true, null, "Öğretmen atandı"));
    }

    [HttpDelete("{id}/teachers/{teacherId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> RemoveTeacher(Guid id, Guid teacherId)
    {
        var (success, error) = await _schoolService.RemoveTeacherAsync(id, teacherId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Öğretmen kaldırıldı"));
    }

    // --- Email Config ---
    [HttpGet("{id}/email-config")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> GetEmailConfig(Guid id)
    {
        var (_, configs) = await _schoolService.GetEmailConfigsAsync(id);
        return Ok(new ApiResponse<List<EmailConfigDto>>(true, configs));
    }

    [HttpPost("{id}/email-config")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> AddEmailConfig(Guid id, [FromBody] CreateEmailConfigRequest request)
    {
        var (success, error) = await _schoolService.AddEmailConfigAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "E-posta yapılandırması eklendi"));
    }

    [HttpDelete("{id}/email-config/{configId}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> RemoveEmailConfig(Guid id, Guid configId)
    {
        var (success, error) = await _schoolService.RemoveEmailConfigAsync(id, configId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "E-posta yapılandırması kaldırıldı"));
    }
}
