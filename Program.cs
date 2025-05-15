// Program.cs
using Article_Review_System_backend.Data;
using Article_Review_System_backend.Repository;
using Article_Review_System_backend.Repository.User;
using Article_Review_System_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Article Review System API is running!");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();