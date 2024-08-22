namespace Application.DataAccess;

public interface ITransaction
{
    public Task<int> SaveChangesAsync(CancellationToken ct);
}