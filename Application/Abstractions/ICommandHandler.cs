using Domain.Common;

namespace Application.Abstractions;

public interface ICommandHandler<TRequest>
{
    public Task<Result> Handle(TRequest command, CancellationToken ct);
}