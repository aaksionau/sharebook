using ShareBook.API.Services.Abstractions.Helpers;

namespace ShareBook.API.Filters;

public class AdminLibraryFilter : IEndpointFilter
{
    private readonly IClaimsHelper claimsHelper;
    private readonly ILogger<AdminLibraryFilter> logger;

    public AdminLibraryFilter(IClaimsHelper claimsHelper, ILogger<AdminLibraryFilter> logger)
    {
        this.claimsHelper = claimsHelper;
        this.logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var libraryId = context.HttpContext.Request.RouteValues["id"]?.ToString();

        if (string.IsNullOrEmpty(libraryId))
        {
            return Results.BadRequest("Library ID is required.");
        }

        var isAdmin = await this.claimsHelper.IsAdminForLibraryIdClaimAsync(
            context.HttpContext.User,
            libraryId
        );
        if (!isAdmin)
        {
            this.logger.LogWarning(
                "User {UserId} is not an admin for library ID: {LibraryId}",
                context.HttpContext.User.Identity?.Name,
                libraryId
            );
            return Results.Forbid();
        }

        return await next(context);
    }
}
