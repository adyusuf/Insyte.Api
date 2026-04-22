namespace Insyte.API.DTOs;

public record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);

public record ApiResponse<T>(bool Success, T? Data, string? Message = null);
public record ApiError(bool Success, string Message);
