using Domain.Common;

namespace Application.Abstractions;

public interface ICommandHandler<T>
{
    public Task<Result> Handle(T command, CancellationToken ct);
}