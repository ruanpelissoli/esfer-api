using Esfer.API.Notification.Email;
using Esfer.API.Shared.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

namespace Esfer.API.Notification;

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
