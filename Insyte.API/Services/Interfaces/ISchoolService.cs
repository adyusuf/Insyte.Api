using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface ISchoolService
{
    Task<PagedResult<SchoolDto>> GetAllAsync(string? search, int page, int pageSize);
    Task<SchoolDto?> GetByIdAsync(Guid id);
    Task<SchoolDto> CreateAsync(
        string name, string? address, string? city, string? phone, string? email,
        string? website, string? schoolType, string? institutionType, string? liseType,
        string? educationSystem, double? latitude, double? longitude, IFormFile? logo);
    Task<(bool Success, string? Error)> UpdateAsync(Guid id, UpdateSchoolRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id);

    // Advisors
    Task<List<SchoolTeacherDto>> GetAdvisorsAsync(Guid schoolId);
    Task<(bool Success, string? Error)> AssignAdvisorAsync(Guid schoolId, Guid userId);
    Task<(bool Success, string? Error)> RemoveAdvisorAsync(Guid schoolId, Guid advisorId);

    // Teachers
    Task<List<SchoolTeacherDto>> GetTeachersAsync(Guid schoolId);
    Task<(bool Success, string? Error)> AssignTeacherAsync(Guid schoolId, Guid userId, Insyte.Core.Enums.UserRole role);
    Task<(bool Success, string? Error)> RemoveTeacherAsync(Guid schoolId, Guid teacherId);

    // Email Config
    Task<(bool Exists, List<EmailConfigDto> Configs)> GetEmailConfigsAsync(Guid schoolId);
    Task<(bool Success, string? Error)> AddEmailConfigAsync(Guid schoolId, CreateEmailConfigRequest request);
    Task<(bool Success, string? Error)> RemoveEmailConfigAsync(Guid schoolId, Guid configId);
}
