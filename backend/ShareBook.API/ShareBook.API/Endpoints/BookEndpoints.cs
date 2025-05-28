using ShareBook.API.Contracts;
using ShareBook.API.Domain.Entities;
using ShareBook.API.Domain.Enums;
using ShareBook.API.Domain.Repositories;
using ShareBook.API.Services.Abstractions.Contracts;
using ShareBook.API.Services.Abstractions.Extensions;
using ShareBook.API.Services.Abstractions.Services;

namespace ShareBook.API.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        var bookGroup = app.MapGroup("/books").RequireAuthorization().WithTags("Books");

        bookGroup.MapPost(
            "/",
            async (
                BookDto book,
                IUserService userService,
                IBookRepository bookRepository,
                IBookInstanceRepository bookInstanceRepository
            ) =>
            {
                var libraryId = await userService.GetCurrentLibraryIdAsync();
                var existingBook = await bookRepository.ExistsAsync(
                    libraryId,
                    book.Isbn10,
                    book.Isbn13
                );
                if (existingBook is not null)
                {
                    await bookInstanceRepository.CreateAsync(
                        new BookInstance
                        {
                            BookId = existingBook.Book.Id,
                            Id = Guid.NewGuid(),
                            Status = BookInstanceStatus.Available,
                        }
                    );

                    return Results.Ok(
                        new AddBookResult()
                        {
                            BookId = existingBook.Book.Id,
                            Message =
                                "Book already exists in the library. A new instance was added.",
                        }
                    );
                }

                var createdBook = await bookRepository.CreateAsync(book.ToEntity(libraryId));
                await bookInstanceRepository.CreateAsync(
                    new BookInstance
                    {
                        BookId = createdBook.Id,
                        Id = Guid.NewGuid(),
                        Status = BookInstanceStatus.Available,
                    }
                );
                return Results.Created($"/api/books/{createdBook.Id}", createdBook.FromEntity());
            }
        );

        bookGroup.MapGet(
            "/{id:guid}",
            async (Guid id, IBookRepository bookRepository, IUserService userService) =>
            {
                var libraryId = await userService.GetCurrentLibraryIdAsync();
                var book = await bookRepository.GetByIdAsync(libraryId, id);
                return book is not null ? Results.Ok(book.FromEntity()) : Results.NotFound();
            }
        );

        bookGroup.MapGet(
            "/",
            async (
                int pageNumber,
                int pageSize,
                IBookRepository bookRepository,
                IUserService userService
            ) =>
            {
                var libraryId = await userService.GetCurrentLibraryIdAsync();
                var books = await bookRepository.GetAllAsync(libraryId, pageNumber, pageSize);
                return Results.Ok(books.Select(b => b.FromEntity()));
            }
        );

        bookGroup.MapPut(
            "/{id:guid}",
            async (
                Guid id,
                BookDto updatedBook,
                IBookRepository bookRepository,
                IUserService userService
            ) =>
            {
                var libraryId = await userService.GetCurrentLibraryIdAsync();
                var book = await bookRepository.UpdateAsync(updatedBook.ToEntity(libraryId));
                return book is not null ? Results.Ok(book) : Results.NotFound();
            }
        );

        bookGroup.MapDelete(
            "/{id:guid}",
            async (Guid id, IBookRepository bookRepository, IUserService userService) =>
            {
                var libraryId = await userService.GetCurrentLibraryIdAsync();
                var deleted = await bookRepository.DeleteAsync(libraryId, id);
                return deleted ? Results.NoContent() : Results.NotFound();
            }
        );
    }
}
