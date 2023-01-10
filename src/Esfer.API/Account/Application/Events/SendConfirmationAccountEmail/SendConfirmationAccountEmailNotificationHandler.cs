using Esfer.API.Account.Domain.Entities;
using Esfer.API.Notification.Email;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace Esfer.API.Account.Application.Events.SendConfirmationAccountEmail;

internal sealed class SendConfirmationAccountEmailNotificationHandler
    : INotificationHandler<SendConfirmationAccountEmailNotification>
{
    readonly UserManager<UserAccount> _userManager;
    readonly IEmailNotification _emailNotification;
    readonly IConfiguration _configuration;
    readonly ILogger<SendConfirmationAccountEmailNotificationHandler> _logger;

    public SendConfirmationAccountEmailNotificationHandler(
        UserManager<UserAccount> userManager,
        IEmailNotification emailNotification,
        IConfiguration configuration,
        ILogger<SendConfirmationAccountEmailNotificationHandler> logger)
    {
        _userManager = userManager;
        _emailNotification = emailNotification;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Handle(SendConfirmationAccountEmailNotification notification, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByEmailAsync(notification.Email);

        if (account == null)
        {
            _logger.LogError("Account not found for email {Email}", notification.Email);
            return;
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);

        var encodedToken = HttpUtility.UrlEncode(token);

        var confirmationLink = $"{_configuration.GetSection("Host").Value}/account/confirm-email?accountId={account.Id}&token={encodedToken}";

        await _emailNotification.SendEmailConfirmationAsync(
            account.Email!,
            account.UserName!,
            confirmationLink);
    }
}
