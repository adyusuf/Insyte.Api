using Insyte.API.DTOs;
using Insyte.Core.Enums;

namespace Insyte.API.Services.Interfaces;

public interface ISchoolDetailsService
{
    // Facilities
    Task<(bool SchoolExists, List<SchoolFacilityDto>? Facilities)> GetFacilitiesAsync(Guid schoolId);
    Task<(bool Success, string? Error, SchoolFacilityDto? Facility)> AddFacilityAsync(Guid schoolId, PhysicalFacility facility);
    Task<(bool Success, string? Error)> RemoveFacilityAsync(Guid schoolId, Guid facilityId);

    // Services
    Task<(bool SchoolExists, List<SchoolServiceDto>? Services)> GetServicesAsync(Guid schoolId);
    Task<(bool Success, string? Error, SchoolServiceDto? Service)> AddServiceAsync(Guid schoolId, Insyte.Core.Enums.SchoolService service);
    Task<(bool Success, string? Error)> RemoveServiceAsync(Guid schoolId, Guid serviceId);

    // Activities
    Task<(bool SchoolExists, List<SchoolActivityDto>? Activities)> GetActivitiesAsync(Guid schoolId);
    Task<(bool Success, string? Error, SchoolActivityDto? Activity)> AddActivityAsync(Guid schoolId, Activity activity);
    Task<(bool Success, string? Error)> RemoveActivityAsync(Guid schoolId, Guid activityId);

    // Languages
    Task<(bool SchoolExists, List<SchoolLanguageDto>? Languages)> GetLanguagesAsync(Guid schoolId);
    Task<(bool Success, string? Error, SchoolLanguageDto? Language)> AddLanguageAsync(Guid schoolId, ForeignLanguage language);
    Task<(bool Success, string? Error)> RemoveLanguageAsync(Guid schoolId, Guid languageId);
}
