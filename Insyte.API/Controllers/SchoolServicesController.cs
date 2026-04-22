using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/services")]
[Authorize]
public class SchoolServicesController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public SchoolServicesController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var services = await _db.SchoolServices
            .Where(s => s.SchoolId == schoolId)
            .OrderBy(s => s.Service)
            .Select(s => new SchoolServiceDto(s.Id, s.SchoolId, s.Service, s.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<SchoolServiceDto>>(true, services));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolServiceRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (await _db.SchoolServices.AnyAsync(s => s.SchoolId == schoolId && s.Service == request.Service))
            return BadRequest(new ApiError(false, "Bu hizmet zaten eklenmiş"));

        var service = new SchoolServiceOffering
        {
            SchoolId = schoolId,
            Service = request.Service
        };

        _db.SchoolServices.Add(service);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<SchoolServiceDto>(true, new SchoolServiceDto(service.Id, service.SchoolId, service.Service, service.CreatedAt), "Hizmet eklendi"));
    }

    [HttpDelete("{serviceId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid serviceId)
    {
        var service = await _db.SchoolServices.FirstOrDefaultAsync(s => s.Id == serviceId && s.SchoolId == schoolId);
        if (service == null)
            return NotFound(new ApiError(false, "Hizmet bulunamadı"));

        _db.SchoolServices.Remove(service);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Hizmet kaldırıldı"));
    }
}
