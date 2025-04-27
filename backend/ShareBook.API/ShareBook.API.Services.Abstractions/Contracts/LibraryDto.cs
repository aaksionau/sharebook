using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Contracts;

public class LibraryDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }
    public bool IsCurrent { get; set; }
}

public static class LibraryDtoExtensions
{
    // Convert LibraryDto to Library entity
    public static Library ToEntity(this LibraryDto libraryDto)
    {
        return new Library
        {
            Id = libraryDto.Id,
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
