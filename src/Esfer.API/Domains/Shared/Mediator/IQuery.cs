using Esfer.API.Domains.Shared.Domain;
using MediatR;

namespace Esfer.API.Domains.Shared.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

