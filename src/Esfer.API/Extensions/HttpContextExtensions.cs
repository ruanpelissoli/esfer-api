using System.Security.Claims;

namespace Esfer.API.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetCurrentAccountId(this HttpContext context)
    {
        if (context.User == null)
        {
            return Guid.Empty;
        }

        var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        return Guid.Parse(id);
    }
}
