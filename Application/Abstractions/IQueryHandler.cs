using Domain.Common;

namespace Application.Abstractions;

public interface IQueryHandler<TResponse, TRequest>
{
    public Task<Result<TResponse>> Handle(TRequest request);
}