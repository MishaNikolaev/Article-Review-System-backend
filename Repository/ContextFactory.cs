using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Article_Review_System_backend.Data;
using Microsoft.Extensions.Configuration;

public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            o => o.MigrationsAssembly("Article-Review-System-backend")
        );
        
        return new AppDbContext(builder.Options);
    }
}