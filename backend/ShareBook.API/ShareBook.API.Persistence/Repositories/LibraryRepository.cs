using Microsoft.EntityFrameworkCore;
using ShareBook.API.Domain.Entities;
using ShareBook.API.Domain.Repositories;

namespace ShareBook.API.Persistence.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly AppDbContext _context;

    public LibraryRepository(AppDbContext context)
    {
        _context = context;
    }

    // Create a new library
    public async Task<Library> CreateAsync(Library library)
    {
        _context.Libraries.Add(library);
        await _context.SaveChangesAsync();
        return library;
    }

    // Get a library by ID
    public async Task<Library?> GetByIdAsync(Guid id)
    {
        return await _context.Libraries
            .FirstOrDefaultAsync(l => l.Id == id.ToString());
    }

    // Get all libraries
    public async Task<List<Library>> GetAllAsync()
    {
        return await _context.Libraries
            .ToListAsync();
    }

    // Update an existing library
    public async Task<Library?> UpdateAsync(Library library)
    {
        var existingLibrary = await _context.Libraries.FindAsync(library.Id);
        if (existingLibrary == null)
        {
            return null;
        }

        _context.Entry(existingLibrary).CurrentValues.SetValues(library);
        await _context.SaveChangesAsync();
        return existingLibrary;
    }

    // Delete a library by ID
    public async Task<bool> DeleteAsync(Guid id)
    {
        var library = await _context.Libraries.FindAsync(id);
        if (library == null)
        {
            return false;
        }

        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
        return true;
    }
}
