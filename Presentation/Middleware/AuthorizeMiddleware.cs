using Application.Auth;

namespace QueueHub.Middleware;

public class AuthorizeMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        Console.WriteLine("Middleware: " + context.Request.Path);
        if (context.Request.Path == "/" ||
            context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next(context);
            return;
        }
        
        var endpoint = context.GetEndpoint();
        var routeValue = context.Request.RouteValues;
        var controllerName = routeValue["controller"]?.ToString();
        var actionName = routeValue["action"]?.ToString();

        if (controllerName == "Auth" &&
            actionName is "Login" or "Register")
        {
            await next(context);
            return;
        }

        var userGuid = context.Request.Headers["Authorization"]
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(userGuid))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Guid is required.");
            return;
        }

        if (!Guid.TryParse(userGuid, out var guid))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid Guid format.");
            return;
        }

        var valid = await authService.IsValidGuid(guid, context.RequestAborted);

        if (!valid)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid Guid.");
            return;
        }

        await next(context);
    }
}