using Application.DataAccess;
using Infrastructure.DbContexts;

namespace Infrastructure.Providers;

public class Transaction : ITransaction
{
    private readonly WriteDbContext _dbContext;

    public Transaction(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }
}