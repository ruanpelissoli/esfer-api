using Esfer.API.Account.Domain.Entities;
using Esfer.API.Shared.Domain;
using Esfer.API.Shared.Mediator;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Account.Application.Commands.Logout;

internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    readonly SignInManager<UserAccount> _signInManager;

    public LogoutCommandHandler(SignInManager<UserAccount> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        return Result.Success();
    }
}
