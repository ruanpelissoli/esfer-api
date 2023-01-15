using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;

namespace Esfer.API.Domains.Notification.Email;

public interface IEmailNotification
{
    Task SendEmailConfirmationAsync(string to, string userName, string confirmationLink);
    Task SendResetEmailAsync(string to, string resetPasswordLink);
}

public class EmailNotification : IEmailNotification
{
    readonly ISendGridClient _sendGridClient;
    readonly SendGridConfiguration _sendGridConfiguration;
    readonly ILogger<EmailNotification> _logger;

    public EmailNotification(
        ISendGridClient sendGridClient,
        IOptions<SendGridConfiguration> sendGridConfigurationOptions,
        ILogger<EmailNotification> logger)
    {
        _sendGridClient = sendGridClient;
        _sendGridConfiguration = sendGridConfigurationOptions.Value;
        _logger = logger;
    }

    public async Task SendEmailConfirmationAsync(string to, string userName, string confirmationLink)
    {
        _logger.LogInformation("Starting {SendEmailConfirmationAsync} to user {to}", nameof(SendEmailConfirmationAsync), to);

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Notification\\Email\\Templates\\" + "welcome.html");
        var htmlBodyString = File.ReadAllText(templatePath, Encoding.UTF8);

        StringBuilder emailBodyBuilder = new(htmlBodyString);

        emailBodyBuilder.Replace("[CONFIRMATION_URL]", confirmationLink);
        emailBodyBuilder.Replace("[EMAIL_ADDRESS]", _sendGridConfiguration.FromEmail);
        emailBodyBuilder.Replace("[YOUR_NAME]", _sendGridConfiguration.FromName);

        var message = new SendGridMessage()
        {
            From = new EmailAddress(_sendGridConfiguration.FromEmail, _sendGridConfiguration.FromName),
            Subject = $"Welcome {userName}, Esfer awaits!",
            HtmlContent = emailBodyBuilder.ToString()
        };
        message.AddTo(new EmailAddress(to));

        var response = await _sendGridClient.SendEmailAsync(message);

        _logger.LogInformation("Finished {SendEmailConfirmationAsync}, StatusCode: {StatusCode}",
            nameof(SendEmailConfirmationAsync), response.StatusCode);
    }

    public async Task SendResetEmailAsync(string to, string resetPasswordLink)
    {
        _logger.LogInformation("Starting {SendResetEmailAsync} to user {to}", nameof(SendResetEmailAsync), to);

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Notification\\Email\\Templates\\" + "reset-email.html");
        var htmlBodyString = File.ReadAllText(templatePath, Encoding.UTF8);

        StringBuilder emailBodyBuilder = new(htmlBodyString);

        emailBodyBuilder.Replace("[RESET_URL]", resetPasswordLink);
        emailBodyBuilder.Replace("[EMAIL_ADDRESS]", _sendGridConfiguration.FromEmail);
        emailBodyBuilder.Replace("[YOUR_NAME]", _sendGridConfiguration.FromName);

        var message = new SendGridMessage()
        {
            From = new EmailAddress(_sendGridConfiguration.FromEmail, _sendGridConfiguration.FromName),
            Subject = $"Reset Password - Esfer",
            HtmlContent = emailBodyBuilder.ToString()
        };
        message.AddTo(new EmailAddress(to));

        var response = await _sendGridClient.SendEmailAsync(message);

        _logger.LogInformation("Finished {SendResetEmailAsync}, StatusCode: {StatusCode}",
            nameof(SendResetEmailAsync), response.StatusCode);
    }
}
