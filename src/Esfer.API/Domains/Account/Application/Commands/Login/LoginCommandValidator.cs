using FluentValidation;

namespace Esfer.API.Domains.Account.Application.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(login => login.UserName).NotNull().NotEmpty();
        RuleFor(login => login.Password).NotNull().NotEmpty();
    }
}
