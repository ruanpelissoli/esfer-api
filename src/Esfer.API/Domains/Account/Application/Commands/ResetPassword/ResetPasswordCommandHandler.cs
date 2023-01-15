using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Notification.Email;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Esfer.API.Domains.Account.Application.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    readonly UserManager<UserAccount> _userManager;
    readonly IEmailNotification _emailNotification;
    readonly IConfiguration _configuration;
    readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        UserManager<UserAccount> userManager,
        IEmailNotification emailNotification,
        IConfiguration configuration,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _emailNotification = emailNotification;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to reset password for {Email}", command.Email);

        var account = await _userManager.FindByEmailAsync(command.Email);

        if (account == null)
            return Result.Failure(new Error("" +
                "Account.NotFound",
                "Account not found"));

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(account);

        var encodedToken = HttpUtility.UrlEncode(resetToken);

        var resetPasswordLink = $"{_configuration.GetSection("Host").Value}/account/change-password?accountId={account.Id}&token={encodedToken}";

        await _emailNotification.SendResetEmailAsync(
            account.Email!,
            resetPasswordLink);

        return Result.Success();
    }
}
