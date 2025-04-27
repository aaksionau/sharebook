using Microsoft.EntityFrameworkCore;
using ShareBook.API.Domain.Entities;
using ShareBook.API.Domain.Helpers;
using ShareBook.API.Domain.Repositories;

namespace ShareBook.API.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    // Create a new book
    public async Task<Book> CreateAsync(Book book)
    {
        book.Id = Guid.NewGuid();
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    // Get a book by ID
    public async Task<Book?> GetByIdAsync(string libraryId, Guid id)
    {
        return await _context.Books.FirstOrDefaultAsync(b =>
            b.LibraryId == libraryId && b.Id == id
        );
    }

    // Get all books
    public async Task<List<Book>> GetAllAsync(
        string libraryId,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        return await _context
            .Books.Where(b => b.LibraryId == libraryId)
            .OrderBy(b => b.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Update an existing book
    public async Task<Book?> UpdateAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.Id);
        if (existingBook == null)
        {
            return null;
        }

        _context.Entry(existingBook).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync();
        return existingBook;
    }

    // Delete a book by ID
    public async Task<bool> DeleteAsync(string libraryId, Guid id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b =>
            b.LibraryId == libraryId && b.Id == id
        );
        if (book == null)
        {
            return false;
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<BookWithCopies?> ExistsAsync(string libraryId, string isbn10, string isbn13)
    {
        return await _context
            .Books.Include(b => b.BookInstances)
            .Where(b => b.LibraryId == libraryId && (b.Isbn10 == isbn10 || b.Isbn13 == isbn13))
            .Select(b => new { Book = b, NumberOfCopies = b.BookInstances.Count() })
            .FirstOrDefaultAsync()
            .ContinueWith(t =>
                t.Result == null ? null : new BookWithCopies(t.Result.Book, t.Result.NumberOfCopies)
            );
    }
}
