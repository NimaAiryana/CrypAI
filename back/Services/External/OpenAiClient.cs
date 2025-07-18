using System.Text;
using System.Text.Json;
using back.Models;
using back.Utilities;

namespace back.Services.External;

public class OpenAiClient : IOpenAiClient
{
    private readonly IHttpClientWrapper _httpClient;
    private readonly ICacheManager _cacheManager;
    private readonly ILogger<OpenAiClient> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _model;

    public OpenAiClient(
        IHttpClientWrapper httpClient,
        ICacheManager cacheManager,
        ILogger<OpenAiClient> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cacheManager = cacheManager;
        _logger = logger;
        _apiKey = configuration["ExternalApis:OpenAI:ApiKey"] ?? throw new ArgumentNullException("OpenAI API key is missing");
        _baseUrl = configuration["ExternalApis:OpenAI:BaseUrl"] ?? "https://api.openai.com/v1";
        _model = configuration["ExternalApis:OpenAI:Model"] ?? "gpt-4o";
    }

    public async Task<string> GenerateTechnicalAnalysisAsync(CryptocurrencyDetails crypto, string timeframe)
    {
        var cacheKey = $"openai_technical_{crypto.Id}_{timeframe}";
        var cached = _cacheManager.Get<string>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved technical analysis from cache for {Symbol}", crypto.Symbol);
            return cached;
        }

        try
        {
            var prompt = new StringBuilder();
            prompt.AppendLine($"Provide a detailed technical analysis for {crypto.Name} ({crypto.Symbol}) with the following data:");
            prompt.AppendLine($"- Current Price: ${crypto.Price}");
            prompt.AppendLine($"- Market Cap: ${crypto.MarketCap}");
            prompt.AppendLine($"- 24h Volume: ${crypto.Volume24h}");
            prompt.AppendLine($"- 24h Change: {crypto.ChangePercentage24h}%");
            
            if (crypto.PriceChange.Any())
            {
                prompt.AppendLine("- Price Changes:");
                foreach (var change in crypto.PriceChange)
                {
                    prompt.AppendLine($"  - {change.Key}: {change.Value}%");
                }
            }
            
            prompt.AppendLine($"Timeframe for analysis: {timeframe}");
            prompt.AppendLine("Include the following in your analysis:");
            prompt.AppendLine("1. A summary of the current technical situation");
            prompt.AppendLine("2. Key support and resistance levels");
            prompt.AppendLine("3. Technical indicators (RSI, MACD, Moving Averages, etc.)");
            prompt.AppendLine("4. Volume analysis");
            prompt.AppendLine("5. An overall trend direction assessment");
            prompt.AppendLine("6. A clear recommendation (Buy, Sell, Hold)");
            prompt.AppendLine("Format the response in a clear, professional structure with headers for each section. Do not include any disclaimers or reminders that this is AI-generated content.");

            var analysis = await MakeOpenAiRequestAsync(prompt.ToString());
            
            // Cache for 1 hour since technical analysis doesn't change as frequently
            _cacheManager.Set(cacheKey, analysis, TimeSpan.FromHours(1));
            
            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating technical analysis for {Symbol}", crypto.Symbol);
            return "Unable to generate technical analysis at this time due to an error.";
        }
    }

    public async Task<string> GenerateFundamentalAnalysisAsync(CryptocurrencyDetails crypto)
    {
        var cacheKey = $"openai_fundamental_{crypto.Id}";
        var cached = _cacheManager.Get<string>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved fundamental analysis from cache for {Symbol}", crypto.Symbol);
            return cached;
        }

        try
        {
            var prompt = new StringBuilder();
            prompt.AppendLine($"Provide a detailed fundamental analysis for {crypto.Name} ({crypto.Symbol}) with the following data:");
            prompt.AppendLine($"- Current Price: ${crypto.Price}");
            prompt.AppendLine($"- Market Cap: ${crypto.MarketCap}");
            prompt.AppendLine($"- Circulating Supply: {crypto.CirculatingSupply}");
            prompt.AppendLine($"- Total Supply: {crypto.TotalSupply}");
            prompt.AppendLine($"- Max Supply: {crypto.MaxSupply}");
            
            if (crypto.Tags.Any())
            {
                prompt.AppendLine("- Tags/Categories:");
                foreach (var tag in crypto.Tags)
                {
                    prompt.AppendLine($"  - {tag}");
                }
            }
            
            prompt.AppendLine($"- Project Description: {crypto.Description}");
            prompt.AppendLine("Include the following in your analysis:");
            prompt.AppendLine("1. Project overview and core value proposition");
            prompt.AppendLine("2. Team assessment (based on general knowledge)");
            prompt.AppendLine("3. Technology assessment and innovation potential");
            prompt.AppendLine("4. Market positioning and competition");
            prompt.AppendLine("5. Tokenomics analysis (supply, distribution, utility)");
            prompt.AppendLine("6. Community strength and ecosystem development");
            prompt.AppendLine("7. An overall project assessment");
            prompt.AppendLine("8. A clear recommendation (Strong Buy, Buy, Hold, Sell, Strong Sell)");
            prompt.AppendLine("Format the response in a clear, professional structure with headers for each section. Do not include any disclaimers or reminders that this is AI-generated content.");

            var analysis = await MakeOpenAiRequestAsync(prompt.ToString());
            
            // Cache for 1 day since fundamental analysis changes less frequently
            _cacheManager.Set(cacheKey, analysis, TimeSpan.FromDays(1));
            
            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating fundamental analysis for {Symbol}", crypto.Symbol);
            return "Unable to generate fundamental analysis at this time due to an error.";
        }
    }

    public async Task<string> GenerateCombinedAnalysisAsync(
        string technicalAnalysis, 
        string fundamentalAnalysis, 
        CryptocurrencyDetails crypto)
    {
        var cacheKey = $"openai_combined_{crypto.Id}";
        var cached = _cacheManager.Get<string>(cacheKey);
        
        if (cached != null)
        {
            _logger.LogInformation("Retrieved combined analysis from cache for {Symbol}", crypto.Symbol);
            return cached;
        }

        try
        {
            var prompt = new StringBuilder();
            prompt.AppendLine($"Create a comprehensive combined analysis for {crypto.Name} ({crypto.Symbol}) by integrating the technical and fundamental analyses below.");
            prompt.AppendLine("\n=== TECHNICAL ANALYSIS ===\n");
            prompt.AppendLine(technicalAnalysis);
            prompt.AppendLine("\n=== FUNDAMENTAL ANALYSIS ===\n");
            prompt.AppendLine(fundamentalAnalysis);
            prompt.AppendLine("\nBased on both analyses, provide:");
            prompt.AppendLine("1. An integrated overview that weighs both technical and fundamental factors");
            prompt.AppendLine("2. Identification of any conflicting signals between technical and fundamental indicators");
            prompt.AppendLine("3. A balanced investment thesis considering short, medium, and long-term outlook");
            prompt.AppendLine("4. Risk assessment highlighting key concerns from both perspectives");
            prompt.AppendLine("5. An overall rating score from 0-100");
            prompt.AppendLine("6. A final recommendation with conviction level (Strong Buy, Buy, Hold, Sell, Strong Sell)");
            prompt.AppendLine("Format the response in a clear, professional structure with headers for each section. Do not include any disclaimers or reminders that this is AI-generated content.");

            var analysis = await MakeOpenAiRequestAsync(prompt.ToString());
            
            // Cache for 1 hour
            _cacheManager.Set(cacheKey, analysis, TimeSpan.FromHours(1));
            
            return analysis;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating combined analysis for {Symbol}", crypto.Symbol);
            return "Unable to generate combined analysis at this time due to an error.";
        }
    }

    private async Task<string> MakeOpenAiRequestAsync(string prompt)
    {
        try
        {
            var url = $"{_baseUrl}/chat/completions";
            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {_apiKey}" },
                { "Content-Type", "application/json" }
            };
            
            var requestData = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a professional cryptocurrency analyst specializing in both technical and fundamental analysis. Provide detailed, data-driven insights without disclosures or disclaimers." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.2
            };
            
            var response = await _httpClient.PostAsync<OpenAiResponse>(url, requestData, headers);
            
            if (response?.Choices == null || response.Choices.Count == 0)
            {
                _logger.LogWarning("Empty response received from OpenAI");
                return "No analysis could be generated at this time.";
            }
            
            return response.Choices[0].Message.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making request to OpenAI: {Message}", ex.Message);
            throw;
        }
    }
}

public class OpenAiResponse
{
    public List<OpenAiChoice> Choices { get; set; } = new List<OpenAiChoice>();
}

public class OpenAiChoice
{
    public OpenAiMessage Message { get; set; } = new OpenAiMessage();
}

public class OpenAiMessage
{
    public string Content { get; set; } = string.Empty;
}

public interface IOpenAiClient
{
    Task<string> GenerateTechnicalAnalysisAsync(CryptocurrencyDetails crypto, string timeframe);
    Task<string> GenerateFundamentalAnalysisAsync(CryptocurrencyDetails crypto);
    Task<string> GenerateCombinedAnalysisAsync(string technicalAnalysis, string fundamentalAnalysis, CryptocurrencyDetails crypto);
}
