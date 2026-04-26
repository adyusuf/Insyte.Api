using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/schedules")]
[Authorize]
public class SchedulesController : BaseController
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService) => _scheduleService = scheduleService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId, [FromQuery] Guid? classId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (schoolExists, result) = await _scheduleService.GetAllAsync(schoolId, classId, page, pageSize);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<PagedResult<ScheduleDto>>(true, result));
    }

    [HttpGet("{scheduleId}")]
    public async Task<IActionResult> GetById(Guid schoolId, Guid scheduleId)
    {
        var schedule = await _scheduleService.GetByIdAsync(scheduleId, schoolId);
        if (schedule == null) return NotFound(new ApiError(false, "Ders planı bulunamadı"));

        return Ok(new ApiResponse<ScheduleDto>(true, schedule));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create(Guid schoolId, [FromBody] CreateScheduleRequest request)
    {
        var (success, error, schedule, conflicts) = await _scheduleService.CreateAsync(schoolId, request);
        if (!success)
        {
            if (conflicts != null && conflicts.Any())
                return BadRequest(new { Success = false, Message = error, Conflicts = conflicts.Select(c => c.ConflictDescription).ToList() });
            return NotFound(new ApiError(false, error!));
        }

        return CreatedAtAction(nameof(GetById), new { schoolId, scheduleId = schedule!.Id },
            new ApiResponse<ScheduleDto>(true, schedule));
    }

    [HttpPut("{scheduleId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid schoolId, Guid scheduleId, [FromBody] UpdateScheduleRequest request)
    {
        var (success, error, conflicts) = await _scheduleService.UpdateAsync(scheduleId, schoolId, request);
        if (!success)
        {
            if (conflicts != null && conflicts.Any())
                return BadRequest(new { Success = false, Message = error, Conflicts = conflicts.Select(c => c.ConflictDescription).ToList() });
            return NotFound(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<object>(true, null, "Ders planı güncellendi"));
    }

    [HttpDelete("{scheduleId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Delete(Guid schoolId, Guid scheduleId)
    {
        var (success, error) = await _scheduleService.DeleteAsync(scheduleId, schoolId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Ders planı deaktif edildi"));
    }

    [HttpPost("check-conflicts")]
    public async Task<IActionResult> CheckConflicts(Guid schoolId, [FromBody] CheckScheduleConflictRequest request)
    {
        var (schoolExists, response) = await _scheduleService.CheckConflictsAsync(schoolId, request);
        if (!schoolExists) return NotFound(new ApiError(false, "Okul bulunamadı"));

        return Ok(new ApiResponse<ScheduleConflictResponse>(true, response));
    }
}
