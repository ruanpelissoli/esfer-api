using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;

namespace Esfer.API.Notification.Email;

public interface IEmailNotification
{
    Task SendAsync(string to, string subject);
    Task SendEmailConfirmationAsync(string to, string userName, string confirmationLink);
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

    public Task SendAsync(string to, string subject)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmailConfirmationAsync(string to, string userName, string confirmationLink)
    {
        _logger.LogInformation("Starting {0} to user {1}", nameof(SendEmailConfirmationAsync), to);

        StringBuilder emailBodyBuilder = new(File.ReadAllText(
            Path.Combine(Directory.GetCurrentDirectory(), "Notification\\Email\\Templates\\" + "welcome.html"), Encoding.UTF8));

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

        _logger.LogInformation("Finished {0}, StatusCode: {1}",
            nameof(SendEmailConfirmationAsync), response.StatusCode);
    }
}
