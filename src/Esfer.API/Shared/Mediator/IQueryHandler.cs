using Esfer.API.Shared.Domain;
using MediatR;

namespace Esfer.API.Shared.Mediator;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
