using System.Collections.Generic;
using System.Net.Http;
using System.Configuration;
using System.Text.Json;
using System.Threading.Tasks;
using ShareBook.Domain.Models;
using System.Linq;
using ShareBook.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ShareBook.GoogleBooks
{
    public class GoogleBookService : IInternetBookService
    {
        private readonly IHttpClientFactory _factory;
        private readonly HttpClient _client;
        private string _apiKey;
        public GoogleBookService(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.BaseAddress = new System.Uri("https://www.googleapis.com/books/v1/volumes");
            _client.DefaultRequestHeaders.Add("UserAgent", "GoogleBooks");

            _apiKey = config["BookSearchServiceKeys:GoogleBooks"];
        }
        public async Task<IEnumerable<BookDetails>> SearchAsync(InternetBookSearchParams searchParams)
        {
            string requestUri = $"?q={searchParams.Isbn}+isbn:{searchParams.Isbn}&key={_apiKey}";
            var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonBooksModel>(responseBody, options: new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            return result.Items.Select(b => new BookDetails()
            {
                Title = b.VolumeInfo.Title
            });
        }
    }
}