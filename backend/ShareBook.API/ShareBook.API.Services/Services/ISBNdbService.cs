using System.Text.Json;
using Microsoft.Extensions.Logging;
using ShareBook.API.Contracts;

public class ISBNdbService : IISBNdbService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ISBNdbService> _logger;
    private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ISBNdbService(HttpClient httpClient, ILogger<ISBNdbService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<BookDto?> GetBookByIsbn(string isbn)
    {
        try
        {
            var response = await _httpClient.GetAsync($"book/{isbn}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IsbnResponse>(content, this._jsonSerializerOptions)?.Book;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching book data from ISBNdb");
            return null;
        }
    }
}