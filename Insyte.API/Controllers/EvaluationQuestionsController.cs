using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/criteria/{criteriaId}/questions")]
[Authorize]
public class EvaluationQuestionsController : BaseController
{
    private readonly IEvaluationQuestionService _questionService;

    public EvaluationQuestionsController(IEvaluationQuestionService questionService) => _questionService = questionService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid criteriaId)
    {
        var (criteriaExists, questions) = await _questionService.GetAllAsync(criteriaId);
        if (!criteriaExists) return NotFound(new ApiError(false, "Kriter bulunamadı"));

        return Ok(new ApiResponse<List<EvaluationQuestionDto>>(true, questions));
    }

    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetById(Guid criteriaId, Guid questionId)
    {
        var question = await _questionService.GetByIdAsync(questionId, criteriaId);
        if (question == null) return NotFound(new ApiError(false, "Soru bulunamadı"));

        return Ok(new ApiResponse<EvaluationQuestionDto>(true, question));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Create(Guid criteriaId, [FromBody] CreateEvaluationQuestionRequest request)
    {
        var (success, error, result) = await _questionService.CreateAsync(criteriaId, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, result, "Soru oluşturuldu"));
    }

    [HttpPut("{questionId}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Update(Guid criteriaId, Guid questionId, [FromBody] UpdateEvaluationQuestionRequest request)
    {
        var (success, error) = await _questionService.UpdateAsync(questionId, criteriaId, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Soru güncellendi"));
    }

    [HttpDelete("{questionId}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Delete(Guid criteriaId, Guid questionId)
    {
        var (success, error) = await _questionService.DeleteAsync(questionId, criteriaId);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Soru silindi"));
    }
}
