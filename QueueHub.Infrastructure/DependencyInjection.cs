using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.ExceptionLogInterfaces;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.MongoDb;
using Infrastructure.Persistence.MongoDb.Repositories;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.OrganizationRepo;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.UserRepo;
using Infrastructure.Persistence.PostgreSql.Seeding;
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
        services.AddScoped<IOrganizationWriteRepository, OrganizationWriteRepository>();
        services.AddScoped<IOrganizationReadRepository, OrganizationReadRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<LocationSeeder>();
    }
}