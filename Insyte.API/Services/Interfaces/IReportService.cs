using Insyte.API.DTOs;
using Insyte.Core.Enums;

namespace Insyte.API.Services.Interfaces;

public interface IReportService
{
    Task<PagedResult<ReportDto>> GetAllAsync(string? search, ReportStatus? status, Guid? schoolId, int page, int pageSize);
    Task<ReportDto?> GetByIdAsync(Guid id);
    Task<(bool Success, string? Error, byte[]? Content, string? FileName)> GetPdfAsync(Guid id);
    Task<(bool Success, string? Error, object? Result)> SendAsync(Guid id, Guid sentById);
}
