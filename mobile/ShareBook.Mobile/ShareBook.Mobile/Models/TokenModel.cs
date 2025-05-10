using System.Text.Json.Serialization;

namespace ShareBook.Mobile.Models;

public class TokenModel
{
    [JsonPropertyName("accessToken")]
    public string? Token { get; set; }
    [JsonPropertyName("refreshToken")]
    public string? Refresh { get; set; }
}