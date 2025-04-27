using System;
using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Domain.Repositories;

public interface ILibraryRepository
{
    Task<Library> CreateAsync(Library library);
    Task<Library?> GetByIdAsync(string id);
    Task<List<Library>> GetAllAsync(List<string> libraryIds);
    Task<Library?> UpdateAsync(Library library);
    Task<bool> DeleteAsync(string id);
}
