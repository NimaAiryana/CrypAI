using back.Models;
using back.Services.External;

namespace back.Services;

public class CryptoService : ICryptoService
{
    private readonly ICoinMarketCapClient _coinMarketCapClient;
    private readonly ILogger<CryptoService> _logger;

    public CryptoService(ICoinMarketCapClient coinMarketCapClient, ILogger<CryptoService> logger)
    {
        _coinMarketCapClient = coinMarketCapClient;
        _logger = logger;
    }

    public async Task<(List<Cryptocurrency> Items, int Page, int PageSize, int TotalItems, int TotalPages)> 
        GetCryptocurrenciesAsync(PaginationRequest request)
    {
        _logger.LogInformation("Getting cryptocurrencies with pagination: Page {Page}, PageSize {PageSize}", 
            request.Page, request.PageSize);
        
        var start = (request.Page - 1) * request.PageSize + 1;
        var sortBy = MapSortField(request.SortBy);
        var sortDir = request.Order.ToLower() == "asc" ? "asc" : "desc";
        
        var cryptocurrencies = await _coinMarketCapClient.GetCryptocurrenciesAsync(
            start, request.PageSize, sortBy, sortDir);
        
        // For demonstration purposes, assuming there are 5000 total cryptocurrencies
        // In a real implementation, you would get this from the CoinMarketCap API
        const int totalItems = 5000;
        var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);
        
        return (cryptocurrencies, request.Page, request.PageSize, totalItems, totalPages);
    }

    public async Task<CryptocurrencyDetails?> GetCryptocurrencyDetailsAsync(string id)
    {
        _logger.LogInformation("Getting cryptocurrency details for ID: {Id}", id);
        
        return await _coinMarketCapClient.GetCryptocurrencyDetailsAsync(id);
    }

    public async Task<PriceHistory?> GetPriceHistoryAsync(string id, string interval, string days)
    {
        _logger.LogInformation("Getting price history for ID: {Id}, interval: {Interval}, days: {Days}", 
            id, interval, days);
        
        return await _coinMarketCapClient.GetPriceHistoryAsync(id, interval, days);
    }

    public async Task<List<Cryptocurrency>> SearchCryptocurrenciesAsync(string query)
    {
        _logger.LogInformation("Searching cryptocurrencies with query: {Query}", query);
        
        return await _coinMarketCapClient.SearchCryptocurrenciesAsync(query);
    }
    
    private string MapSortField(string sortField)
    {
        return sortField.ToLower() switch
        {
            "market_cap" => "market_cap",
            "price" => "price",
            "volume" => "volume_24h",
            "change" => "percent_change_24h",
            "name" => "name",
            _ => "market_cap" // default sort
        };
    }
}
