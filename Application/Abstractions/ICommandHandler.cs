using Domain.Common;

namespace Application.Abstractions;

public interface ICommandHandler<TRequest>
{
    public Task<Result> Handle(TRequest request, CancellationToken ct);
}