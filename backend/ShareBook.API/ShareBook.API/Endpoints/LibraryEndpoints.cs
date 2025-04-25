using System;
using ShareBook.API.Contracts;
using ShareBook.API.Domain.Repositories;

namespace ShareBook.API.Endpoints;

public static class LibraryEndpoints
{
    public static void MapLibraryEndpoints(this WebApplication app)
    {
        var libraryGroup = app.MapGroup("/api/libraries")
            .WithTags("Libraries");

        libraryGroup.MapPost("/", async (ILibraryRepository libraryRepository, LibraryDto library) =>
        {
            var adminId = ""; // TODO: Get the admin ID from the authenticated user or context
            var createdLibrary = await libraryRepository.CreateAsync(library.ToEntity(adminId));
            return Results.Created($"/api/libraries/{createdLibrary.Id}", createdLibrary);
        });
        libraryGroup.MapGet("/{id:guid}", async (ILibraryRepository libraryRepository, Guid id) =>
            {
                var library = await libraryRepository.GetByIdAsync(id);
                return library is not null ? Results.Ok(library) : Results.NotFound();
            });
        libraryGroup.MapGet("/", async (ILibraryRepository libraryRepository) =>
        {
            var libraries = await libraryRepository.GetAllAsync();
            return Results.Ok(libraries);
        });
        libraryGroup.MapPut("/{id:guid}", async (ILibraryRepository libraryRepository, Guid id, LibraryDto updatedLibrary) =>
        {
            if (id.ToString() != updatedLibrary.Id)
            {
                return Results.BadRequest("ID mismatch.");
            }
            var adminId = ""; // TODO: Get the admin ID from the authenticated user or context
            var library = await libraryRepository.UpdateAsync(updatedLibrary.ToEntity(adminId));
            return library is not null ? Results.Ok(library) : Results.NotFound();
        });
        libraryGroup.MapDelete("/{id:guid}", async (ILibraryRepository libraryRepository, Guid id) =>
        {
            var deleted = await libraryRepository.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
