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
using QueueHub;
using QueueHub.Application;
using QueueHub.Application.Common.Behaviors;
using QueueHub.ErrorHandling;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog();

// Dependency Injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Authentication
var secretKey = builder.Configuration["Jwt:SecretKey"];

//Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerWithJwt();

// Build
var app = builder.Build();


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

app.Run();