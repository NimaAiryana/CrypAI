namespace back.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string Source { get; set; } = "api"; // "api" or "cache"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class PaginatedResponse<T> : ApiResponse<List<T>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}

// Request Models
public class AnalysisRequest
{
    public string CryptoId { get; set; } = string.Empty;
    public string? Timeframe { get; set; } // Optional, defaults to "24h" in controller
}

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string SortBy { get; set; } = "market_cap";
    public string Order { get; set; } = "desc"; // "asc" or "desc"
}
