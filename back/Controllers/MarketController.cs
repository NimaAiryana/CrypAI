using Microsoft.AspNetCore.Mvc;
using back.Models;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMarketService _marketService;
    private readonly ILogger<MarketController> _logger;

    public MarketController(IMarketService marketService, ILogger<MarketController> logger)
    {
        _marketService = marketService;
        _logger = logger;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<ApiResponse<MarketOverview>>> GetMarketOverview()
    {
        try
        {
            _logger.LogInformation("Retrieving market overview");
            
            var overview = await _marketService.GetMarketOverviewAsync();
            
            return Ok(new ApiResponse<MarketOverview>
            {
                Success = true,
                Message = "Market overview retrieved successfully",
                Data = overview
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving market overview");
            return StatusCode(500, new ErrorResponse
            {
                Message = "Failed to retrieve market overview"
            });
        }
    }

    [HttpGet("trending")]
    public async Task<ActionResult<ApiResponse<List<Cryptocurrency>>>> GetTrendingCoins()
    {
        try
        {
            _logger.LogInformation("Retrieving trending coins");
            
            var trendingCoins = await _marketService.GetTrendingCoinsAsync();
            
            return Ok(new ApiResponse<List<Cryptocurrency>>
            {
                Success = true,
                Message = "Trending coins retrieved successfully",
                Data = trendingCoins
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving trending coins");
            return StatusCode(500, new ErrorResponse
            {
                Message = "Failed to retrieve trending coins"
            });
        }
    }

    [HttpGet("global-metrics")]
    public async Task<ActionResult<ApiResponse<GlobalMetrics>>> GetGlobalMetrics()
    {
        try
        {
            _logger.LogInformation("Retrieving global market metrics");
            
            var metrics = await _marketService.GetGlobalMetricsAsync();
            
            return Ok(new ApiResponse<GlobalMetrics>
            {
                Success = true,
                Message = "Global metrics retrieved successfully",
                Data = metrics
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving global metrics");
            return StatusCode(500, new ErrorResponse
            {
                Message = "Failed to retrieve global metrics"
            });
        }
    }
}
