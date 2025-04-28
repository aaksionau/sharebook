using ShareBook.API.Contracts;
using ShareBook.API.Domain.Repositories;
using ShareBook.API.Filters;
using ShareBook.API.Services.Abstractions.Helpers;

namespace ShareBook.API.Endpoints;

public static class LibraryEndpoints
{
    public static void MapLibraryEndpoints(this WebApplication app)
    {
        var libraryGroup = app.MapGroup("/libraries").RequireAuthorization().WithTags("Libraries");

        /// <summary>
        /// Create a new library
        /// </summary>
        libraryGroup.MapPost(
            "/",
            async (
                ILibraryRepository libraryRepository,
                LibraryDto library,
                IClaimsHelper claimsHelper
            ) =>
            {
                // TODO: Check if the library already exists
                var createdLibrary = await libraryRepository.CreateAsync(library.ToEntity());
                await claimsHelper.AddAdminForLibraryIdCAsync(createdLibrary.Id);
                await claimsHelper.UpdateCurrentUserLibraryIdAsync(createdLibrary.Id, false);
                return Results.Created(
                    $"/api/libraries/{createdLibrary.Id}",
                    createdLibrary.FromEntity()
                );
            }
        );

        /// <summary>
        /// Set the current library for the authenticated user
        /// </summary>
        /// <remarks>
        /// This endpoint updates the current library ID claim for the authenticated user.
        /// It is used to set the current library for the user.
        /// Id is to check if the user is an admin for the library.
        /// </remarks>
        libraryGroup
            .MapPost(
                "/{id}/current/",
                async (string id, IClaimsHelper claimsHelper) =>
                {
                    var result = await claimsHelper.UpdateCurrentUserLibraryIdAsync(id);
                    return result
                        ? Results.Ok()
                        : Results.Problem("Failed to update current library.");
                }
            )
            .AddEndpointFilter<AdminLibraryFilter>();

        /// <summary>
        /// Get a library by ID
        /// </summary>
        libraryGroup.MapGet(
            "/{id}",
            async (ILibraryRepository libraryRepository, string id, IClaimsHelper claimsHelper) =>
            {
                var library = await libraryRepository.GetByIdAsync(id);
                if (library is null)
                {
                    return Results.NotFound();
                }
                var currentLibrary = await claimsHelper.GetCurrentLibraryIdAsync();
                var result = library.FromEntity();
                result.IsCurrent = result.Id == currentLibrary;
                return library is not null ? Results.Ok(result) : Results.NotFound();
            }
        );

        /// <summary>
        /// Get all libraries for the current user
        /// </summary>
        libraryGroup.MapGet(
            "/",
            async (ILibraryRepository libraryRepository, IClaimsHelper claimsHelper) =>
            {
                var adminLibraryIds = await claimsHelper.GetAdminForLibraryIdsAsync();
                if (!adminLibraryIds.Any())
                {
                    return Results.NotFound("No library found for the current user.");
                }
                var currentLibrary = await claimsHelper.GetCurrentLibraryIdAsync();
                var libraries = await libraryRepository.GetAllAsync(adminLibraryIds);
                var dtos = libraries.Select(x => x.FromEntity()).ToList();
                dtos.ForEach(l =>
                {
                    l.IsCurrent = l.Id == currentLibrary;
                });
                return Results.Ok(dtos);
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
                    IClaimsHelper claimsHelper
                ) =>
                {
                    if (id.ToString() != updatedLibrary.Id)
                    {
                        return Results.BadRequest("ID mismatch.");
                    }
                    var library = await libraryRepository.UpdateAsync(updatedLibrary.ToEntity());
                    return library is not null
                        ? Results.Ok(library.FromEntity())
                        : Results.NotFound();
                }
            )
            .AddEndpointFilter<AdminLibraryFilter>();

        libraryGroup
            .MapDelete(
                "/{id}",
                async (
                    string id,
                    ILibraryRepository libraryRepository,
                    IClaimsHelper claimsHelper
                ) =>
                {
                    var deleted = await libraryRepository.DeleteAsync(id);
                    return deleted ? Results.NoContent() : Results.NotFound();
                }
            )
            .AddEndpointFilter<AdminLibraryFilter>();
    }
}
