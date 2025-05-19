using Article_Review_System_backend.Data;
using Article_Review_System_backend.Repository;
using Article_Review_System_backend.Repository.User;
using Article_Review_System_backend.Repository.Review;
using Article_Review_System_backend.Services;
using Article_Review_System_backend.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Article_Review_System_backend.Models;
using System.Text.Json;



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

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

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

app.MapPost("/api/user/avatar", async (IFormFile file, IUserRepository userRepository, int userId) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded");
    }

    var user = await userRepository.GetUserByIdAsync(userId);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
    var filePath = Path.Combine("wwwroot/avatars", fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    user.AvatarUrl = $"/avatars/{fileName}";
    await userRepository.UpdateUserAsync(user);

    return Results.Ok(new { AvatarUrl = user.AvatarUrl });
});
app.MapPost("/api/users/reviews", async (ReviewRequest review, IReviewService reviewService, int articleId) =>
{
    await reviewService.SubmitReviewAsync(review);

    return Results.Ok(new { ReviewerId = review.ReviewerId });
});
/*
app.MapPost("/api/user/{userId}/reviews", async (int reviewId, IUserRepository userRepository, int userId) =>
{
    var user = await userRepository.GetUserByIdAsync(userId);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    user.Reviews.Add(reviewId);
    await userRepository.UpdateUserAsync(user);

    return Results.Ok(new { Reviews = user.Reviews });
});
app.MapPost("/api/users/{userId}/reviews-text", async (List<int> reviewText, IUserRepository userRepository, int userId) =>
{
    var user = await userRepository.GetUserByIdAsync(userId);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    user.ReviewsFinished = reviewText;
    await userRepository.UpdateUserAsync(user);

    return Results.Ok(new { ReviewsYf = user.ReviewsFinished });
});
*/
app.MapPut("/api/user/profile", async (ProfileUpdateRequest request, IUserRepository userRepository) =>
{
    var user = await userRepository.GetUserByIdAsync(request.Id);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.Email = request.Email;
    user.Specialization = request.Specialization;
    user.Location = request.Location;
    user.Bio = request.Bio;
    user.Twitter = request.Twitter;
    user.LinkedIn = request.LinkedIn;
    user.UpdatedAt = DateTime.UtcNow;

    await userRepository.UpdateUserAsync(user);

    return Results.Ok(new
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        Role = user.Role,
        Specialization = user.Specialization,
        Location = user.Location,
        Bio = user.Bio,
        Twitter = user.Twitter,
        LinkedIn = user.LinkedIn,
        AvatarUrl = user.AvatarUrl,
    });
}).Accepts<ProfileUpdateRequest>("application/json");

app.MapPost("/api/articles/submit", async (ArticleRequest request, IArticleService articleService) =>
{
    try
    {
        var article = await articleService.SubmitArticleAsync(request);
        return Results.Ok(new
        {
            Id = article.Id,
            Title = article.Title,
            SubmittedDate = article.SubmittedDate.ToString("MMMM d, yyyy"),
            Category = article.Category,
            Status = article.Status
        });
    }
    catch (ApplicationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
}).Accepts<ArticleRequest>("application/json");

app.MapGet("/api/articles/my-articles/{authorId}", async (int authorId, IArticleService articleService) =>
{
    try
    {
        var articles = await articleService.GetUserArticlesAsync(authorId);
        return Results.Ok(articles);
    }
    catch (ApplicationException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
});

app.MapGet("/api/users", async (AppDbContext context) =>
{
    try
    {
        var users = await context.Users
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.AvatarUrl,
                u.Gender,
                u.IsBlocked,
                u.Reviews,
                u.ReviewsFinished,
            })
            .ToListAsync();

        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
});
app.MapGet("/api/articles", async (AppDbContext context) =>
{
    try
    {
        var articles = await context.Articles
            .Select(u => new
            {
                u.Id,
            })
            .ToListAsync();

        return Results.Ok(articles);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
});
app.MapGet("/api/reviews", async (AppDbContext context) =>
{
    try
    {
        var reviews = await context.Reviews
            .Select(u => new
            {
                u.Id,
                u.ReviewerId,
                u.ArticleId,
                u.ArticleBase,
                u.Author,
                u.Status,
                u.DueDate,
                u.Rating,
                u.TechnicalMerit,
                u.Originality,
                u.PresentationQuality,
                u.CommentsToAuthor,
                u.CommentsToEditor,
                u.AttachmentUrl,
            })
            .ToListAsync();

        return Results.Ok(reviews);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
});
app.MapPut("/api/users/{id}/block", async (int id, AppDbContext context, HttpRequest request) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null)
        return Results.NotFound("User not found");

    var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
    var json = JsonSerializer.Deserialize<Dictionary<string, bool>>(requestBody);

    if (json == null || !json.ContainsKey("isBlocked"))
        return Results.BadRequest("Missing isBlocked property");

    user.IsBlocked = json["isBlocked"];
    await context.SaveChangesAsync();

    return Results.Ok();
});

app.MapPut("/api/users/{id}/role", async (int id, AppDbContext context, HttpRequest request) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null)
        return Results.NotFound("User not found");

    var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
    var json = JsonSerializer.Deserialize<Dictionary<string, string>>(requestBody);

    if (json == null || !json.ContainsKey("role"))
        return Results.BadRequest("Missing role property");

    user.Role = json["role"];
    await context.SaveChangesAsync();

    return Results.Ok();
});


app.MapPost("/api/articles/upload-image", async (IFormFile file) =>
{
    try
    {
        if (file == null || file.Length == 0)
            return Results.BadRequest("No file uploaded");

        var uploadsDir = Path.Combine("wwwroot", "article-images");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        return Results.Ok(new { imageUrl = $"/article-images/{fileName}" });
    }
    catch
    {
        return Results.StatusCode(500);
    }
}).DisableAntiforgery();

app.MapPost("/api/articles/upload-file", async (IFormFile file) =>
{
    try
    {
        if (file == null || file.Length == 0)
            return Results.BadRequest("No file uploaded");

        var uploadsDir = Path.Combine("wwwroot", "article-files");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        return Results.Ok(new
        {
            fileUrl = $"/article-files/{fileName}",
            originalFileName = file.FileName
        });
    }
    catch
    {
        return Results.StatusCode(500);
    }
}).DisableAntiforgery();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userRepository = services.GetRequiredService<IUserRepository>();
    var configuration = services.GetRequiredService<IConfiguration>();

    await AdminSeeder.SeedAdminAsync(userRepository, configuration);
}

app.UseStaticFiles();

app.MapGet("/", () => "Article Review System API is running!");

app.Run();