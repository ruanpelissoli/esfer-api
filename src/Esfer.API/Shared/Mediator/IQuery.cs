using Esfer.API.Shared.Domain;
using MediatR;

namespace Esfer.API.Shared.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

