using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Domain.Repositories;

public interface IBookRepository
{
    Task<Book> CreateAsync(Book book);
    Task<Book?> GetByIdAsync(Guid id);
    Task<List<Book>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<Book?> UpdateAsync(Book book);
    Task<bool> DeleteAsync(Guid id);
}
