using ShareBook.API.Contracts;
using ShareBook.API.Domain.Repositories;
using ShareBook.API.Filters;
using ShareBook.API.Services.Abstractions.Helpers;

namespace ShareBook.API.Endpoints;

public static class LibraryEndpoints
{
    public static void MapLibraryEndpoints(this WebApplication app)
    {
        var libraryGroup = app.MapGroup("/api/libraries")
            .RequireAuthorization()
            .WithTags("Libraries");

        /// <summary>
        /// Create a new library
        /// </summary>
        libraryGroup.MapPost(
            "/",
            async (
                ILibraryRepository libraryRepository,
                LibraryDto library,
                IClaimsHelper claimsHelper,
                HttpContext httpContext
            ) =>
            {
                var createdLibrary = await libraryRepository.CreateAsync(library.ToEntity());
                await claimsHelper.AddAdminForLibraryIdClaimAsync(
                    httpContext.User,
                    createdLibrary.Id
                );
                return Results.Created(
                    $"/api/libraries/{createdLibrary.Id}",
                    createdLibrary.FromEntity()
                );
            }
        );

        /// <summary>
        /// Get a library by ID
        /// </summary>
        libraryGroup
            .MapGet(
                "/{id}",
                async (ILibraryRepository libraryRepository, string id) =>
                {
                    var library = await libraryRepository.GetByIdAsync(id);
                    return library is not null
                        ? Results.Ok(library.FromEntity())
                        : Results.NotFound();
                }
            )
            .AddEndpointFilter<AdminLibraryFilter>();

        /// <summary>
        /// Get all libraries for the current user
        /// </summary>
        libraryGroup.MapGet(
            "/",
            async (
                ILibraryRepository libraryRepository,
                HttpContext httpContext,
                IClaimsHelper claimsHelper
            ) =>
            {
                var adminLibraryId = await claimsHelper.GetAdminForLibraryIdClaimAsync(
                    httpContext.User
                );
                if (string.IsNullOrEmpty(adminLibraryId))
                {
                    return Results.NotFound("No library found for the current user.");
                }
                var libraries = await libraryRepository.GetAllAsync(adminLibraryId);
                return Results.Ok(libraries.Select(l => l.FromEntity()));
            }
        );

        /// <summary>
        /// Update a library by ID
        /// </summary>
        libraryGroup
            .MapPut(
                "/{id}",
                async (
                    string id,
                    LibraryDto updatedLibrary,
                    ILibraryRepository libraryRepository,
                    HttpContext httpContext,
                    IClaimsHelper claimsHelper
                ) =>
                {
                    if (id.ToString() != updatedLibrary.Id)
                    {
                        return Results.BadRequest("ID mismatch.");
                    }
                    var library = await libraryRepository.UpdateAsync(updatedLibrary.ToEntity());
                    return library is not null ? Results.Ok(library) : Results.NotFound();
                }
            )
            .AddEndpointFilter<AdminLibraryFilter>();

        libraryGroup
            .MapDelete(
                "/{id}",
                async (
                    string id,
                    ILibraryRepository libraryRepository,
                    IClaimsHelper claimsHelper,
                    HttpContext httpContext
                ) =>
                {
                    var deleted = await libraryRepository.DeleteAsync(id);
                    return deleted ? Results.NoContent() : Results.NotFound();
                }
            )
            .AddEndpointFilter<AdminLibraryFilter>();
    }
}
