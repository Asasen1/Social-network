using Domain.Common;

namespace Application.Abstractions;

public interface IQueryHandler<TRequest, TResponse>
{
    public Task<Result<TResponse>> Handle(TRequest request, CancellationToken ct);
}