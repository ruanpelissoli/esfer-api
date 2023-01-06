using Esfer.API.Account.Domain.Entities;
using Esfer.API.Account.Infrastructure.Token;
using Esfer.API.Shared.Domain;
using Esfer.API.Shared.Mediator;
using Microsoft.AspNetCore.Identity;

namespace Esfer.API.Account.Application.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, LoginResponse>
{
    readonly UserManager<UserAccount> _userManager;
    readonly SignInManager<UserAccount> _signInManager;
    readonly ITokenService _tokenService;
    readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<UserAccount> userManager,
        SignInManager<UserAccount> signInManager,
        ITokenService tokenService,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting login for user {0}", request.Username);

        var account = await _userManager.FindByNameAsync(request.Username);

        if (account is null || !await _userManager.CheckPasswordAsync(account, request.Password))
        {
            return Result.Failure<LoginResponse>(new Error(
                "Account.InvalidUsernameOrPassword",
                "Invalid username or password"));
        }

        await _signInManager.SignInAsync(account, true);

        var token = _tokenService.GenerateToken(request.Username);

        return Result.Success(new LoginResponse(token));
    }
}
