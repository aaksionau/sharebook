using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ShareBook.API.Domain.Entities;

public class AppUser : IdentityUser
{
    public virtual List<Library> Libraries { get; set; } = new List<Library>();

    [MaxLength(128)]
    public string? CurrentLibraryId { get; set; }
}
