using back.Models;
using back.Services.External;

namespace back.Services;

public class AnalysisService : IAnalysisService
{
    private readonly ICryptoService _cryptoService;
    private readonly IGeminiClient _geminiClient;
    private readonly ILogger<AnalysisService> _logger;

    public AnalysisService(
        ICryptoService cryptoService,
        IGeminiClient geminiClient,
        ILogger<AnalysisService> logger)
    {
        _cryptoService = cryptoService;
        _geminiClient = geminiClient;
        _logger = logger;
    }

    public async Task<TechnicalAnalysis> GetTechnicalAnalysisAsync(AnalysisRequest request)
    {
        _logger.LogInformation("Generating technical analysis for crypto ID: {CryptoId} with timeframe: {Timeframe}",
            request.CryptoId, request.Timeframe ?? "24h");
            
        var timeframe = request.Timeframe ?? "24h";
            
        // Get cryptocurrency details
        var crypto = await _cryptoService.GetCryptocurrencyDetailsAsync(request.CryptoId);
        if (crypto == null)
        {
            _logger.LogWarning("Cryptocurrency with ID {CryptoId} not found", request.CryptoId);
            throw new KeyNotFoundException($"Cryptocurrency with ID {request.CryptoId} not found");
        }
        
        // Generate AI analysis using Gemini
        var analysisText = await _geminiClient.GenerateTechnicalAnalysisAsync(crypto, timeframe);
        
        // Parse key indicators from the analysis text
        // In a real implementation, you might want to use more structured prompts to get consistent outputs
        var supportLevel = ExtractDecimalFromText(analysisText, "support", crypto.Price * 0.9m);
        var resistanceLevel = ExtractDecimalFromText(analysisText, "resistance", crypto.Price * 1.1m);
        var rsi = ExtractDecimalFromText(analysisText, "RSI", 50m);
        var macd = ExtractDecimalFromText(analysisText, "MACD", 0m);
        var trendDirection = ExtractTrendDirection(analysisText);
        var recommendation = ExtractRecommendation(analysisText);
        
        // Create and return the technical analysis
        var analysis = new TechnicalAnalysis
        {
            CryptoId = crypto.Id,
            CryptoName = crypto.Name,
            CryptoSymbol = crypto.Symbol,
            Timeframe = timeframe,
            Summary = analysisText,
            TrendDirection = trendDirection,
            RSI = rsi,
            MACD = macd,
            Volume = crypto.Volume24h,
            Recommendation = recommendation,
            CreatedAt = DateTime.UtcNow
        };
        
        analysis.SupportLevels.Add("key", supportLevel);
        analysis.ResistanceLevels.Add("key", resistanceLevel);
        
        analysis.Indicators.Add("RSI", rsi.ToString("F2"));
        analysis.Indicators.Add("MACD", macd.ToString("F2"));
        analysis.Indicators.Add("Trend", trendDirection);
        
        return analysis;
    }

    public async Task<FundamentalAnalysis> GetFundamentalAnalysisAsync(AnalysisRequest request)
    {
        _logger.LogInformation("Generating fundamental analysis for crypto ID: {CryptoId}", request.CryptoId);
        
        // Get cryptocurrency details
        var crypto = await _cryptoService.GetCryptocurrencyDetailsAsync(request.CryptoId);
        if (crypto == null)
        {
            _logger.LogWarning("Cryptocurrency with ID {CryptoId} not found", request.CryptoId);
            throw new KeyNotFoundException($"Cryptocurrency with ID {request.CryptoId} not found");
        }
        
        // Generate AI analysis using Gemini
        var analysisText = await _geminiClient.GenerateFundamentalAnalysisAsync(crypto);
        
        // Extract key insights
        var teamAssessment = ExtractSection(analysisText, "Team");
        var technologyAssessment = ExtractSection(analysisText, "Technology");
        var communityAssessment = ExtractSection(analysisText, "Community");
        var marketSentiment = ExtractMarketSentiment(analysisText);
        var recommendation = ExtractRecommendation(analysisText);
        
        // Create and return the fundamental analysis
        var analysis = new FundamentalAnalysis
        {
            CryptoId = crypto.Id,
            CryptoName = crypto.Name,
            CryptoSymbol = crypto.Symbol,
            Summary = analysisText,
            TeamAssessment = teamAssessment,
            TechnologyAssessment = technologyAssessment,
            CommunityAssessment = communityAssessment,
            MarketSentiment = marketSentiment,
            CompetitiveAnalysis = ExtractSection(analysisText, "Competition"),
            Recommendation = recommendation,
            CreatedAt = DateTime.UtcNow
        };
        
        analysis.Indicators.Add("Team", teamAssessment.Length > 100 ? teamAssessment.Substring(0, 100) + "..." : teamAssessment);
        analysis.Indicators.Add("Technology", technologyAssessment.Length > 100 ? technologyAssessment.Substring(0, 100) + "..." : technologyAssessment);
        analysis.Indicators.Add("Community", communityAssessment.Length > 100 ? communityAssessment.Substring(0, 100) + "..." : communityAssessment);
        analysis.Indicators.Add("Sentiment", marketSentiment);
        
        return analysis;
    }

    public async Task<CombinedAnalysis> GetCombinedAnalysisAsync(AnalysisRequest request)
    {
        _logger.LogInformation("Generating combined analysis for crypto ID: {CryptoId}", request.CryptoId);
        
        // Get cryptocurrency details
        var crypto = await _cryptoService.GetCryptocurrencyDetailsAsync(request.CryptoId);
        if (crypto == null)
        {
            _logger.LogWarning("Cryptocurrency with ID {CryptoId} not found", request.CryptoId);
            throw new KeyNotFoundException($"Cryptocurrency with ID {request.CryptoId} not found");
        }
        
        // Get individual analyses first
        var technicalAnalysis = await GetTechnicalAnalysisAsync(request);
        var fundamentalAnalysis = await GetFundamentalAnalysisAsync(request);
        
        // Generate combined analysis using Gemini
        var combinedText = await _geminiClient.GenerateCombinedAnalysisAsync(
            technicalAnalysis.Summary,
            fundamentalAnalysis.Summary,
            crypto);
            
        // Extract overall score and recommendation
        var overallScore = ExtractOverallScore(combinedText);
        var recommendation = ExtractRecommendation(combinedText);
        var outlook = ExtractSection(combinedText, "Outlook");
        
        // Create and return the combined analysis
        var analysis = new CombinedAnalysis
        {
            CryptoId = crypto.Id,
            CryptoName = crypto.Name,
            CryptoSymbol = crypto.Symbol,
            Summary = combinedText,
            TechnicalData = technicalAnalysis,
            FundamentalData = fundamentalAnalysis,
            IntegratedOutlook = outlook,
            OverallScore = overallScore,
            Recommendation = recommendation,
            CreatedAt = DateTime.UtcNow
        };
        
        analysis.Indicators.Add("Technical", technicalAnalysis.Recommendation);
        analysis.Indicators.Add("Fundamental", fundamentalAnalysis.Recommendation);
        analysis.Indicators.Add("Score", overallScore.ToString());
        
        return analysis;
    }
    
    #region Helper Methods
    
    private decimal ExtractDecimalFromText(string text, string keyword, decimal defaultValue)
    {
        try
        {
            // Very simple extraction - in a real implementation, you'd want to use 
            // more sophisticated NLP or structured outputs from the AI
            var keywordIndex = text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
            if (keywordIndex < 0) return defaultValue;
            
            var textAfterKeyword = text.Substring(keywordIndex, Math.Min(100, text.Length - keywordIndex));
            var numberMatch = System.Text.RegularExpressions.Regex.Match(textAfterKeyword, @"[-+]?\d*\.?\d+");
            
            if (numberMatch.Success && decimal.TryParse(numberMatch.Value, out var result))
            {
                return result;
            }
            
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
    
    private string ExtractTrendDirection(string text)
    {
        if (text.Contains("uptrend", StringComparison.OrdinalIgnoreCase) || 
            text.Contains("bullish", StringComparison.OrdinalIgnoreCase))
        {
            return "Bullish";
        }
        else if (text.Contains("downtrend", StringComparison.OrdinalIgnoreCase) || 
                 text.Contains("bearish", StringComparison.OrdinalIgnoreCase))
        {
            return "Bearish";
        }
        else if (text.Contains("sideways", StringComparison.OrdinalIgnoreCase) || 
                 text.Contains("neutral", StringComparison.OrdinalIgnoreCase) ||
                 text.Contains("ranging", StringComparison.OrdinalIgnoreCase))
        {
            return "Neutral";
        }
        
        return "Neutral";
    }
    
    private string ExtractRecommendation(string text)
    {
        if (text.Contains("strong buy", StringComparison.OrdinalIgnoreCase))
        {
            return "Strong Buy";
        }
        else if (text.Contains("buy", StringComparison.OrdinalIgnoreCase))
        {
            return "Buy";
        }
        else if (text.Contains("strong sell", StringComparison.OrdinalIgnoreCase))
        {
            return "Strong Sell";
        }
        else if (text.Contains("sell", StringComparison.OrdinalIgnoreCase))
        {
            return "Sell";
        }
        else if (text.Contains("hold", StringComparison.OrdinalIgnoreCase) || 
                 text.Contains("neutral", StringComparison.OrdinalIgnoreCase))
        {
            return "Hold";
        }
        
        return "Hold";
    }
    
    private string ExtractSection(string text, string sectionName)
    {
        try
        {
            var sectionHeaders = new[]
            {
                sectionName,
                sectionName + " Assessment",
                sectionName + " Analysis",
                sectionName + " Evaluation"
            };
            
            foreach (var header in sectionHeaders)
            {
                var headerIndex = text.IndexOf(header, StringComparison.OrdinalIgnoreCase);
                if (headerIndex >= 0)
                {
                    var startIndex = text.IndexOf("\n", headerIndex);
                    if (startIndex >= 0)
                    {
                        startIndex += 1;  // Skip the newline
                        
                        // Find the next section header (assume it starts with #, ##, or is a capitalized word followed by a colon)
                        var nextSectionPattern = @"(\n#+\s|\n[A-Z][a-zA-Z\s]+:)";
                        var nextSectionMatch = System.Text.RegularExpressions.Regex.Match(text.Substring(startIndex), nextSectionPattern);
                        
                        var endIndex = nextSectionMatch.Success ? 
                            startIndex + nextSectionMatch.Index : 
                            text.Length;
                            
                        var sectionText = text.Substring(startIndex, endIndex - startIndex).Trim();
                        return sectionText;
                    }
                }
            }
            
            // If no specific section found, return a default message
            return $"No {sectionName.ToLower()} assessment available.";
        }
        catch
        {
            return $"Error extracting {sectionName.ToLower()} information.";
        }
    }
    
    private string ExtractMarketSentiment(string text)
    {
        var sentimentKeywords = new Dictionary<string, string>
        {
            { "very positive", "Very Positive" },
            { "positive", "Positive" },
            { "neutral", "Neutral" },
            { "negative", "Negative" },
            { "very negative", "Very Negative" },
            { "bullish", "Bullish" },
            { "bearish", "Bearish" }
        };
        
        foreach (var keyword in sentimentKeywords)
        {
            if (text.Contains(keyword.Key, StringComparison.OrdinalIgnoreCase))
            {
                return keyword.Value;
            }
        }
        
        return "Neutral";
    }
    
    private int ExtractOverallScore(string text)
    {
        try
        {
            // Look for patterns like "Score: 75/100" or "Rating: 75"
            var scorePatterns = new[]
            {
                @"score:?\s*(\d+)(?:/100)?",
                @"rating:?\s*(\d+)(?:/100)?",
                @"overall\s+score:?\s*(\d+)(?:/100)?",
                @"overall\s+rating:?\s*(\d+)(?:/100)?"
            };
            
            foreach (var pattern in scorePatterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(text, pattern, 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                
                if (match.Success && int.TryParse(match.Groups[1].Value, out var score))
                {
                    // Ensure score is in 0-100 range
                    return Math.Max(0, Math.Min(100, score));
                }
            }
            
            // If no score found, determine a score based on recommendation
            var recommendation = ExtractRecommendation(text);
            return recommendation switch
            {
                "Strong Buy" => 90,
                "Buy" => 75,
                "Hold" => 50,
                "Sell" => 25,
                "Strong Sell" => 10,
                _ => 50
            };
        }
        catch
        {
            return 50; // Default score
        }
    }
    
    #endregion
}
