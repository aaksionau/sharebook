using ShareBook.API.Contracts;
using ShareBook.API.Services;

public interface IISBNdbService
{
    Task<BookDto?> GetBookByIsbn(string isbn);
}