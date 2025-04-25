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

public static class BookDtoExtensions
{
    public static Book ToEntity(this BookDto bookDto, string libraryId)
    {
        if (string.IsNullOrWhiteSpace(libraryId))
            throw new ArgumentException("Library ID cannot be null or empty.", nameof(libraryId));

        return new Book
        {
            LibraryId = libraryId,
            Publisher = bookDto.Publisher,
            Synopsis = bookDto.Synopsis,
            Language = bookDto.Language,
            Image = bookDto.Image,
            ImageOriginal = bookDto.ImageOriginal,
            TitleLong = bookDto.TitleLong,
            Edition = bookDto.Edition,
            Dimensions = bookDto.Dimensions,
            Pages = bookDto.Pages,
            DatePublished = bookDto.DatePublished,
            Subjects = bookDto.Subjects,
            Authors = bookDto.Authors,
            Title = bookDto.Title,
            Isbn13 = bookDto.Isbn13,
            Binding = bookDto.Binding,
            Isbn = bookDto.Isbn,
            Isbn10 = bookDto.Isbn10
        };
    }
    public static BookDto FromEntity(this Book book)
    {
        return new BookDto
        {
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
            Subjects = book.Subjects,
            Authors = book.Authors,
            Title = book.Title,
            Isbn13 = book.Isbn13,
            Binding = book.Binding,
            Isbn = book.Isbn,
            Isbn10 = book.Isbn10
        };
    }
}
