using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class ReportService : IReportService
{
    private readonly InsyteDbContext _db;

    public ReportService(InsyteDbContext db) => _db = db;

    public async Task<PagedResult<ReportDto>> GetAllAsync(string? search, ReportStatus? status, Guid? schoolId, int page, int pageSize)
    {
        var query = _db.Reports
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.School)
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.Teacher)
            .Include(r => r.ApprovedBy)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(r => r.Evaluation.Video.Title.Contains(search) ||
                                     r.Evaluation.Video.School.Name.Contains(search));
        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);
        if (schoolId.HasValue)
            query = query.Where(r => r.Evaluation.Video.SchoolId == schoolId.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReportDto(
                r.Id, r.EvaluationId, r.Evaluation.Video.Title,
                r.Evaluation.Video.School.Name,
                r.Evaluation.Video.Teacher.FirstName + " " + r.Evaluation.Video.Teacher.LastName,
                r.PdfPath,
                r.ApprovedBy != null ? r.ApprovedBy.FirstName + " " + r.ApprovedBy.LastName : null,
                r.ApprovedAt, r.Status, r.CreatedAt))
            .ToListAsync();

        return new PagedResult<ReportDto>(items, total, page, pageSize);
    }

    public async Task<ReportDto?> GetByIdAsync(Guid id)
    {
        var r = await _db.Reports
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.School)
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.Teacher)
            .Include(r => r.ApprovedBy)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (r == null) return null;

        return new ReportDto(
            r.Id, r.EvaluationId, r.Evaluation.Video.Title,
            r.Evaluation.Video.School.Name,
            r.Evaluation.Video.Teacher.FirstName + " " + r.Evaluation.Video.Teacher.LastName,
            r.PdfPath,
            r.ApprovedBy != null ? r.ApprovedBy.FirstName + " " + r.ApprovedBy.LastName : null,
            r.ApprovedAt, r.Status, r.CreatedAt);
    }

    public async Task<(bool Success, string? Error, byte[]? Content, string? FileName)> GetPdfAsync(Guid id)
    {
        var report = await _db.Reports.FindAsync(id);
        if (report == null) return (false, "Rapor bulunamadı", null, null);
        if (string.IsNullOrEmpty(report.PdfPath) || !File.Exists(report.PdfPath))
            return (false, "PDF dosyası bulunamadı", null, null);

        var bytes = await File.ReadAllBytesAsync(report.PdfPath);
        return (true, null, bytes, $"report-{report.Id}.pdf");
    }

    public async Task<(bool Success, string? Error, object? Result)> SendAsync(Guid id, Guid sentById)
    {
        var report = await _db.Reports
            .Include(r => r.Evaluation).ThenInclude(e => e.Video)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (report == null) return (false, "Rapor bulunamadı", null);
        if (report.Status != ReportStatus.Approved)
            return (false, "Sadece onaylanmış raporlar gönderilebilir", null);

        var emailConfigs = await _db.EmailConfigs
            .Where(ec => ec.SchoolId == report.Evaluation.Video.SchoolId && ec.IsActive)
            .ToListAsync();

        // TODO: Gerçek e-posta gönderimi e-posta servisi üzerinden yapılacak
        foreach (var config in emailConfigs)
        {
            _db.EmailLogs.Add(new Core.Entities.EmailLog
            {
                ReportId = id,
                RecipientEmail = config.RecipientEmail,
                Status = EmailStatus.Pending
            });
        }

        report.Status = ReportStatus.Sent;
        await _db.SaveChangesAsync();

        return (true, null, new { EmailCount = emailConfigs.Count });
    }
}
