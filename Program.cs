using Article_Review_System_backend.Data;
using Article_Review_System_backend.Repository;
using Article_Review_System_backend.Repository.User;
using Article_Review_System_backend.Services;
using Article_Review_System_backend.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Console.WriteLine("connection: " + 
                  builder.Configuration.GetConnectionString("DefaultConnection"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string is not set");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapPost("/api/auth/register", async (RegisterRequest request, IAuthService authService) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(request);

    if (!Validator.TryValidateObject(request, context, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    try
    {
        var response = await authService.RegisterAsync(request);
        return Results.Ok(response);
    }
    catch (ApplicationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
}).Accepts<RegisterRequest>("application/json");

app.MapPost("/api/auth/login", async (LoginRequest request, IAuthService authService) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(request);

    if (!Validator.TryValidateObject(request, context, validationResults, true))
    {
        return Results.BadRequest(validationResults);
    }

    try
    {
        var response = await authService.LoginAsync(request);
        return Results.Ok(response);
    }
    catch (ApplicationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
}).Accepts<LoginRequest>("application/json");

app.MapGet("/", () => "Article Review System API is running!");

app.Run();
