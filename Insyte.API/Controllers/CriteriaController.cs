using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CriteriaController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public CriteriaController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.EvaluationCriteria.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search) || (c.Subject != null && c.Subject.Contains(search)));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CriteriaDto(c.Id, c.Name, c.Description, c.Instructions, c.Subject, c.IsActive, c.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<CriteriaDto>>(true, new PagedResult<CriteriaDto>(items, total, page, pageSize)));
    }

    [HttpGet("with-questions")]
    public async Task<IActionResult> GetWithQuestions()
    {
        var criteria = await _db.EvaluationCriteria
            .Include(c => c.Questions)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CriteriaWithQuestionsDto(
                c.Id,
                c.Name,
                c.Description,
                c.Instructions,
                c.Subject,
                c.IsActive,
                c.CreatedAt,
                c.Questions
                    .OrderBy(q => q.Order)
                    .Select(q => new EvaluationQuestionDto(q.Id, q.CriteriaId, q.Question, q.Category, q.Order, q.IsActive, q.CreatedAt))
                    .ToList()
            ))
            .ToListAsync();

        return Ok(new ApiResponse<object>(true, new { items = criteria }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var c = await _db.EvaluationCriteria.FindAsync(id);
        if (c == null) return NotFound(new ApiError(false, "Kriter bulunamadı"));

        return Ok(new ApiResponse<CriteriaDto>(true,
            new CriteriaDto(c.Id, c.Name, c.Description, c.Instructions, c.Subject, c.IsActive, c.CreatedAt)));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Create([FromBody] CreateCriteriaRequest request)
    {
        var criteria = new EvaluationCriteria
        {
            Name = request.Name,
            Description = request.Description,
            Instructions = request.Instructions,
            Subject = request.Subject
        };

        _db.EvaluationCriteria.Add(criteria);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, new { criteria.Id }, "Kriter oluşturuldu"));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCriteriaRequest request)
    {
        var criteria = await _db.EvaluationCriteria.FindAsync(id);
        if (criteria == null) return NotFound(new ApiError(false, "Kriter bulunamadı"));

        if (request.Name != null) criteria.Name = request.Name;
        if (request.Description != null) criteria.Description = request.Description;
        if (request.Instructions != null) criteria.Instructions = request.Instructions;
        if (request.Subject != null) criteria.Subject = request.Subject;
        if (request.IsActive.HasValue) criteria.IsActive = request.IsActive.Value;
        criteria.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Kriter güncellendi"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var criteria = await _db.EvaluationCriteria.FindAsync(id);
        if (criteria == null) return NotFound(new ApiError(false, "Kriter bulunamadı"));

        criteria.IsActive = false;
        criteria.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Kriter deaktif edildi"));
    }
}
