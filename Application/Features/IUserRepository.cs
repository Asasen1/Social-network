using Domain.Agregates;
using Domain.Common;
using Domain.Entities;

namespace Application.Features;

public interface IUserRepository
{
    public Task<Result<User>> GetByEmail(string email, CancellationToken ct);
    public Task<Result<User>> GetById(Guid id, CancellationToken ct = default);
}