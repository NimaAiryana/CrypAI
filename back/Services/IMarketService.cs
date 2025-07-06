using back.Models;

namespace back.Services;

public interface IMarketService
{
    Task<MarketOverview> GetMarketOverviewAsync();
    Task<List<Cryptocurrency>> GetTrendingCoinsAsync();
    Task<GlobalMetrics> GetGlobalMetricsAsync();
}
