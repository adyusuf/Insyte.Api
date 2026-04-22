using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/languages")]
[Authorize]
public class SchoolLanguagesController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public SchoolLanguagesController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid schoolId)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        var languages = await _db.SchoolLanguages
            .Where(l => l.SchoolId == schoolId)
            .OrderBy(l => l.Language)
            .Select(l => new SchoolLanguageDto(l.Id, l.SchoolId, l.Language, l.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<SchoolLanguageDto>>(true, languages));
    }

    [HttpPost]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Add(Guid schoolId, [FromBody] AddSchoolLanguageRequest request)
    {
        if (!await _db.Schools.AnyAsync(s => s.Id == schoolId))
            return NotFound(new ApiError(false, "Okul bulunamadı"));

        if (await _db.SchoolLanguages.AnyAsync(l => l.SchoolId == schoolId && l.Language == request.Language))
            return BadRequest(new ApiError(false, "Bu dil zaten eklenmiş"));

        var language = new SchoolLanguage
        {
            SchoolId = schoolId,
            Language = request.Language
        };

        _db.SchoolLanguages.Add(language);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<SchoolLanguageDto>(true, new SchoolLanguageDto(language.Id, language.SchoolId, language.Language, language.CreatedAt), "Dil eklendi"));
    }

    [HttpDelete("{languageId}")]
    [Authorize(Policy = "AllStaff")]
    public async Task<IActionResult> Remove(Guid schoolId, Guid languageId)
    {
        var language = await _db.SchoolLanguages.FirstOrDefaultAsync(l => l.Id == languageId && l.SchoolId == schoolId);
        if (language == null)
            return NotFound(new ApiError(false, "Dil bulunamadı"));

        _db.SchoolLanguages.Remove(language);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Dil kaldırıldı"));
    }
}
