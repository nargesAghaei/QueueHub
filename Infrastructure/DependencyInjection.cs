using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        // Database
        services.AddDbContext<QueueHubDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}