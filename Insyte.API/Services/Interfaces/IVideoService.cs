using Insyte.API.DTOs;
using Insyte.Core.Enums;

namespace Insyte.API.Services.Interfaces;

public interface IVideoService
{
    Task<PagedResult<VideoDto>> GetAllAsync(string? search, Guid? schoolId, Guid? teacherId, VideoStatus? status, int page, int pageSize);
    Task<VideoDto?> GetByIdAsync(Guid id);
    Task<(bool Success, string? Error, object? Result)> UploadAsync(CreateVideoRequest request, IFormFile file, Guid uploadedByUserId);
    Task<(bool Success, string? Error, object? Result)> TriggerEvaluationAsync(Guid videoId, EvaluateVideoRequest request);
}
