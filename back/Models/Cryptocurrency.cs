namespace back.Models;

public class Cryptocurrency
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal MarketCap { get; set; }
    public decimal Volume24h { get; set; }
    public decimal ChangePercentage24h { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int Rank { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class CryptocurrencyDetails : Cryptocurrency
{
    public string Description { get; set; } = string.Empty;
    public string Algorithm { get; set; } = string.Empty;
    public decimal CirculatingSupply { get; set; }
    public decimal MaxSupply { get; set; }
    public decimal TotalSupply { get; set; }
    public Dictionary<string, decimal> PriceChange { get; set; } = new Dictionary<string, decimal>();
    public List<string> Tags { get; set; } = new List<string>();
    public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
}
