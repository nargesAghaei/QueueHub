using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.MongoDb;
using Infrastructure.Persistence.MongoDb.Repositories;
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

        // MongoDB (exception logging)
        services.Configure<MongoSettings>(configuration.GetSection("Mongo"));
        services.AddSingleton<MongoContext>();
        services.AddScoped<IExceptionLogReadRepository, ExceptionLogReadRepository>();
        services.AddScoped<IExceptionLogWriteRepository, ExceptionLogWriteRepository>();
        

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IRefreshTokenReadRepository, RefreshTokenReadRepository>();
        services.AddScoped<IRefreshTokenWriteRepository, RefreshTokenWriteRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}