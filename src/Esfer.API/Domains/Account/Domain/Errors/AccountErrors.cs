using Esfer.API.Domains.Shared.Domain;

namespace Esfer.API.Domains.Account.Domain.Errors;

public static class AccountErrors
{
    public static Error InvalidUserNameOrPassword =>
        new(
        AccountErrorMessages.InvalidUserNameOrPassword.Code,
        AccountErrorMessages.InvalidUserNameOrPassword.Message);

    public static Error EmailNotConfirmed =>
        new(
        AccountErrorMessages.EmailNotConfirmed.Code,
        AccountErrorMessages.EmailNotConfirmed.Message);
}
