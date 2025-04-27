namespace ShareBook.API.Services.Abstractions.Helpers;

public interface IClaimsHelper
{
    Task<bool> UpdateCurrentUserLibraryIdAsync(string libraryId, bool updateExisting = true);
    Task<bool> AddAdminForLibraryIdCAsync(string libraryId);
    Task<bool> RemoveAdminForLibraryIdAsync(string libraryId);
    Task<List<string>> GetAdminForLibraryIdsAsync();
    Task<string> GetCurrentLibraryIdAsync();
    Task<bool> IsAdminForLibraryIdAsync(string libraryId);
    Task<string> GetCurrentUserLibraryIdAsync();
}
