using Application.Auth;
using Application.Users.Commands.CreateUser;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using QueueHub.Filters;
using QueueHub.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QueueHubDbContext>(option=>
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<AuthorizeActionFilter>();

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreateCitizenCommand).Assembly); });

builder.Services.AddControllers(options => { options.Filters.Add(typeof(AuthorizeActionFilter)); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<AuthorizeMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();