using ShareBook.API.Domain.Enums;

namespace ShareBook.API.Domain.Entities;

public class BookInstance
{
    public required Guid Id { get; set; }
    public required Guid BookId { get; set; }
    public virtual Book Book { get; set; }
    public BookInstanceStatus Status { get; set; }
}
