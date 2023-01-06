using Esfer.API.Shared.Domain;
using MediatR;

namespace Esfer.API.Shared.Mediator;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
