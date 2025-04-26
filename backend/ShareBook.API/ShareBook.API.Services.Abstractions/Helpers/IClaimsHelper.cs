using System.Security.Claims;

namespace ShareBook.API.Services.Abstractions.Helpers;

public interface IClaimsHelper
{
    /// <summary>
    /// Adds an admin claim for a specific library ID to the provided user's claims principal.
    /// </summary>
    /// <param name="userClaimsPrinciple">The claims principal of the user to which the admin claim will be added.</param>
    /// <param name="libraryId">The ID of the library for which the admin claim is being added.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the claim was successfully added.</returns>
    Task<bool> AddAdminForLibraryIdClaimAsync(
        ClaimsPrincipal userClaimsPrinciple,
        string libraryId
    );
    Task<string> GetAdminForLibraryIdClaimAsync(ClaimsPrincipal userClaimsPrinciple);
    Task<bool> RemoveAdminForLibraryIdClaimAsync(
        ClaimsPrincipal userClaimsPrinciple,
        string libraryId
    );
    Task<bool> IsAdminForLibraryIdClaimAsync(ClaimsPrincipal userClaimsPrinciple, string libraryId);
}
