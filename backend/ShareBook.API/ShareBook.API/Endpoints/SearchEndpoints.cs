using Microsoft.AspNetCore.Mvc;
using ShareBook.API.Contracts;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        var searchGroup = app.MapGroup("/api/search").WithTags("Search");

        searchGroup
            .MapGet(
                "/",
                async (string? query, string? isbn, [FromServices] IISBNdbService isbnDbService) =>
                {
                    if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(isbn))
                    {
                        return Results.BadRequest("Please provide a search query or an ISBN.");
                    }

                    if (!string.IsNullOrWhiteSpace(isbn))
                    {
                        var book = await isbnDbService.GetBookByIsbn(isbn);
                        return book is not null ? Results.Ok(book) : Results.NotFound();
                    }

                    return Results.BadRequest("Invalid search parameters.");
                }
            )
            .WithName("SearchBooks")
            .Produces<BookDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}
