using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ShareBook.API.Services.Abstractions.Helpers;

public class ClaimsHelper : IClaimsHelper
{
    private readonly UserManager<IdentityUser> _userManager;

    public ClaimsHelper(UserManager<IdentityUser> _userManager)
    {
        this._userManager = _userManager;
    }

    private async Task<IdentityUser> GetUserAsync(ClaimsPrincipal user)
    {
        var userId = _userManager.GetUserId(user);
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<bool> UpdateCurrentUserLibraryIdClaimAsync(
        ClaimsPrincipal userClaimsPrinciple,
        string libraryId,
        bool updateExisting = true
    )
    {
        // Check if the claim already exists
        var user = await GetUserAsync(userClaimsPrinciple);
        var existingClaims = await _userManager.GetClaimsAsync(user);
        var existingClaim = existingClaims.FirstOrDefault(c =>
            c.Type == "CurrentLibraryId" && c.Value != null
        );
        if (existingClaim != null && updateExisting)
        {
            // Remove the existing claim
            await _userManager.RemoveClaimAsync(user, existingClaim);
        }

        // Add the new claim
        var claim = new Claim("CurrentLibraryId", libraryId);
        var result = await _userManager.AddClaimAsync(user, claim);
        return result.Succeeded;
    }

    public async Task<bool> AddAdminForLibraryIdClaimAsync(
        ClaimsPrincipal userClaimsPrinciple,
        string libraryId
    )
    {
        // Check if the claim already exists
        var user = await GetUserAsync(userClaimsPrinciple);
        var existingClaims = await _userManager.GetClaimsAsync(user);
        if (existingClaims.Any(c => c.Type == "AdminForLibraryId" && c.Value == libraryId))
        {
            return false; // Claim already exists
        }

        // Add the new claim
        var claim = new Claim("AdminForLibraryId", libraryId);
        var result = await _userManager.AddClaimAsync(user, claim);

        return result.Succeeded;
    }

    public async Task<bool> RemoveAdminForLibraryIdClaimAsync(
        ClaimsPrincipal userClaimsPrinciple,
        string libraryId
    )
    {
        // Check if the claim exists
        var user = await GetUserAsync(userClaimsPrinciple);
        var existingClaims = await _userManager.GetClaimsAsync(user);
        var claimToRemove = existingClaims.FirstOrDefault(c =>
            c.Type == "AdminForLibraryId" && c.Value == libraryId
        );
        if (claimToRemove == null)
        {
            return false; // Claim does not exist
        }
        // Remove the claim
        var result = await _userManager.RemoveClaimAsync(user, claimToRemove);
        return result.Succeeded;
    }

    public async Task<List<string>> GetAdminForLibraryIdsClaimAsync(
        ClaimsPrincipal userClaimsPrinciple
    )
    {
        var user = await GetUserAsync(userClaimsPrinciple);
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.Where(c => c.Type == "AdminForLibraryId").Select(c => c.Value).ToList();
    }

    public async Task<string> GetCurrentLibraryIdClaimAsync(ClaimsPrincipal userClaimsPrinciple)
    {
        var user = await GetUserAsync(userClaimsPrinciple);
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.FirstOrDefault(c => c.Type == "CurrentLibraryId")?.Value ?? string.Empty;
    }

    public async Task<bool> IsAdminForLibraryIdClaimAsync(
        ClaimsPrincipal userClaimsPrinciple,
        string libraryId
    )
    {
        var user = await GetUserAsync(userClaimsPrinciple);
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.Any(c => c.Type == "AdminForLibraryId" && c.Value == libraryId);
    }

    public async Task<string> GetCurrentUserLibraryIdClaimAsync(ClaimsPrincipal userClaimsPrinciple)
    {
        var user = await GetUserAsync(userClaimsPrinciple);
        var claims = await _userManager.GetClaimsAsync(user);
        var claim = claims.FirstOrDefault(c => c.Type == "CurrentLibraryId");
        return claim?.Value ?? string.Empty;
    }
}
