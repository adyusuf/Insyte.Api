using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : BaseController
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService) => _teacherService = teacherService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] Guid? schoolId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _teacherService.GetAllAsync(search, schoolId, page, pageSize);
        return Ok(new ApiResponse<PagedResult<UserDto>>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var teacher = await _teacherService.GetByIdAsync(id);
        if (teacher == null) return NotFound(new ApiError(false, "Öğretmen bulunamadı"));

        return Ok(new ApiResponse<UserDto>(true, teacher));
    }
}
