using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ShareBook.API.Services.Abstractions.Helpers;

public class ClaimsHelper : IClaimsHelper
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ClaimsHelper(
        UserManager<IdentityUser> _userManager,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._userManager = _userManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> UpdateCurrentUserLibraryIdAsync(
        string libraryId,
        bool updateExisting = true
    )
    {
        // Check if the claim already exists
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
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

    public async Task<bool> AddAdminForLibraryIdCAsync(string libraryId)
    {
        // Check if the claim already exists
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
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

    public async Task<bool> RemoveAdminForLibraryIdAsync(string libraryId)
    {
        // Check if the claim exists
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
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

    public async Task<List<string>> GetAdminForLibraryIdsAsync()
    {
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.Where(c => c.Type == "AdminForLibraryId").Select(c => c.Value).ToList();
    }

    public async Task<string> GetCurrentLibraryIdAsync()
    {
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.FirstOrDefault(c => c.Type == "CurrentLibraryId")?.Value ?? string.Empty;
    }

    public async Task<bool> IsAdminForLibraryIdAsync(string libraryId)
    {
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.Any(c => c.Type == "AdminForLibraryId" && c.Value == libraryId);
    }

    public async Task<string> GetCurrentUserLibraryIdAsync()
    {
        var user = await _userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
        var claims = await _userManager.GetClaimsAsync(user);
        var claim = claims.FirstOrDefault(c => c.Type == "CurrentLibraryId");
        return claim?.Value ?? string.Empty;
    }
}
