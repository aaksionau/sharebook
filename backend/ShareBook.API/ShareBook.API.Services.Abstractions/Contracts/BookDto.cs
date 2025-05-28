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

    public static BookDto FromEntity(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Publisher = book.Publisher,
            Synopsis = book.Synopsis,
            Language = book.Language,
            Image = book.Image,
            ImageOriginal = book.ImageOriginal,
            TitleLong = book.TitleLong,
            Edition = book.Edition,
            Dimensions = book.Dimensions,
            Pages = book.Pages,
            DatePublished = book.DatePublished,
            Subjects = book.Subjects?.ToList(),
            Authors = book.Authors?.ToList(),
            Title = book.Title,
            Isbn13 = book.Isbn13,
            Binding = book.Binding,
            Isbn = book.Isbn,
            Isbn10 = book.Isbn10,
        };
    }
}
