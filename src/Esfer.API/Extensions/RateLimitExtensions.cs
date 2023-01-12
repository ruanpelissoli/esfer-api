using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Esfer.API.Extensions;

public static class RateLimitExtensions
{
    static readonly string Policy = "PerUserRateLimit";

    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        return services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(Policy, context =>
            {
                var userName = context.User.FindFirstValue(ClaimTypes.Name);

                return RateLimitPartition.GetTokenBucketLimiter(userName, _ =>
                {
                    return new()
                    {
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        AutoReplenishment = true,
                        TokenLimit = 100,
                        TokensPerPeriod = 100,
                        QueueLimit = 100,
                    };
                });
            });
        });
    }

    public static IEndpointConventionBuilder RequirePerUserRateLimit(this IEndpointConventionBuilder builder) =>
        builder.RequireRateLimiting(Policy);
}
