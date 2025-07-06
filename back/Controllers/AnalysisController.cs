using Microsoft.AspNetCore.Mvc;
using back.Models;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IAnalysisService _analysisService;
    private readonly ILogger<AnalysisController> _logger;

    public AnalysisController(IAnalysisService analysisService, ILogger<AnalysisController> logger)
    {
        _analysisService = analysisService;
        _logger = logger;
    }

    [HttpGet("technical/{cryptoId}")]
    public async Task<ActionResult<ApiResponse<TechnicalAnalysis>>> GetTechnicalAnalysis(
        string cryptoId, 
        [FromQuery] string timeframe = "24h")
    {
        try
        {
            _logger.LogInformation("Getting technical analysis for {CryptoId} with timeframe {Timeframe}", 
                cryptoId, timeframe);
            
            var request = new AnalysisRequest { CryptoId = cryptoId, Timeframe = timeframe };
            var analysis = await _analysisService.GetTechnicalAnalysisAsync(request);
            
            return Ok(new ApiResponse<TechnicalAnalysis>
            {
                Success = true,
                Message = "Technical analysis retrieved successfully",
                Data = analysis
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving technical analysis for {CryptoId}", cryptoId);
            return StatusCode(500, new ErrorResponse
            {
                Message = $"Failed to retrieve technical analysis for {cryptoId}"
            });
        }
    }

    [HttpGet("fundamental/{cryptoId}")]
    public async Task<ActionResult<ApiResponse<FundamentalAnalysis>>> GetFundamentalAnalysis(string cryptoId)
    {
        try
        {
            _logger.LogInformation("Getting fundamental analysis for {CryptoId}", cryptoId);
            
            var request = new AnalysisRequest { CryptoId = cryptoId };
            var analysis = await _analysisService.GetFundamentalAnalysisAsync(request);
            
            return Ok(new ApiResponse<FundamentalAnalysis>
            {
                Success = true,
                Message = "Fundamental analysis retrieved successfully",
                Data = analysis
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving fundamental analysis for {CryptoId}", cryptoId);
            return StatusCode(500, new ErrorResponse
            {
                Message = $"Failed to retrieve fundamental analysis for {cryptoId}"
            });
        }
    }

    [HttpGet("combined/{cryptoId}")]
    public async Task<ActionResult<ApiResponse<CombinedAnalysis>>> GetCombinedAnalysis(
        string cryptoId, 
        [FromQuery] string timeframe = "24h")
    {
        try
        {
            _logger.LogInformation("Getting combined analysis for {CryptoId} with timeframe {Timeframe}", 
                cryptoId, timeframe);
            
            var request = new AnalysisRequest { CryptoId = cryptoId, Timeframe = timeframe };
            var analysis = await _analysisService.GetCombinedAnalysisAsync(request);
            
            return Ok(new ApiResponse<CombinedAnalysis>
            {
                Success = true,
                Message = "Combined analysis retrieved successfully",
                Data = analysis
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving combined analysis for {CryptoId}", cryptoId);
            return StatusCode(500, new ErrorResponse
            {
                Message = $"Failed to retrieve combined analysis for {cryptoId}"
            });
        }
    }
}
