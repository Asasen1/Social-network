using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class MigrationExtension
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}