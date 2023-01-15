using Esfer.API.Domains.Shared.Domain;
using MediatR;

namespace Esfer.API.Domains.Shared.Mediator;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
