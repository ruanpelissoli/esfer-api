using Esfer.API.Domains.Notification.Email;
using Esfer.API.Domains.Shared.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

namespace Esfer.API.Domains.Notification;

public class NotificationInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SendGridConfiguration>(
            configuration.GetSection(nameof(SendGridConfiguration)).Bind);

        var sendGridConfiguration = services.BuildServiceProvider().GetService<IOptions<SendGridConfiguration>>()?.Value
            ?? throw new InvalidOperationException("SendGrid configuration not found");

        services.AddSendGrid(options =>
            options.ApiKey = sendGridConfiguration.ApiKey);

        services.AddSingleton<IEmailNotification, EmailNotification>();
    }
}
