using Application.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QueueHub.Filters;

public class AuthorizeActionFilter(IAuthService authService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controllerName =
            context.RouteData.Values["controller"]?.ToString();

        var actionName =
            context.RouteData.Values["action"]?.ToString();

        if (controllerName == "Auth" &&
            (actionName == "Login" ||
             actionName == "Register"))
        {
            await next();
            return;
        }
        var userGuid = context.HttpContext.Request.Headers["Authorization"]
            .FirstOrDefault();

        var guid = Guid.Parse(userGuid);
        var valid = await authService.IsValidGuid(guid, context.HttpContext.RequestAborted);
        if (!valid)
        {
            context.Result =
                new UnauthorizedObjectResult("Unauthorized.");
            return;
        }

        await next();
    }
}