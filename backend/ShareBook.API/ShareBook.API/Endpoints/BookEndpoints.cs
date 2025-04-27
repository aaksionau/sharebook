using System;
using ShareBook.API.Contracts;
using ShareBook.API.Domain.Repositories;

namespace ShareBook.API.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        var bookGroup = app.MapGroup("/api/books").RequireAuthorization().WithTags("Books");

        bookGroup.MapPost(
            "/",
            async (IBookRepository bookRepository, BookDto book) =>
            {
                // TODO: Get the library ID from the authenticated user or context
                var libraryId = "";
                var createdBook = await bookRepository.CreateAsync(book.ToEntity(libraryId));
                return Results.Created($"/api/books/{createdBook.Id}", createdBook);
            }
        );

        bookGroup.MapGet(
            "/{id:guid}",
            async (IBookRepository bookRepository, Guid id) =>
            {
                var book = await bookRepository.GetByIdAsync(id);
                return book is not null ? Results.Ok(book) : Results.NotFound();
            }
        );

        bookGroup.MapGet(
            "/",
            async (IBookRepository bookRepository, int pageNumber = 1, int pageSize = 10) =>
            {
                var books = await bookRepository.GetAllAsync(pageNumber, pageSize);
                return Results.Ok(books);
            }
        );

        bookGroup.MapPut(
            "/{id:guid}",
            async (IBookRepository bookRepository, Guid id, BookDto updatedBook) =>
            {
                var libraryId = ""; // TODO: Get the library ID from the authenticated user or context
                var book = await bookRepository.UpdateAsync(updatedBook.ToEntity(libraryId));
                return book is not null ? Results.Ok(book) : Results.NotFound();
            }
        );

        bookGroup.MapDelete(
            "/{id:guid}",
            async (IBookRepository bookRepository, Guid id) =>
            {
                var deleted = await bookRepository.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            }
        );
    }
}
