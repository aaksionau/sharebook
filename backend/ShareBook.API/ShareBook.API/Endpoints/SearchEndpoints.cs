using ShareBook.API.Contracts;
using ShareBook.API.Domain.Entities;
using ShareBook.API.Domain.Repositories;
using ShareBook.API.Services.Abstractions.Contracts;
using ShareBook.API.Services.Abstractions.Extensions;
using ShareBook.API.Services.Abstractions.Helpers;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        var searchGroup = app.MapGroup("/api/search").RequireAuthorization().WithTags("Search");

        searchGroup
            .MapGet(
                "/",
                async (
                    string? isbn,
                    IISBNdbService isbnDbService,
                    IBookRepository bookRepository,
                    IBookInstanceRepository bookInstanceRepository,
                    IClaimsHelper claimsHelper
                ) =>
                {
                    if (string.IsNullOrWhiteSpace(isbn))
                    {
                        return Results.BadRequest("Please provide a search query or an ISBN.");
                    }

                    var libraryId = await claimsHelper.GetCurrentLibraryIdAsync();
                    var localBook = await bookRepository.ExistsAsync(libraryId, isbn, isbn);

                    if (localBook is not null)
                    {
                        return Results.Ok(
                            new SearchResultDto
                            {
                                Book = localBook.Book.FromEntity(),
                                ExistInCurrentLibrary = true,
                                NumberOfCopies = localBook.NumberOfCopies,
                            }
                        );
                    }

                    var book = await isbnDbService.GetBookByIsbn(isbn);

                    var result = new SearchResultDto { Book = book };
                    return Results.Ok(result);
                }
            )
            .WithName("SearchBooks")
            .Produces<BookDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}
