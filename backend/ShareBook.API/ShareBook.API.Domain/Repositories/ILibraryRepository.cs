using System;
using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Domain.Repositories;

public interface ILibraryRepository
{
    Task<Library> CreateAsync(Library library);
    Task<Library?> GetByIdAsync(Guid id);
    Task<List<Library>> GetAllAsync();
    Task<Library?> UpdateAsync(Library library);
    Task<bool> DeleteAsync(Guid id);
}
