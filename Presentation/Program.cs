using Application.Users.Commands.CreateUser;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
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
using QueueHub;
using QueueHub.Application;
using QueueHub.Application.Common;
using QueueHub.Application.Common.Behaviors;
using QueueHub.ErrorHandling;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("========== APPLICATION STARTED ==========");
// Logging
builder.Host.UseSerilog();

// Dependency Injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Authentication
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

//Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerWithJwt();

// Build
var app = builder.Build();
Console.WriteLine("========== APP BUILT ==========");

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
Console.WriteLine("========== APP RUNNING ==========");

app.Run();