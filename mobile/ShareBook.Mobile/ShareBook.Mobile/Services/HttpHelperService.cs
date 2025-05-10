using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ShareBook.Mobile.Models;

namespace Namespace.ShareBook.Mobile.Services;

public class HttpHelperService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public string? Token { get; private set; }
    private string? RefreshToken { get; set; }
    private int MaxRetryCount { get; set; } = 3;

    public HttpHelperService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );
        _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var baseUrl = configuration?.GetSection("ShareBookApiUrl")?.Value;
        if (baseUrl == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<T?> GetAsync<T>(string alias, int retryCount = 0)
    {
        var response = await _httpClient.GetAsync(alias);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result, _jsonSerializerOptions);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await RefreshTokenAsync();
            retryCount++;

            if (retryCount > MaxRetryCount)
            {
                // TODO: Add notification about the authentication error
                return default;
            }
            return await GetAsync<T>(alias, retryCount);
        }
        else
        {
            // TODO: Handle other status codes
        }
        return default;
    }

    private async Task<bool> RefreshTokenAsync()
    {
        var response = await this.PostAsync<TokenModel, TokenModel>(
            "refresh",
            new TokenModel() { RefreshToken = this.RefreshToken }
        );
        if (string.IsNullOrEmpty(response?.Token))
        {
            return false;
        }

        SetToken(response.Token!, response.RefreshToken!);

        return !string.IsNullOrEmpty(this.Token);
    }

    public async Task<T?> PostAsync<T, V>(string alias, V data)
        where T : class
        where V : class
    {
        var content = new StringContent(
            JsonSerializer.Serialize(data, _jsonSerializerOptions),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(alias, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<T>(result, _jsonSerializerOptions);
            return model;
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) { }
        else
        {
            // Handle other status codes
            throw new Exception($"Error: {response.StatusCode}");
        }
        return default;
    }

    public void SetToken(string token, string refreshToken)
    {
        this.Token = token;
        this.RefreshToken = refreshToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            this.Token
        );
    }

    public bool IsAuthenticated()
    {
        return !string.IsNullOrEmpty(this.Token);
    }
}
