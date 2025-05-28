using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Contracts;

public class LibraryDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }
    public bool IsCurrent { get; set; }
}
