using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Domain.Helpers;

public class BookWithCopies
{
    public Book Book { get; set; }
    public int NumberOfCopies { get; set; }

    public BookWithCopies(Book book, int? numberOfCopies)
    {
        Book = book;
        NumberOfCopies = numberOfCopies.HasValue ? numberOfCopies.Value : 0;
    }
}
