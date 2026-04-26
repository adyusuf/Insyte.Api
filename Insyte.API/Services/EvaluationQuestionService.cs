using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class EvaluationQuestionService : IEvaluationQuestionService
{
    private readonly InsyteDbContext _db;

    public EvaluationQuestionService(InsyteDbContext db) => _db = db;

    public async Task<(bool CriteriaExists, List<EvaluationQuestionDto>? Questions)> GetAllAsync(Guid criteriaId)
    {
        if (!await _db.EvaluationCriteria.AnyAsync(c => c.Id == criteriaId))
            return (false, null);

        var questions = await _db.EvaluationQuestions
            .Where(q => q.CriteriaId == criteriaId)
            .OrderBy(q => q.Order)
            .Select(q => new EvaluationQuestionDto(q.Id, q.CriteriaId, q.Question, q.Category, q.Order, q.IsActive, q.CreatedAt))
            .ToListAsync();

        return (true, questions);
    }

    public async Task<EvaluationQuestionDto?> GetByIdAsync(Guid id, Guid criteriaId)
    {
        var q = await _db.EvaluationQuestions.FirstOrDefaultAsync(x => x.Id == id && x.CriteriaId == criteriaId);
        if (q == null) return null;

        return new EvaluationQuestionDto(q.Id, q.CriteriaId, q.Question, q.Category, q.Order, q.IsActive, q.CreatedAt);
    }

    public async Task<(bool Success, string? Error, object? Result)> CreateAsync(Guid criteriaId, CreateEvaluationQuestionRequest request)
    {
        if (!await _db.EvaluationCriteria.AnyAsync(c => c.Id == criteriaId))
            return (false, "Kriter bulunamadı", null);

        var question = new EvaluationQuestion
        {
            CriteriaId = criteriaId,
            Question = request.Question,
            Category = request.Category,
            Order = request.Order
        };

        _db.EvaluationQuestions.Add(question);
        await _db.SaveChangesAsync();

        return (true, null, new { question.Id });
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Guid id, Guid criteriaId, UpdateEvaluationQuestionRequest request)
    {
        var question = await _db.EvaluationQuestions.FirstOrDefaultAsync(q => q.Id == id && q.CriteriaId == criteriaId);
        if (question == null) return (false, "Soru bulunamadı");

        if (!string.IsNullOrEmpty(request.Question)) question.Question = request.Question;
        if (request.Category != null) question.Category = request.Category;
        if (request.Order.HasValue) question.Order = request.Order.Value;
        if (request.IsActive.HasValue) question.IsActive = request.IsActive.Value;
        question.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid criteriaId)
    {
        var question = await _db.EvaluationQuestions.FirstOrDefaultAsync(q => q.Id == id && q.CriteriaId == criteriaId);
        if (question == null) return (false, "Soru bulunamadı");

        _db.EvaluationQuestions.Remove(question);
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
