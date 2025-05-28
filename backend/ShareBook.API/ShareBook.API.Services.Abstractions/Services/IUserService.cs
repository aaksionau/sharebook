using System;

namespace ShareBook.API.Services.Abstractions.Services;

public interface IUserService
{
    Task AddUserToLibraryAdminsAsync(string libraryId);
    Task RemoveUserFromLibraryAdminsAsync(string libraryId);
    Task<bool> IsUserAdminForLibraryAsync(string libraryId);
    Task<List<string>> GetUserLibraryAdminIdsAsync();
    Task<bool> SetCurrentLibraryAsync(string libraryId);
    Task<string> GetCurrentLibraryIdAsync();
}
