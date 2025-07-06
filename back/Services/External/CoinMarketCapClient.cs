using System.Text.Json;
using back.Models;
using back.Utilities;

namespace back.Services.External;

public class CoinMarketCapClient : ICoinMarketCapClient
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly ICacheManager _cacheManager;
    private readonly ILogger<CoinMarketCapClient> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public CoinMarketCapClient(
        IHttpClientWrapper httpClient,
        ICacheManager cacheManager,
        ILogger<CoinMarketCapClient> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cacheManager = cacheManager;
        _logger = logger;
        _apiKey = configuration["ExternalApis:CoinMarketCap:ApiKey"] ?? throw new ArgumentNullException("CoinMarketCap API key is missing");
        _baseUrl = configuration["ExternalApis:CoinMarketCap:BaseUrl"] ?? "https://pro-api.coinmarketcap.com/v1";
    }

    public async Task<List<Cryptocurrency>> GetCryptocurrenciesAsync(int start, int limit, string sort, string sortDirection)
    {
        var cacheKey = $"cmc_listings_{start}_{limit}_{sort}_{sortDirection}";
        var cached = _cacheManager.Get<List<Cryptocurrency>>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved cryptocurrencies from cache");
            return cached;
        }

        try
        {
            var url = $"{_baseUrl}/cryptocurrency/listings/latest?start={start}&limit={limit}&sort={sort}&sort_dir={sortDirection}";
            var headers = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", _apiKey },
                { "Accept", "application/json" }
            };
            
            var response = await _httpClient.GetAsync<CoinMarketCapListingsResponse>(url, headers);
            
            if (response?.Data == null)
            {
                _logger.LogWarning("No data returned from CoinMarketCap listings endpoint");
                return new List<Cryptocurrency>();
            }
            
            var cryptocurrencies = response.Data.Select(item => new Cryptocurrency
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                Symbol = item.Symbol,
                Price = item.Quote.USD.Price,
                MarketCap = item.Quote.USD.MarketCap,
                Volume24h = item.Quote.USD.Volume24h,
                ChangePercentage24h = item.Quote.USD.PercentChange24h,
                Rank = item.CmcRank,
                LastUpdated = item.LastUpdated
            }).ToList();
            
            // Cache for 5 minutes
            _cacheManager.Set(cacheKey, cryptocurrencies, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Retrieved {Count} cryptocurrencies from CoinMarketCap", cryptocurrencies.Count);
            
            return cryptocurrencies;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cryptocurrencies from CoinMarketCap");
            return new List<Cryptocurrency>();
        }
    }

    public async Task<CryptocurrencyDetails?> GetCryptocurrencyDetailsAsync(string id)
    {
        var cacheKey = $"cmc_crypto_{id}";
        var cached = _cacheManager.Get<CryptocurrencyDetails>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved crypto details from cache for ID: {Id}", id);
            return cached;
        }
        
        try
        {
            var url = $"{_baseUrl}/cryptocurrency/info?id={id}";
            var headers = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", _apiKey },
                { "Accept", "application/json" }
            };
            
            var response = await _httpClient.GetAsync<CoinMarketCapInfoResponse>(url, headers);
            
            if (response?.Data == null || !response.Data.ContainsKey(id))
            {
                _logger.LogWarning("No data returned from CoinMarketCap info endpoint for ID: {Id}", id);
                return null;
            }
            
            var coinInfo = response.Data[id];
            
            // Get additional price/market data
            var quotesUrl = $"{_baseUrl}/cryptocurrency/quotes/latest?id={id}";
            var quotesResponse = await _httpClient.GetAsync<CoinMarketCapQuotesResponse>(quotesUrl, headers);
            
            if (quotesResponse?.Data == null || !quotesResponse.Data.ContainsKey(id))
            {
                _logger.LogWarning("No quotes data returned from CoinMarketCap for ID: {Id}", id);
                return null;
            }
            
            var quoteData = quotesResponse.Data[id];
            
            var crypto = new CryptocurrencyDetails
            {
                Id = id,
                Name = coinInfo.Name,
                Symbol = coinInfo.Symbol,
                Description = coinInfo.Description,
                Algorithm = coinInfo.Algorithm ?? "N/A",
                Price = quoteData.Quote.USD.Price,
                MarketCap = quoteData.Quote.USD.MarketCap,
                Volume24h = quoteData.Quote.USD.Volume24h,
                ChangePercentage24h = quoteData.Quote.USD.PercentChange24h,
                CirculatingSupply = quoteData.CirculatingSupply,
                TotalSupply = quoteData.TotalSupply,
                MaxSupply = quoteData.MaxSupply,
                Rank = quoteData.CmcRank,
                ImageUrl = coinInfo.Logo,
                LastUpdated = quoteData.LastUpdated,
                Tags = coinInfo.Tags?.ToList() ?? new List<string>(),
                PriceChange = new Dictionary<string, decimal>
                {
                    { "1h", quoteData.Quote.USD.PercentChange1h },
                    { "24h", quoteData.Quote.USD.PercentChange24h },
                    { "7d", quoteData.Quote.USD.PercentChange7d },
                    { "30d", quoteData.Quote.USD.PercentChange30d }
                }
            };
            
            // Cache for 10 minutes
            _cacheManager.Set(cacheKey, crypto, TimeSpan.FromMinutes(10));
            _logger.LogInformation("Retrieved crypto details from CoinMarketCap for ID: {Id}", id);
            
            return crypto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cryptocurrency details from CoinMarketCap for ID: {Id}", id);
            return null;
        }
    }

    public async Task<PriceHistory?> GetPriceHistoryAsync(string id, string interval, string days)
    {
        var cacheKey = $"cmc_history_{id}_{interval}_{days}";
        var cached = _cacheManager.Get<PriceHistory>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved price history from cache for ID: {Id}", id);
            return cached;
        }
        
        try
        {
            // Implement mock history data for now as CoinMarketCap's historical data requires Enterprise plan
            // In a real implementation, you would use the appropriate CoinMarketCap endpoint
            
            var random = new Random();
            var now = DateTime.UtcNow;
            var daysCount = int.Parse(days);
            var dataPoints = new List<PricePoint>();
            
            // Get current price for reference
            var crypto = await GetCryptocurrencyDetailsAsync(id);
            if (crypto == null)
                return null;
                
            decimal basePrice = crypto.Price;
            
            // Generate mock historical data
            for (int i = daysCount; i >= 0; i--)
            {
                var timestamp = now.AddDays(-i);
                var volatility = (decimal)(random.NextDouble() * 0.05); // 5% max daily volatility
                var direction = random.Next(2) == 0 ? -1 : 1;
                var priceChange = basePrice * volatility * direction;
                
                // Make the price trend somewhat realistic
                basePrice = basePrice + priceChange;
                if (basePrice <= 0) basePrice = crypto.Price * 0.1m; // Avoid negative prices
                
                dataPoints.Add(new PricePoint
                {
                    Timestamp = timestamp,
                    Price = basePrice,
                    Volume = crypto.Volume24h * (decimal)(0.7 + random.NextDouble() * 0.6) // 70-130% of current volume
                });
            }
            
            var history = new PriceHistory
            {
                CryptoId = id,
                Interval = interval,
                Data = dataPoints
            };
            
            // Cache for 30 minutes
            _cacheManager.Set(cacheKey, history, TimeSpan.FromMinutes(30));
            _logger.LogInformation("Generated price history for ID: {Id}", id);
            
            return history;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating price history for ID: {Id}", id);
            return null;
        }
    }

    public async Task<GlobalMetrics> GetGlobalMetricsAsync()
    {
        var cacheKey = "cmc_global_metrics";
        var cached = _cacheManager.Get<GlobalMetrics>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved global metrics from cache");
            return cached;
        }
        
        try
        {
            var url = $"{_baseUrl}/global-metrics/quotes/latest";
            var headers = new Dictionary<string, string>
            {
                { "X-CMC_PRO_API_KEY", _apiKey },
                { "Accept", "application/json" }
            };
            
            var response = await _httpClient.GetAsync<CoinMarketCapGlobalMetricsResponse>(url, headers);
            
            if (response?.Data == null)
            {
                _logger.LogWarning("No data returned from CoinMarketCap global metrics endpoint");
                return new GlobalMetrics();
            }
            
            var metrics = new GlobalMetrics
            {
                TotalMarketCap = response.Data.Quote.USD.TotalMarketCap,
                TotalVolume24h = response.Data.Quote.USD.TotalVolume24h,
                BitcoinDominance = response.Data.BtcDominance,
                ActiveCryptocurrencies = response.Data.ActiveCryptocurrencies,
                ActiveExchanges = response.Data.ActiveExchanges,
                MarketCapChangePercentage24h = response.Data.Quote.USD.TotalMarketCapPercentChange24h
            };
            
            // Cache for 15 minutes
            _cacheManager.Set(cacheKey, metrics, TimeSpan.FromMinutes(15));
            _logger.LogInformation("Retrieved global metrics from CoinMarketCap");
            
            return metrics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving global metrics from CoinMarketCap");
            return new GlobalMetrics();
        }
    }

    public async Task<List<Cryptocurrency>> SearchCryptocurrenciesAsync(string query)
    {
        try
        {
            // Since CoinMarketCap doesn't have a dedicated search endpoint in the free tier,
            // we'll get the top 100 cryptocurrencies and filter them locally
            var cryptocurrencies = await GetCryptocurrenciesAsync(1, 100, "market_cap", "desc");
            
            if (string.IsNullOrWhiteSpace(query))
                return cryptocurrencies;
                
            return cryptocurrencies
                .Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                            c.Symbol.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching cryptocurrencies with query: {Query}", query);
            return new List<Cryptocurrency>();
        }
    }
}

// CoinMarketCap API response models
public class CoinMarketCapListingsResponse
{
    public List<CoinMarketCapCrypto> Data { get; set; } = new List<CoinMarketCapCrypto>();
}

public class CoinMarketCapInfoResponse
{
    public Dictionary<string, CoinMarketCapInfo> Data { get; set; } = new Dictionary<string, CoinMarketCapInfo>();
}

public class CoinMarketCapQuotesResponse
{
    public Dictionary<string, CoinMarketCapCrypto> Data { get; set; } = new Dictionary<string, CoinMarketCapCrypto>();
}

public class CoinMarketCapGlobalMetricsResponse
{
    public CoinMarketCapGlobalData Data { get; set; } = new CoinMarketCapGlobalData();
}

public class CoinMarketCapCrypto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int CmcRank { get; set; }
    public decimal CirculatingSupply { get; set; }
    public decimal TotalSupply { get; set; }
    public decimal MaxSupply { get; set; }
    public DateTime LastUpdated { get; set; }
    public CoinMarketCapQuote Quote { get; set; } = new CoinMarketCapQuote();
}

public class CoinMarketCapInfo
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string[] Categories { get; set; } = Array.Empty<string>();
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string Algorithm { get; set; } = string.Empty;
}

public class CoinMarketCapQuote
{
    public CoinMarketCapUsdData USD { get; set; } = new CoinMarketCapUsdData();
}

public class CoinMarketCapUsdData
{
    public decimal Price { get; set; }
    public decimal Volume24h { get; set; }
    public decimal MarketCap { get; set; }
    public decimal PercentChange1h { get; set; }
    public decimal PercentChange24h { get; set; }
    public decimal PercentChange7d { get; set; }
    public decimal PercentChange30d { get; set; }
    public decimal TotalMarketCap { get; set; }
    public decimal TotalVolume24h { get; set; }
    public decimal TotalMarketCapPercentChange24h { get; set; }
}

public class CoinMarketCapGlobalData
{
    public int ActiveCryptocurrencies { get; set; }
    public int ActiveExchanges { get; set; }
    public decimal BtcDominance { get; set; }
    public decimal EthDominance { get; set; }
    public CoinMarketCapQuote Quote { get; set; } = new CoinMarketCapQuote();
}

public interface ICoinMarketCapClient
{
    Task<List<Cryptocurrency>> GetCryptocurrenciesAsync(int start, int limit, string sort, string sortDirection);
    Task<CryptocurrencyDetails?> GetCryptocurrencyDetailsAsync(string id);
    Task<PriceHistory?> GetPriceHistoryAsync(string id, string interval, string days);
    Task<GlobalMetrics> GetGlobalMetricsAsync();
    Task<List<Cryptocurrency>> SearchCryptocurrenciesAsync(string query);
}
