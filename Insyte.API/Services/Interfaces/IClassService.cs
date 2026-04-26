using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface IClassService
{
    Task<(bool SchoolExists, PagedResult<ClassDto>? Result)> GetAllAsync(Guid schoolId, string? search, int page, int pageSize);
    Task<ClassDto?> GetByIdAsync(Guid id, Guid schoolId);
    Task<(bool Success, string? Error, ClassDto? Class)> CreateAsync(Guid schoolId, CreateClassRequest request);
    Task<(bool Success, string? Error)> UpdateAsync(Guid id, Guid schoolId, UpdateClassRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid schoolId);
}
