namespace Esfer.API.Domains.Account.Domain.Errors;

public static class AccountErrorMessages
{
    public partial class InvalidUserNameOrPassword
    {
        public const string Code = $"Account.{nameof(InvalidUserNameOrPassword)}";
        public const string Message = "Invalid username or password";
    }

    public partial class EmailNotConfirmed
    {
        public const string Code = $"Account.{nameof(EmailNotConfirmed)}";
        public const string Message = "Please confirm your email";
    }
}
