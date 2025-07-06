using back.Models;

namespace back.Services;

public interface ICryptoService
{
    Task<(List<Cryptocurrency> Items, int Page, int PageSize, int TotalItems, int TotalPages)> GetCryptocurrenciesAsync(PaginationRequest request);
    Task<CryptocurrencyDetails?> GetCryptocurrencyDetailsAsync(string id);
    Task<PriceHistory?> GetPriceHistoryAsync(string id, string interval, string days);
    Task<List<Cryptocurrency>> SearchCryptocurrenciesAsync(string query);
}
