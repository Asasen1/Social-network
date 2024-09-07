using Domain.AgregateRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DbContexts;

public class WriteDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public WriteDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("SocialNetwork"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

}