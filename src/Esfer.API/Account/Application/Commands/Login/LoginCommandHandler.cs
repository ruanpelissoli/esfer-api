using Esfer.API.Account.Domain.Entities;
using Esfer.API.Account.Infrastructure.Token;
using Esfer.API.Shared.Domain;
using Esfer.API.Shared.Mediator;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Account.Application.Commands.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, LoginResponse>
{
    readonly UserManager<UserAccount> _userManager;
    readonly ITokenService _tokenService;
    readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<UserAccount> userManager,
        ITokenService tokenService,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting login for user {0}", request.UserName);

        var account = await _userManager.FindByNameAsync(request.UserName);

        if (account is null
            || !await _userManager.CheckPasswordAsync(account, request.Password))
            return Result.Failure<LoginResponse>(new Error(
                "Account.InvalidUsernameOrPassword",
                "Invalid username or password"));

        if (!await _userManager.IsEmailConfirmedAsync(account))
            return Result.Failure<LoginResponse>(new Error(
                "Account.ConfirmEmail",
                "Please confirm your email"));

        var token = _tokenService.GenerateToken(request.UserName);

        return Result.Success(new LoginResponse(token));
    }
}
