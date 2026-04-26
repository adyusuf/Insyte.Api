using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class ScheduleService : IScheduleService
{
    private readonly InsyteDbContext _db;

    public ScheduleService(InsyteDbContext db) => _db = db;

    public async Task<(bool SchoolExists, PagedResult<ScheduleDto>? Result)> GetAllAsync(Guid schoolId, Guid? classId, int page, int pageSize)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

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

        return (true, new PagedResult<ScheduleDto>(items, total, page, pageSize));
    }

    public async Task<ScheduleDto?> GetByIdAsync(Guid id, Guid schoolId)
    {
        var schedule = await _db.Schedules.FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId);
        if (schedule == null) return null;

        return new ScheduleDto(schedule.Id, schedule.SchoolId, schedule.ClassId, schedule.SubjectId, schedule.TeacherUserId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime, schedule.Room, schedule.Notes, schedule.IsActive, schedule.CreatedAt, schedule.UpdatedAt);
    }

    public async Task<(bool Success, string? Error, ScheduleDto? Schedule, List<ScheduleConflict>? Conflicts)> CreateAsync(Guid schoolId, CreateScheduleRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null, null);

        if (!await _db.Classes.AnyAsync(c => c.Id == request.ClassId && c.SchoolId == schoolId))
            return (false, "Sınıf bulunamadı", null, null);

        if (!await _db.Subjects.AnyAsync(s => s.Id == request.SubjectId && s.SchoolId == schoolId))
            return (false, "Ders bulunamadı", null, null);

        if (!await _db.Users.AnyAsync(u => u.Id == request.TeacherUserId))
            return (false, "Öğretmen bulunamadı", null, null);

        var conflicts = await CheckScheduleConflictsAsync(request.ClassId, request.DayOfWeek, request.StartTime, request.EndTime, null);
        if (conflicts.Any())
            return (false, "Ders planı çakışması bulundu", null, conflicts);

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

        return (true, null, new ScheduleDto(schedule.Id, schedule.SchoolId, schedule.ClassId, schedule.SubjectId, schedule.TeacherUserId, schedule.DayOfWeek, schedule.StartTime, schedule.EndTime, schedule.Room, schedule.Notes, schedule.IsActive, schedule.CreatedAt, schedule.UpdatedAt), null);
    }

    public async Task<(bool Success, string? Error, List<ScheduleConflict>? Conflicts)> UpdateAsync(Guid id, Guid schoolId, UpdateScheduleRequest request)
    {
        var schedule = await _db.Schedules.FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId);
        if (schedule == null) return (false, "Ders planı bulunamadı", null);

        // Çakışma kontrolü — zaman bazlı alanlar güncelleniyorsa
        if (request.ClassId.HasValue || request.DayOfWeek.HasValue || request.StartTime.HasValue || request.EndTime.HasValue)
        {
            var classId = request.ClassId ?? schedule.ClassId;
            var dayOfWeek = request.DayOfWeek ?? schedule.DayOfWeek;
            var startTime = request.StartTime ?? schedule.StartTime;
            var endTime = request.EndTime ?? schedule.EndTime;

            var conflicts = await CheckScheduleConflictsAsync(classId, dayOfWeek, startTime, endTime, id);
            if (conflicts.Any())
                return (false, "Ders planı çakışması bulundu", conflicts);
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
        return (true, null, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid schoolId)
    {
        var schedule = await _db.Schedules.FirstOrDefaultAsync(s => s.Id == id && s.SchoolId == schoolId);
        if (schedule == null) return (false, "Ders planı bulunamadı");

        schedule.IsActive = false;
        schedule.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool SchoolExists, ScheduleConflictResponse? Response)> CheckConflictsAsync(Guid schoolId, CheckScheduleConflictRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

        var conflicts = await CheckScheduleConflictsAsync(request.ClassId, request.DayOfWeek, request.StartTime, request.EndTime, null);
        return (true, new ScheduleConflictResponse(conflicts.Any(), conflicts));
    }

    private async Task<List<ScheduleConflict>> CheckScheduleConflictsAsync(Guid classId, ScheduleDay dayOfWeek, TimeSpan startTime, TimeSpan endTime, Guid? excludeScheduleId)
    {
        var conflicts = new List<ScheduleConflict>();

        var existingSchedules = await _db.Schedules
            .Where(s => s.ClassId == classId && s.DayOfWeek == dayOfWeek && s.IsActive)
            .ToListAsync();

        if (excludeScheduleId.HasValue)
            existingSchedules = existingSchedules.Where(s => s.Id != excludeScheduleId.Value).ToList();

        foreach (var existing in existingSchedules)
        {
            if (TimeSpansOverlap(startTime, endTime, existing.StartTime, existing.EndTime))
            {
                var conflictDescription = $"Sınıf {dayOfWeek} günü {existing.StartTime:hh\\:mm}-{existing.EndTime:hh\\:mm} arasında dolu";
                conflicts.Add(new ScheduleConflict(existing.Id, Guid.Empty, conflictDescription));
            }
        }

        return conflicts;
    }

    private static bool TimeSpansOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
    {
        return start1 < end2 && start2 < end1;
    }
}
