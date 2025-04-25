using System.ComponentModel.DataAnnotations;

namespace ShareBook.API.Domain.Entities;

public abstract class Audit
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [StringLength(128)]
    public string? CreatedBy { get; set; }
    [StringLength(128)]
    public string? UpdatedBy { get; set; }
}
