using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class CriteriaService : ICriteriaService
{
    private readonly InsyteDbContext _db;

    public CriteriaService(InsyteDbContext db) => _db = db;

    public async Task<PagedResult<CriteriaDto>> GetAllAsync(string? search, int page, int pageSize)
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

        return new PagedResult<CriteriaDto>(items, total, page, pageSize);
    }

    public async Task<CriteriaDto?> GetByIdAsync(Guid id)
    {
        var c = await _db.EvaluationCriteria.FindAsync(id);
        if (c == null) return null;

        return new CriteriaDto(c.Id, c.Name, c.Description, c.Instructions, c.Subject, c.IsActive, c.CreatedAt);
    }

    public async Task<object> GetWithQuestionsAsync()
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

        return new { items = criteria };
    }

    public async Task<object> CreateAsync(CreateCriteriaRequest request)
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

        return new { criteria.Id };
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Guid id, UpdateCriteriaRequest request)
    {
        var criteria = await _db.EvaluationCriteria.FindAsync(id);
        if (criteria == null) return (false, "Kriter bulunamadı");

        if (request.Name != null) criteria.Name = request.Name;
        if (request.Description != null) criteria.Description = request.Description;
        if (request.Instructions != null) criteria.Instructions = request.Instructions;
        if (request.Subject != null) criteria.Subject = request.Subject;
        if (request.IsActive.HasValue) criteria.IsActive = request.IsActive.Value;
        criteria.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id)
    {
        var criteria = await _db.EvaluationCriteria.FindAsync(id);
        if (criteria == null) return (false, "Kriter bulunamadı");

        criteria.IsActive = false;
        criteria.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
