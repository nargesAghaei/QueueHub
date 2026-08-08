using Application.Auth;
using Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QueueHub.Application.Common.Behaviors;

namespace QueueHub.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AssemblyReference>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssemblyContaining<AssemblyReference>();
        

    }
}