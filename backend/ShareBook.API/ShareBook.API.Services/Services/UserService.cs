using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareBook.API.Domain.Repositories;
using ShareBook.API.Services.Abstractions.Services;

namespace ShareBook.API.Services.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> userManager;
    private readonly ILibraryRepository libraryRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserService(
        UserManager<AppUser> userManager,
        ILibraryRepository libraryRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.userManager = userManager;
        this.libraryRepository = libraryRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task AddUserToLibraryAdminsAsync(string libraryId)
    {
        var user = await GetAppUserAsync();

        var library = await libraryRepository.GetByIdAsync(libraryId);

        if (library == null)
        {
            throw new InvalidOperationException($"Library with ID {libraryId} not found.");
        }

        user.Libraries.Add(library);
        await userManager.UpdateAsync(user);
    }

    public async Task RemoveUserFromLibraryAdminsAsync(string libraryId)
    {
        var user = await GetAppUserAsync();

        var library = await libraryRepository.GetByIdAsync(libraryId);

        if (library == null)
        {
            throw new InvalidOperationException($"Library with ID {libraryId} not found.");
        }

        user.Libraries.Remove(library);
        await userManager.UpdateAsync(user);
    }

    public async Task<bool> IsUserAdminForLibraryAsync(string libraryId)
    {
        var user = await GetAppUserAsync();

        return user.Libraries.Any(l => l.Id == libraryId);
    }

    public async Task<List<string>> GetUserLibraryAdminIdsAsync()
    {
        var user = await GetAppUserAsync();

        return user.Libraries.Select(l => l.Id).ToList();
    }

    public async Task<string> GetCurrentLibraryIdAsync()
    {
        var user = await GetAppUserAsync();
        if (string.IsNullOrEmpty(user.CurrentLibraryId))
        {
            throw new InvalidOperationException("Current library ID is not set for the user.");
        }

        return user.CurrentLibraryId;
    }

    public async Task<bool> SetCurrentLibraryAsync(string libraryId)
    {
        var user = await GetAppUserAsync();

        if (!user.Libraries.Any(l => l.Id == libraryId))
        {
            throw new InvalidOperationException($"User is not an admin for library {libraryId}.");
        }

        user.CurrentLibraryId = libraryId;

        var result = await userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    private async Task<AppUser> GetAppUserAsync()
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(
            ClaimTypes.NameIdentifier
        );
        var user = await userManager
            .Users.Include(u => u.Libraries)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        return user;
    }
}
