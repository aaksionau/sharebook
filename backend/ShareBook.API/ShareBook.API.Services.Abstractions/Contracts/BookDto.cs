using System.Text.Json.Serialization;
using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Contracts;

public class BookDto
{
    public Guid Id { get; set; }
    public string? Publisher { get; set; }
    public string? Synopsis { get; set; }
    public string? Language { get; set; }
    public string? Image { get; set; }

    [JsonPropertyName("image_original")]
    public string? ImageOriginal { get; set; }

    [JsonPropertyName("title_long")]
    public string? TitleLong { get; set; }
    public string? Edition { get; set; }
    public string? Dimensions { get; set; }
    public DimensionsStructured? DimensionsStructured { get; set; }
    public int Pages { get; set; }
    public DateTime DatePublished { get; set; }
    public List<string>? Subjects { get; set; }
    public List<string>? Authors { get; set; }
    public required string Title { get; set; }
    public string? Isbn13 { get; set; }
    public string? Binding { get; set; }
    public string? Isbn { get; set; }
    public string? Isbn10 { get; set; }
}
