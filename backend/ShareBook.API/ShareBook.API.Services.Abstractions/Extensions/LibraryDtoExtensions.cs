using System;
using ShareBook.API.Contracts;
using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Services.Abstractions.Extensions;

public static class LibraryDtoExtensions
{
    // Convert LibraryDto to Library entity
    public static Library ToEntity(this LibraryDto libraryDto)
    {
        return new Library
        {
            Id = string.IsNullOrWhiteSpace(libraryDto.Id)
                ? Guid.NewGuid().ToString()
                : libraryDto.Id,
            Name = libraryDto.Name,
            Address = libraryDto.Address,
        };
    }

    // Convert Library entity to LibraryDto
    public static LibraryDto FromEntity(this Library library)
    {
        return new LibraryDto
        {
            Id = library.Id.ToString(),
            Name = library.Name,
            Address = library.Address,
        };
    }
}
