using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/criteria/{criteriaId}/questions")]
[Authorize]
public class EvaluationQuestionsController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public EvaluationQuestionsController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid criteriaId)
    {
        if (!await _db.EvaluationCriteria.AnyAsync(c => c.Id == criteriaId))
            return NotFound(new ApiError(false, "Kriter bulunamadı"));

        var questions = await _db.EvaluationQuestions
            .Where(q => q.CriteriaId == criteriaId)
            .OrderBy(q => q.Order)
            .Select(q => new EvaluationQuestionDto(q.Id, q.CriteriaId, q.Question, q.Category, q.Order, q.IsActive, q.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<EvaluationQuestionDto>>(true, questions));
    }

    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetById(Guid criteriaId, Guid questionId)
    {
        var q = await _db.EvaluationQuestions.FirstOrDefaultAsync(x => x.Id == questionId && x.CriteriaId == criteriaId);
        if (q == null) return NotFound(new ApiError(false, "Soru bulunamadı"));

        return Ok(new ApiResponse<EvaluationQuestionDto>(true,
            new EvaluationQuestionDto(q.Id, q.CriteriaId, q.Question, q.Category, q.Order, q.IsActive, q.CreatedAt)));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Create(Guid criteriaId, [FromBody] CreateEvaluationQuestionRequest request)
    {
        if (!await _db.EvaluationCriteria.AnyAsync(c => c.Id == criteriaId))
            return NotFound(new ApiError(false, "Kriter bulunamadı"));

        var question = new EvaluationQuestion
        {
            CriteriaId = criteriaId,
            Question = request.Question,
            Category = request.Category,
            Order = request.Order
        };

        _db.EvaluationQuestions.Add(question);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, new { question.Id }, "Soru oluşturuldu"));
    }

    [HttpPut("{questionId}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Update(Guid criteriaId, Guid questionId, [FromBody] UpdateEvaluationQuestionRequest request)
    {
        var question = await _db.EvaluationQuestions.FirstOrDefaultAsync(q => q.Id == questionId && q.CriteriaId == criteriaId);
        if (question == null) return NotFound(new ApiError(false, "Soru bulunamadı"));

        if (!string.IsNullOrEmpty(request.Question)) question.Question = request.Question;
        if (request.Category != null) question.Category = request.Category;
        if (request.Order.HasValue) question.Order = request.Order.Value;
        if (request.IsActive.HasValue) question.IsActive = request.IsActive.Value;
        question.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Soru güncellendi"));
    }

    [HttpDelete("{questionId}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Delete(Guid criteriaId, Guid questionId)
    {
        var question = await _db.EvaluationQuestions.FirstOrDefaultAsync(q => q.Id == questionId && q.CriteriaId == criteriaId);
        if (question == null) return NotFound(new ApiError(false, "Soru bulunamadı"));

        _db.EvaluationQuestions.Remove(question);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Soru silindi"));
    }
}
