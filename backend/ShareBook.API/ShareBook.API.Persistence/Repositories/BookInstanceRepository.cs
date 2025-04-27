using ShareBook.API.Domain.Entities;
using ShareBook.API.Domain.Repositories;

namespace ShareBook.API.Persistence.Repositories;

public class BookInstanceRepository : IBookInstanceRepository
{
    private readonly AppDbContext _context;

    public BookInstanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookInstance> CreateAsync(BookInstance bookInstance)
    {
        _context.BookInstances.Add(bookInstance);
        await _context.SaveChangesAsync();
        return bookInstance;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bookInstance = await _context.BookInstances.FindAsync(id);
        if (bookInstance == null)
        {
            return false;
        }

        _context.BookInstances.Remove(bookInstance);
        await _context.SaveChangesAsync();
        return true;
    }
}
