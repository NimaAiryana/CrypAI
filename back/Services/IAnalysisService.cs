using back.Models;

namespace back.Services;

public interface IAnalysisService
{
    Task<TechnicalAnalysis> GetTechnicalAnalysisAsync(AnalysisRequest request);
    Task<FundamentalAnalysis> GetFundamentalAnalysisAsync(AnalysisRequest request);
    Task<CombinedAnalysis> GetCombinedAnalysisAsync(AnalysisRequest request);
}
