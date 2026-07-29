using Application.Auth;
using Application.Users.Commands.CreateUser;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using QueueHub.Filters;
using QueueHub.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Infrastructure.Services;
using MediatR;
using Microsoft.OpenApi;
using QueueHub.Application;
using QueueHub.Application.Common.Behaviors;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


// Database
builder.Services.AddDbContext<QueueHubDbContext>(option =>
    option.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")));


// Dependency Injection
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateCitizenCommand).Assembly);
});

builder.Services
    .AddValidatorsFromAssembly(
        typeof(AssemblyReference).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

// Authentication
var secretKey = builder.Configuration["Jwt:SecretKey"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey!)),

                ValidateIssuer = false,

                ValidateAudience = false,

                ValidateLifetime = true
            };
    });


// MVC + Swagger
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {token}"
    });

    options.AddSecurityRequirement(document =>
    {
        return new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document),
                new List<string>()
            }
        };
    });
});


// Build
var app = builder.Build();


// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();