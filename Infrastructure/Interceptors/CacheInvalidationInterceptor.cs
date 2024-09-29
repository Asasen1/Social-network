using Application.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public class CacheInvalidationInterceptor : SaveChangesInterceptor
{
    private readonly ICacheProvider _cacheProvider;

    public CacheInvalidationInterceptor(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        await InvalidateCache(eventData, ct);
        return result;
    }

    private async Task InvalidateCache(DbContextEventData eventData, CancellationToken ct = default)
    {
        if (eventData.Context is null)
            return;

        var entries = eventData.Context.ChangeTracker.Entries()
            .Where(e =>
                e.State
                    is EntityState.Added
                    or EntityState.Modified
                    or EntityState.Deleted);
        foreach (var entity in entries)
        {
            var entityName = entity.GetType().Name;
            await _cacheProvider.RemoveByPrefixAsync(entityName, ct);
        }
    }
}