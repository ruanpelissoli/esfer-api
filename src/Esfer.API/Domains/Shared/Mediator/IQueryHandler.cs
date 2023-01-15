using Esfer.API.Domains.Shared.Domain;
using MediatR;

namespace Esfer.API.Domains.Shared.Mediator;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
