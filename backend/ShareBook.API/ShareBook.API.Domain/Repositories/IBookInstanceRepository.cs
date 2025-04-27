using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Domain.Repositories;

public interface IBookInstanceRepository
{
    Task<BookInstance> CreateAsync(BookInstance bookInstance);
    Task<bool> DeleteAsync(Guid id);
}
