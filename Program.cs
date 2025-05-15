//using Microsoft.EntityFrameworkCore;


//var builder = WebApplication.CreateBuilder(args);

//var app = builder.Build();

//app.UseCors();
/*
app.MapGet("/comments", async (ICommentServices service, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("GET /comments");
        var comments = service.GetAllComments();
        return Results.Ok(comments);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error fetching comments");
        return Results.Problem("Error fetching comments");
    }
});

app.MapGet("/comments/{id:int}", async (int id, ICommentServices service, ILogger<Program> logger) =>
{
    try
    {
        var comment = await Task.Run(() => service.GetCommentById(id));
        logger.LogInformation("GET /comments/{Id}", id);
        return comment is not null ? Results.Ok(comment) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error fetching comment ID: {Id}", id);
        return Results.Problem("Error fetching comment");
    }
});

app.MapPost("/comments", async (Comment comment, ICommentServices service, ILogger<Program> logger) =>
{
    try
    {
        var added = await Task.Run(() => service.AddComment(comment));
        logger.LogInformation("POST /comments - ID: {Id}", added.Id);
        return Results.Created($"/comments/{added.Id}", added);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error creating comment");
        return Results.Problem("Error creating comment");
    }
});

app.MapPatch("/comments/{id:int}", async (int id, Comment comment, ICommentServices service, ILogger<Program> logger) =>
{
    try
    {
        var updated = await Task.Run(() => service.UpdateComment(id, comment));
        logger.LogInformation("PATCH /comments/{Id}", id);
        return updated is not null ? Results.Ok(updated) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error updating comment ID: {Id}", id);
        return Results.Problem("Error updating comment");
    }
});

app.MapDelete("/comments/{id:int}", async (int id, ICommentServices service, ILogger<Program> logger) =>
{
    try
    {
        var deleted = await Task.Run(() => service.DeleteComment(id));
        logger.LogInformation("DELETE /comments/{Id}", id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error deleting comment ID: {Id}", id);
        return Results.Problem("Error deleting comment");
    }
});

app.MapGet("/logs", async (CommentDBContext db, string? level, string? search) =>
{
    var query = db.Logs.AsQueryable();
    
    if (!string.IsNullOrEmpty(level))
    {
        query = query.Where(l => l.Level == level);
    }
    
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(l => 
            l.Message.Contains(search) || 
            l.Endpoint.Contains(search) ||
            (l.Exception != null && l.Exception.Contains(search)));
    }
    
    var logs = await query.OrderByDescending(l => l.Timestamp).Take(100).ToListAsync();
    return Results.Ok(logs);
});
*/

//app.MapGet("/", () => "API Glebasika run uraaa. Use endpoints: /comments, /logs");

//app.Run();

Console.WriteLine("hello ruban");