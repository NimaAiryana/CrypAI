using back.Models;
using back.Services.External;

namespace back.Services;

public class MarketService : IMarketService
{
    private readonly ICoinMarketCapClient _coinMarketCapClient;
    private readonly ILogger<MarketService> _logger;

    public MarketService(ICoinMarketCapClient coinMarketCapClient, ILogger<MarketService> logger)
    {
        _coinMarketCapClient = coinMarketCapClient;
        _logger = logger;
    }

    public async Task<MarketOverview> GetMarketOverviewAsync()
    {
        _logger.LogInformation("Getting market overview");
        
        var globalMetrics = await _coinMarketCapClient.GetGlobalMetricsAsync();
        var trendingCoins = await GetTrendingCoinsAsync();
        
        return new MarketOverview
        {
            GlobalMetrics = globalMetrics,
            TrendingCoins = trendingCoins,
            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task<List<Cryptocurrency>> GetTrendingCoinsAsync()
    {
        _logger.LogInformation("Getting trending coins");
        
        // For trending coins, we'll get the top 10 by 24h volume
        var sortBy = "volume_24h";
        var sortDir = "desc";
        var limit = 10;
        
        return await _coinMarketCapClient.GetCryptocurrenciesAsync(1, limit, sortBy, sortDir);
    }

    public async Task<GlobalMetrics> GetGlobalMetricsAsync()
    {
        _logger.LogInformation("Getting global metrics");
        
        return await _coinMarketCapClient.GetGlobalMetricsAsync();
    }
}
