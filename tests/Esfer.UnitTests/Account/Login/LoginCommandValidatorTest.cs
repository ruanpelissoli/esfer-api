using Esfer.API.Domains.Account.Application.Commands.Login;

namespace Esfer.UnitTests.Account.Login;
public class LoginCommandValidatorTest
{
    [Fact]
    public void Should_ValidationBeInvalid()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand("", "");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(f => f.UserName);
        result.ShouldHaveValidationErrorFor(f => f.Password);
    }

    [Fact]
    public void Should_ValidationBeSuccessful()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand("username", "password");

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
