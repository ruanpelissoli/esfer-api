using Esfer.API.Domains.Account.Domain.Errors;
using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Account.Infrastructure.Token;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Commands.Login;

public sealed class LoginCommandHandler
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

    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting login for user {UserName}", command.UserName);

        var account = await _accountRepository.FindByNameAsync(command.UserName);

        if (account is null
            || !await _accountRepository.CheckPasswordAsync(account, command.Password))
            return Result.Failure<LoginResponse>(AccountErrors.InvalidUserNameOrPassword);

        if (!await _accountRepository.IsEmailConfirmedAsync(account))
            return Result.Failure<LoginResponse>(AccountErrors.EmailNotConfirmed);

        var token = _tokenService.GenerateToken(command.UserName);

        return Result.Success(new LoginResponse(token));
    }
}
