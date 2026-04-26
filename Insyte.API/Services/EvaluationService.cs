using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class EvaluationService : IEvaluationService
{
    private readonly InsyteDbContext _db;

    public EvaluationService(InsyteDbContext db) => _db = db;

    public async Task<PagedResult<EvaluationDto>> GetAllAsync(string? search, EvaluationStatus? status, int page, int pageSize)
    {
        var query = _db.Evaluations
            .Include(e => e.Video)
            .Include(e => e.Criteria)
            .Include(e => e.AIModel)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(e => e.Video.Title.Contains(search) || e.Criteria.Name.Contains(search));
        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EvaluationDto(
                e.Id, e.VideoId, e.Video.Title, e.CriteriaId, e.Criteria.Name,
                e.AIModelId, e.AIModel.Name, e.Result,
                e.TokenUsageInput, e.TokenUsageOutput, e.Status, e.ErrorMessage,
                e.CreatedAt, e.CompletedAt))
            .ToListAsync();

        return new PagedResult<EvaluationDto>(items, total, page, pageSize);
    }

    public async Task<EvaluationDto?> GetByIdAsync(Guid id)
    {
        var e = await _db.Evaluations
            .Include(e => e.Video)
            .Include(e => e.Criteria)
            .Include(e => e.AIModel)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (e == null) return null;

        return new EvaluationDto(
            e.Id, e.VideoId, e.Video.Title, e.CriteriaId, e.Criteria.Name,
            e.AIModelId, e.AIModel.Name, e.Result,
            e.TokenUsageInput, e.TokenUsageOutput, e.Status, e.ErrorMessage,
            e.CreatedAt, e.CompletedAt);
    }

    public async Task<(bool Success, string? Error, object? Result)> ApproveAsync(Guid id, Guid approverId)
    {
        var evaluation = await _db.Evaluations.Include(e => e.Video).FirstOrDefaultAsync(e => e.Id == id);
        if (evaluation == null) return (false, "Değerlendirme bulunamadı", null);

        if (evaluation.Status != EvaluationStatus.Completed)
            return (false, "Sadece tamamlanmış değerlendirmeler onaylanabilir", null);

        evaluation.Video.Status = VideoStatus.Approved;

        var report = new Report
        {
            EvaluationId = id,
            ApprovedByUserId = approverId,
            ApprovedAt = DateTime.UtcNow,
            Status = ReportStatus.Approved
        };

        _db.Reports.Add(report);
        await _db.SaveChangesAsync();

        return (true, null, new { report.Id });
    }

    public async Task<(bool Success, string? Error)> RejectAsync(Guid id)
    {
        var evaluation = await _db.Evaluations.Include(e => e.Video).FirstOrDefaultAsync(e => e.Id == id);
        if (evaluation == null) return (false, "Değerlendirme bulunamadı");

        evaluation.Video.Status = VideoStatus.Rejected;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
