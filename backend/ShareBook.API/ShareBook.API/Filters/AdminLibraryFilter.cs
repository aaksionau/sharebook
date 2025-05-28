using ShareBook.API.Services.Abstractions.Services;

namespace ShareBook.API.Filters;

public class AdminLibraryFilter : IEndpointFilter
{
    private readonly IUserService userService;
    private readonly ILogger<AdminLibraryFilter> logger;

    public AdminLibraryFilter(IUserService userService, ILogger<AdminLibraryFilter> logger)
    {
        this.userService = userService;
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

        var isAdmin = await this.userService.IsUserAdminForLibraryAsync(libraryId);
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
