using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace back.Utilities;

public interface IHttpClientWrapper
{
    Task<T?> GetAsync<T>(string url, Dictionary<string, string>? headers = null);
    Task<T?> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null);
}

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientWrapper> _logger;
    
    public HttpClientWrapper(HttpClient httpClient, ILogger<HttpClientWrapper> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<T?> GetAsync<T>(string url, Dictionary<string, string>? headers = null)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            
            _logger.LogDebug("Making GET request to {Url}", url);
            var response = await _httpClient.SendAsync(request);
            
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error for GET {Url}: {Message}", url, ex.Message);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error for GET {Url}: {Message}", url, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error for GET {Url}: {Message}", url, ex.Message);
            throw;
        }
    }
    
    public async Task<T?> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(data);
            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            
            _logger.LogDebug("Making POST request to {Url}", url);
            var response = await _httpClient.SendAsync(request);
            
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error for POST {Url}: {Message}", url, ex.Message);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error for POST {Url}: {Message}", url, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error for POST {Url}: {Message}", url, ex.Message);
            throw;
        }
    }
}
