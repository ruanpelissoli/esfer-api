using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Account.Infrastructure.Token;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, LoginResponse>
{
    readonly IAccountRepository _accountRepository;
    readonly ITokenService _tokenService;
    readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IAccountRepository accountRepository,
        ITokenService tokenService,
        ILogger<LoginCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting login for user {UserName}", request.UserName);

        var account = await _accountRepository.FindByNameAsync(request.UserName);

        if (account is null
            || !await _accountRepository.CheckPasswordAsync(account, request.Password))
            return Result.Failure<LoginResponse>(new Error(
                "Account.InvalidUsernameOrPassword",
                "Invalid username or password"));

        if (!await _accountRepository.IsEmailConfirmedAsync(account))
            return Result.Failure<LoginResponse>(new Error(
                "Account.ConfirmEmail",
                "Please confirm your email"));

        var token = _tokenService.GenerateToken(request.UserName);

        return Result.Success(new LoginResponse(token));
    }
}
