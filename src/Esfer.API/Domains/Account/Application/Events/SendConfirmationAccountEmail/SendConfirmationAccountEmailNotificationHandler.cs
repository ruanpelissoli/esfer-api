using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Notification.Email;
using MediatR;
using System.Web;

namespace Esfer.API.Domains.Account.Application.Events.SendConfirmationAccountEmail;

internal sealed class SendConfirmationAccountEmailNotificationHandler
    : INotificationHandler<SendConfirmationAccountEmailNotification>
{
    readonly IAccountRepository _accountRepository;
    readonly IEmailNotification _emailNotification;
    readonly IConfiguration _configuration;
    readonly ILogger<SendConfirmationAccountEmailNotificationHandler> _logger;

    public SendConfirmationAccountEmailNotificationHandler(
        IAccountRepository accountRepository,
        IEmailNotification emailNotification,
        IConfiguration configuration,
        ILogger<SendConfirmationAccountEmailNotificationHandler> logger)
    {
        _accountRepository = accountRepository;
        _emailNotification = emailNotification;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Handle(SendConfirmationAccountEmailNotification notification, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindByEmailAsync(notification.Email);

        if (account == null)
        {
            _logger.LogError("Account not found for email {Email}", notification.Email);
            return;
        }

        var token = await _accountRepository.GenerateEmailConfirmationTokenAsync(account);

        var encodedToken = HttpUtility.UrlEncode(token);

        var confirmationLink = $"{_configuration.GetSection("Host").Value}/account/confirm-email?accountId={account.Id}&token={encodedToken}";

        await _emailNotification.SendEmailConfirmationAsync(
            account.Email!,
            account.UserName!,
            confirmationLink);
    }
}
