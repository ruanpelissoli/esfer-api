using FluentValidation;

namespace Esfer.API.Domains.Account.Application.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(f => f.UserName).NotNull().NotEmpty();
        RuleFor(f => f.Password).NotNull().NotEmpty();
        RuleFor(f => f.Email).EmailAddress().NotNull().NotEmpty();
    }
}
