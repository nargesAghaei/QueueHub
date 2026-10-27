using Application.Interfaces.Authentication;
using Application.Users.Authentication;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QueueHub.Application;
using QueueHub.Application.Common.Behaviors;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssemblyContaining<AssemblyReference>();
        services.AddScoped<IAuthSessionService, AuthSessionService>();

    }
}