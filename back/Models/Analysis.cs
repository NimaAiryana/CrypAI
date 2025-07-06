namespace back.Models;

public enum AnalysisType
{
    Technical,
    Fundamental,
    Combined
}

public class Analysis
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CryptoId { get; set; } = string.Empty;
    public string CryptoName { get; set; } = string.Empty;
    public string CryptoSymbol { get; set; } = string.Empty;
    public AnalysisType Type { get; set; }
    public string Summary { get; set; } = string.Empty;
    public Dictionary<string, string> Indicators { get; set; } = new Dictionary<string, string>();
    public string Recommendation { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TechnicalAnalysis : Analysis
{
    public TechnicalAnalysis()
    {
        Type = AnalysisType.Technical;
    }

    public string Timeframe { get; set; } = "24h";
    public Dictionary<string, decimal> SupportLevels { get; set; } = new Dictionary<string, decimal>();
    public Dictionary<string, decimal> ResistanceLevels { get; set; } = new Dictionary<string, decimal>();
    public string TrendDirection { get; set; } = string.Empty;
    public decimal RSI { get; set; }
    public decimal MACD { get; set; }
    public decimal Volume { get; set; }
}

public class FundamentalAnalysis : Analysis
{
    public FundamentalAnalysis()
    {
        Type = AnalysisType.Fundamental;
    }

    public string TeamAssessment { get; set; } = string.Empty;
    public string TechnologyAssessment { get; set; } = string.Empty;
    public string CommunityAssessment { get; set; } = string.Empty;
    public string MarketSentiment { get; set; } = string.Empty;
    public List<string> RecentNews { get; set; } = new List<string>();
    public string CompetitiveAnalysis { get; set; } = string.Empty;
}

public class CombinedAnalysis : Analysis
{
    public CombinedAnalysis()
    {
        Type = AnalysisType.Combined;
    }

    public TechnicalAnalysis TechnicalData { get; set; } = new TechnicalAnalysis();
    public FundamentalAnalysis FundamentalData { get; set; } = new FundamentalAnalysis();
    public string IntegratedOutlook { get; set; } = string.Empty;
    public int OverallScore { get; set; } // 0-100
}
