using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/facilities")]
[Authorize]
public class SchoolFacilitiesController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public SchoolFacilitiesController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var facilities = await _db.SchoolFacilities
            .Where(f => f.SchoolId == schoolId)
            .OrderBy(f => f.Facility)
            .Select(f => new SchoolFacilityDto(f.Id, f.SchoolId, f.Facility, f.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<SchoolFacilityDto>>(true, facilities));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolFacilityRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (await _db.SchoolFacilities.AnyAsync(f => f.SchoolId == schoolId && f.Facility == request.Facility))
            return BadRequest(new ApiError(false, "Bu imkan zaten eklenmiş"));

        var facility = new SchoolFacility
        {
            SchoolId = schoolId,
            Facility = request.Facility
        };

        _db.SchoolFacilities.Add(facility);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<SchoolFacilityDto>(true, new SchoolFacilityDto(facility.Id, facility.SchoolId, facility.Facility, facility.CreatedAt), "Fiziksel imkan eklendi"));
    }

    [HttpDelete("{facilityId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid facilityId)
    {
        var facility = await _db.SchoolFacilities.FirstOrDefaultAsync(f => f.Id == facilityId && f.SchoolId == schoolId);
        if (facility == null)
            return NotFound(new ApiError(false, "Fiziksel imkan bulunamadı"));

        _db.SchoolFacilities.Remove(facility);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Fiziksel imkan kaldırıldı"));
    }
}
