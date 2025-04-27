using ShareBook.API.Domain.Entities;
using ShareBook.API.Domain.Helpers;

namespace ShareBook.API.Domain.Repositories;

public interface IBookRepository
{
    Task<Book> CreateAsync(Book book);
    Task<Book?> GetByIdAsync(string libraryId, Guid id);
    Task<List<Book>> GetAllAsync(string libraryId, int pageNumber = 1, int pageSize = 10);
    Task<Book?> UpdateAsync(Book book);
    Task<bool> DeleteAsync(string libraryId, Guid id);
    Task<BookWithCopies> ExistsAsync(string libraryId, string isbn10, string isbn13);
}
