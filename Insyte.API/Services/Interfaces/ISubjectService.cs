using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface ISubjectService
{
    Task<(bool SchoolExists, PagedResult<SubjectDto>? Result)> GetAllAsync(Guid schoolId, string? search, int page, int pageSize);
    Task<SubjectDto?> GetByIdAsync(Guid id, Guid schoolId);
    Task<(bool Success, string? Error, SubjectDto? Subject)> CreateAsync(Guid schoolId, CreateSubjectRequest request);
    Task<(bool Success, string? Error)> UpdateAsync(Guid id, Guid schoolId, UpdateSubjectRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid schoolId);
}
