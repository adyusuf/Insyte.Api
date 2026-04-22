using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/schedules")]
[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public SchedulesController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId, [FromQuery] Guid? classId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var query = _db.Schedules.Where(s => s.SchoolId == schoolId);

        if (classId.HasValue)
            query = query.Where(s => s.ClassId == classId.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ScheduleDto(s.Id, s.SchoolId, s.ClassId, s.SubjectId, s.TeacherUserId, s.DayOfWeek, s.StartTime, s.EndTime, s.Room, s.Notes, s.IsActive, s.CreatedAt, s.UpdatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<ScheduleDto>>(true, new PagedResult<ScheduleDto>(items, total, page, pageSize)));
    }

    [HttpGet("{scheduleId}")]
    public async Task<IActionResult> GetById(Guid schoolId, Guid scheduleId)
    {
        var schedule = await _db.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId && s.SchoolId == schoolId);
        if (schedule == null) return NotFound(new ApiError(false, "Ders planı bulunamadı"));

        var dto = new ScheduleDto(schedule.Id, schedule.SchoolId, schedule.ClassId, schedule.SubjectId, schedule.TeacherUserId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime, schedule.Room, schedule.Notes, schedule.IsActive, schedule.CreatedAt, schedule.UpdatedAt);
        return Ok(new ApiResponse<ScheduleDto>(true, dto));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Create(Guid schoolId, [FromBody] CreateScheduleRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        // Verify references exist
        if (!await _db.Classes.AnyAsync(c => c.Id == request.ClassId && c.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Sınıf bulunamadı"));

        if (!await _db.Subjects.AnyAsync(s => s.Id == request.SubjectId && s.SchoolId == schoolId))
            return NotFound(new ApiError(false, "Ders bulunamadı"));

        if (!await _db.Users.AnyAsync(u => u.Id == request.TeacherUserId))
            return NotFound(new ApiError(false, "Öğretmen bulunamadı"));

        // Check for conflicts
        var conflicts = await CheckScheduleConflicts(request.ClassId, request.DayOfWeek, request.StartTime, request.EndTime, null);
        if (conflicts.Any())
        {
            var conflictDescriptions = conflicts.Select(c => c.ConflictDescription).ToList();
            return BadRequest(new { Success = false, Message = "Ders planı çakışması bulundu", Conflicts = conflictDescriptions });
        }

        var schedule = new Schedule
        {
            SchoolId = schoolId,
            ClassId = request.ClassId,
            SubjectId = request.SubjectId,
            TeacherUserId = request.TeacherUserId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Room = request.Room,
            Notes = request.Notes
        };

        _db.Schedules.Add(schedule);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { schoolId, scheduleId = schedule.Id },
            new ApiResponse<ScheduleDto>(true, new ScheduleDto(schedule.Id, schedule.SchoolId, schedule.ClassId, schedule.SubjectId, schedule.TeacherUserId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime, schedule.Room, schedule.Notes, schedule.IsActive, schedule.CreatedAt, schedule.UpdatedAt)));
    }

    [HttpPut("{scheduleId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Update(Guid schoolId, Guid scheduleId, [FromBody] UpdateScheduleRequest request)
    {
        var schedule = await _db.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId && s.SchoolId == schoolId);
        if (schedule == null) return NotFound(new ApiError(false, "Ders planı bulunamadı"));

        // If updating time-based fields, check for conflicts
        if (request.ClassId.HasValue || request.DayOfWeek.HasValue || request.StartTime.HasValue || request.EndTime.HasValue)
        {
            var classId = request.ClassId ?? schedule.ClassId;
            var dayOfWeek = request.DayOfWeek ?? schedule.DayOfWeek;
            var startTime = request.StartTime ?? schedule.StartTime;
            var endTime = request.EndTime ?? schedule.EndTime;

            var conflicts = await CheckScheduleConflicts(classId, dayOfWeek, startTime, endTime, scheduleId);
            if (conflicts.Any())
            {
                var conflictDescriptions = conflicts.Select(c => c.ConflictDescription).ToList();
                return BadRequest(new { Success = false, Message = "Ders planı çakışması bulundu", Conflicts = conflictDescriptions });
            }
        }

        if (request.ClassId.HasValue) schedule.ClassId = request.ClassId.Value;
        if (request.SubjectId.HasValue) schedule.SubjectId = request.SubjectId.Value;
        if (request.TeacherUserId.HasValue) schedule.TeacherUserId = request.TeacherUserId.Value;
        if (request.DayOfWeek.HasValue) schedule.DayOfWeek = request.DayOfWeek.Value;
        if (request.StartTime.HasValue) schedule.StartTime = request.StartTime.Value;
        if (request.EndTime.HasValue) schedule.EndTime = request.EndTime.Value;
        if (request.Room != null) schedule.Room = request.Room;
        if (request.Notes != null) schedule.Notes = request.Notes;
        if (request.IsActive.HasValue) schedule.IsActive = request.IsActive.Value;
        schedule.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Ders planı güncellendi"));
    }

    [HttpDelete("{scheduleId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Delete(Guid schoolId, Guid scheduleId)
    {
        var schedule = await _db.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId && s.SchoolId == schoolId);
        if (schedule == null) return NotFound(new ApiError(false, "Ders planı bulunamadı"));

        schedule.IsActive = false;
        schedule.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Ders planı deaktif edildi"));
    }

    [HttpPost("check-conflicts")]
    public async Task<IActionResult> CheckConflicts(Guid schoolId, [FromBody] CheckScheduleConflictRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var conflicts = await CheckScheduleConflicts(request.ClassId, request.DayOfWeek, request.StartTime, request.EndTime, null);

        return Ok(new ApiResponse<ScheduleConflictResponse>(true, new ScheduleConflictResponse(conflicts.Any(), conflicts)));
    }

    private async Task<List<ScheduleConflict>> CheckScheduleConflicts(Guid classId, Insyte.Core.Enums.ScheduleDay dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? excludeScheduleId = null)
    {
        var conflicts = new List<ScheduleConflict>();

        // Get existing schedules for the same class on the same day
        var existingSchedules = await _db.Schedules
            .Where(s => s.ClassId == classId && s.DayOfWeek == dayOfWeek && s.IsActive)
            .ToListAsync();

        if (excludeScheduleId.HasValue)
            existingSchedules = existingSchedules.Where(s => s.Id != excludeScheduleId.Value).ToList();

        foreach (var existing in existingSchedules)
        {
            // Check if times overlap
            if (TimeSpansOverlap(startTime, endTime, existing.StartTime, existing.EndTime))
            {
                var conflictDescription = $"Sınıf {dayOfWeek} günü {existing.StartTime:hh\\:mm}-{existing.EndTime:hh\\:mm} arasında dolu";
                conflicts.Add(new ScheduleConflict(existing.Id, Guid.Empty, conflictDescription));
            }
        }

        return conflicts;
    }

    private bool TimeSpansOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
    {
        return start1 < end2 && start2 < end1;
    }
}
