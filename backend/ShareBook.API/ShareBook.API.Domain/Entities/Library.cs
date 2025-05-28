using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ShareBook.API.Domain.Entities;

public class Library : Audit
{
    [StringLength(128)]
    public required string Id { get; set; }

    [StringLength(512)]
    public required string Name { get; set; }

    [StringLength(512)]
    public string? Address { get; set; }
    public Collection<Book> Books { get; set; } = new Collection<Book>();
    public Collection<AppUser> Administrators { get; set; } = new Collection<AppUser>();
}
