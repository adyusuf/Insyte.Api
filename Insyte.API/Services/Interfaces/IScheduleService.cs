using Insyte.API.DTOs;
using Insyte.Core.Enums;

namespace Insyte.API.Services.Interfaces;

public interface IScheduleService
{
    Task<(bool SchoolExists, PagedResult<ScheduleDto>? Result)> GetAllAsync(Guid schoolId, Guid? classId, int page, int pageSize);
    Task<ScheduleDto?> GetByIdAsync(Guid id, Guid schoolId);
    Task<(bool Success, string? Error, ScheduleDto? Schedule, List<ScheduleConflict>? Conflicts)> CreateAsync(Guid schoolId, CreateScheduleRequest request);
    Task<(bool Success, string? Error, List<ScheduleConflict>? Conflicts)> UpdateAsync(Guid id, Guid schoolId, UpdateScheduleRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid schoolId);
    Task<(bool SchoolExists, ScheduleConflictResponse? Response)> CheckConflictsAsync(Guid schoolId, CheckScheduleConflictRequest request);
}
