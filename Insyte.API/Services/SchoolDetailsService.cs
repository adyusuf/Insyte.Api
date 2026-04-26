using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SchoolServiceEnum = Insyte.Core.Enums.SchoolService;

namespace Insyte.API.Services;

public class SchoolDetailsService : ISchoolDetailsService
{
    private readonly InsyteDbContext _db;

    public SchoolDetailsService(InsyteDbContext db) => _db = db;

    // Facilities
    public async Task<(bool SchoolExists, List<SchoolFacilityDto>? Facilities)> GetFacilitiesAsync(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

        var facilities = await _db.SchoolFacilities
            .Where(f => f.SchoolId == schoolId)
            .OrderBy(f => f.Facility)
            .Select(f => new SchoolFacilityDto(f.Id, f.SchoolId, f.Facility, f.CreatedAt))
            .ToListAsync();

        return (true, facilities);
    }

    public async Task<(bool Success, string? Error, SchoolFacilityDto? Facility)> AddFacilityAsync(Guid schoolId, PhysicalFacility facility)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null);

        if (await _db.SchoolFacilities.AnyAsync(f => f.SchoolId == schoolId && f.Facility == facility))
            return (false, "Bu imkan zaten eklenmiş", null);

        var entity = new SchoolFacility { SchoolId = schoolId, Facility = facility };
        _db.SchoolFacilities.Add(entity);
        await _db.SaveChangesAsync();

        return (true, null, new SchoolFacilityDto(entity.Id, entity.SchoolId, entity.Facility, entity.CreatedAt));
    }

    public async Task<(bool Success, string? Error)> RemoveFacilityAsync(Guid schoolId, Guid facilityId)
    {
        var facility = await _db.SchoolFacilities.FirstOrDefaultAsync(f => f.Id == facilityId && f.SchoolId == schoolId);
        if (facility == null) return (false, "Fiziksel imkan bulunamadı");

        _db.SchoolFacilities.Remove(facility);
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Services
    public async Task<(bool SchoolExists, List<SchoolServiceDto>? Services)> GetServicesAsync(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

        var services = await _db.SchoolServices
            .Where(s => s.SchoolId == schoolId)
            .OrderBy(s => s.Service)
            .Select(s => new SchoolServiceDto(s.Id, s.SchoolId, s.Service, s.CreatedAt))
            .ToListAsync();

        return (true, services);
    }

    public async Task<(bool Success, string? Error, SchoolServiceDto? Service)> AddServiceAsync(Guid schoolId, Insyte.Core.Enums.SchoolService service)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null);

        if (await _db.SchoolServices.AnyAsync(s => s.SchoolId == schoolId && s.Service == service))
            return (false, "Bu hizmet zaten eklenmiş", null);

        var entity = new SchoolServiceOffering { SchoolId = schoolId, Service = service };
        _db.SchoolServices.Add(entity);
        await _db.SaveChangesAsync();

        return (true, null, new SchoolServiceDto(entity.Id, entity.SchoolId, entity.Service, entity.CreatedAt));
    }

    public async Task<(bool Success, string? Error)> RemoveServiceAsync(Guid schoolId, Guid serviceId)
    {
        var service = await _db.SchoolServices.FirstOrDefaultAsync(s => s.Id == serviceId && s.SchoolId == schoolId);
        if (service == null) return (false, "Hizmet bulunamadı");

        _db.SchoolServices.Remove(service);
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Activities
    public async Task<(bool SchoolExists, List<SchoolActivityDto>? Activities)> GetActivitiesAsync(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

        var activities = await _db.SchoolActivities
            .Where(a => a.SchoolId == schoolId)
            .OrderBy(a => a.Activity)
            .Select(a => new SchoolActivityDto(a.Id, a.SchoolId, a.Activity, a.CreatedAt))
            .ToListAsync();

        return (true, activities);
    }

    public async Task<(bool Success, string? Error, SchoolActivityDto? Activity)> AddActivityAsync(Guid schoolId, Activity activity)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null);

        if (await _db.SchoolActivities.AnyAsync(a => a.SchoolId == schoolId && a.Activity == activity))
            return (false, "Bu aktivite zaten eklenmiş", null);

        var entity = new SchoolActivity { SchoolId = schoolId, Activity = activity };
        _db.SchoolActivities.Add(entity);
        await _db.SaveChangesAsync();

        return (true, null, new SchoolActivityDto(entity.Id, entity.SchoolId, entity.Activity, entity.CreatedAt));
    }

    public async Task<(bool Success, string? Error)> RemoveActivityAsync(Guid schoolId, Guid activityId)
    {
        var activity = await _db.SchoolActivities.FirstOrDefaultAsync(a => a.Id == activityId && a.SchoolId == schoolId);
        if (activity == null) return (false, "Aktivite bulunamadı");

        _db.SchoolActivities.Remove(activity);
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Languages
    public async Task<(bool SchoolExists, List<SchoolLanguageDto>? Languages)> GetLanguagesAsync(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, null);

        var languages = await _db.SchoolLanguages
            .Where(l => l.SchoolId == schoolId)
            .OrderBy(l => l.Language)
            .Select(l => new SchoolLanguageDto(l.Id, l.SchoolId, l.Language, l.CreatedAt))
            .ToListAsync();

        return (true, languages);
    }

    public async Task<(bool Success, string? Error, SchoolLanguageDto? Language)> AddLanguageAsync(Guid schoolId, ForeignLanguage language)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return (false, "Okul bulunamadı", null);

        if (await _db.SchoolLanguages.AnyAsync(l => l.SchoolId == schoolId && l.Language == language))
            return (false, "Bu dil zaten eklenmiş", null);

        var entity = new SchoolLanguage { SchoolId = schoolId, Language = language };
        _db.SchoolLanguages.Add(entity);
        await _db.SaveChangesAsync();

        return (true, null, new SchoolLanguageDto(entity.Id, entity.SchoolId, entity.Language, entity.CreatedAt));
    }

    public async Task<(bool Success, string? Error)> RemoveLanguageAsync(Guid schoolId, Guid languageId)
    {
        var language = await _db.SchoolLanguages.FirstOrDefaultAsync(l => l.Id == languageId && l.SchoolId == schoolId);
        if (language == null) return (false, "Dil bulunamadı");

        _db.SchoolLanguages.Remove(language);
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
