using Esfer.API.Domains.Shared.Domain;
using Microsoft.AspNetCore.Identity;

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

    public static Error CreateCreationFailed(IEnumerable<IdentityError> errors)
    {
        var createCreationFailed = new AccountErrorMessages.CreateCreationFailed(errors);

        return new(
         createCreationFailed.Code,
         createCreationFailed.Message);
    }
}
