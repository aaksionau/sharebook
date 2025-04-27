using System.Collections.ObjectModel;
using ShareBook.API.Contracts;
using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Services.Abstractions.Extensions;

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
            Isbn10 = bookDto.Isbn10,
            BookInstances = new Collection<BookInstance>(),
        };
    }

    public static BookDto FromEntity(this Book book)
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
            Subjects = book.Subjects,
            Authors = book.Authors,
            Title = book.Title,
            Isbn13 = book.Isbn13,
            Binding = book.Binding,
            Isbn = book.Isbn,
            Isbn10 = book.Isbn10,
        };
    }
}
