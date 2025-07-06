namespace back.Models;

public class MarketOverview
{
    public GlobalMetrics GlobalMetrics { get; set; } = new GlobalMetrics();
    public List<Cryptocurrency> TrendingCoins { get; set; } = new List<Cryptocurrency>();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class GlobalMetrics
{
    public decimal TotalMarketCap { get; set; }
    public decimal TotalVolume24h { get; set; }
    public decimal BitcoinDominance { get; set; }
    public int ActiveCryptocurrencies { get; set; }
    public int ActiveExchanges { get; set; }
    public decimal MarketCapChangePercentage24h { get; set; }
}

public class PricePoint
{
    public DateTime Timestamp { get; set; }
    public decimal Price { get; set; }
    public decimal Volume { get; set; }
}

public class PriceHistory
{
    public string CryptoId { get; set; } = string.Empty;
    public string Interval { get; set; } = string.Empty; // 1h, 1d, 7d, 30d, etc.
    public List<PricePoint> Data { get; set; } = new List<PricePoint>();
}
