using Microsoft.AspNetCore.Mvc;
using back.Models;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
    private readonly ICryptoService _cryptoService;
    private readonly ILogger<CryptoController> _logger;

    public CryptoController(ICryptoService cryptoService, ILogger<CryptoController> logger)
    {
        _cryptoService = cryptoService;
        _logger = logger;
    }

    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<List<Cryptocurrency>>>> GetCryptoList(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50,
        [FromQuery] string sortBy = "market_cap",
        [FromQuery] string order = "desc")
    {
        try
        {
            _logger.LogInformation("Retrieving crypto list with page={Page}, pageSize={PageSize}", page, pageSize);
            
            var paginationRequest = new PaginationRequest
            {
                Page = page,
                PageSize = Math.Min(pageSize, 100), // Limit to 100 items per page
                SortBy = sortBy,
                Order = order
            };
            
            var result = await _cryptoService.GetCryptocurrenciesAsync(paginationRequest);
            
            return Ok(new PaginatedResponse<Cryptocurrency>
            {
                Success = true,
                Message = "Cryptocurrencies retrieved successfully",
                Data = result.Items,
                Page = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalItems = result.TotalItems
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cryptocurrency list");
            return StatusCode(500, new ErrorResponse
            {
                Message = "Failed to retrieve cryptocurrency list"
            });
        }
    }

    [HttpGet("details/{id}")]
    public async Task<ActionResult<ApiResponse<CryptocurrencyDetails>>> GetCryptoDetails(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving details for crypto with id: {Id}", id);
            
            var crypto = await _cryptoService.GetCryptocurrencyDetailsAsync(id);
            
            if (crypto == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = $"Cryptocurrency with ID {id} not found"
                });
            }
            
            return Ok(new ApiResponse<CryptocurrencyDetails>
            {
                Success = true,
                Message = "Cryptocurrency details retrieved successfully",
                Data = crypto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cryptocurrency details for ID: {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = $"Failed to retrieve cryptocurrency details for ID: {id}"
            });
        }
    }

    [HttpGet("price-history/{id}")]
    public async Task<ActionResult<ApiResponse<PriceHistory>>> GetPriceHistory(
        string id, 
        [FromQuery] string interval = "1d", 
        [FromQuery] string days = "30")
    {
        try
        {
            _logger.LogInformation("Retrieving price history for crypto {Id} with interval {Interval} and days {Days}", 
                id, interval, days);
            
            var priceHistory = await _cryptoService.GetPriceHistoryAsync(id, interval, days);
            
            if (priceHistory == null || !priceHistory.Data.Any())
            {
                return NotFound(new ErrorResponse
                {
                    Message = $"Price history for cryptocurrency {id} not found"
                });
            }
            
            return Ok(new ApiResponse<PriceHistory>
            {
                Success = true,
                Message = "Price history retrieved successfully",
                Data = priceHistory
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving price history for ID: {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = $"Failed to retrieve price history for ID: {id}"
            });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<List<Cryptocurrency>>>> SearchCryptos([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Search query must be at least 2 characters long"
                });
            }
            
            _logger.LogInformation("Searching cryptocurrencies with query: {Query}", query);
            
            var results = await _cryptoService.SearchCryptocurrenciesAsync(query);
            
            return Ok(new ApiResponse<List<Cryptocurrency>>
            {
                Success = true,
                Message = "Search completed successfully",
                Data = results
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching cryptocurrencies with query: {Query}", query);
            return StatusCode(500, new ErrorResponse
            {
                Message = "Failed to search cryptocurrencies"
            });
        }
    }
}
