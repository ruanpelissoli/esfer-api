using Esfer.API.Domains.Account.Application.Commands.CreateAccount;

namespace Esfer.UnitTests.Account.CreateAccount;
public class CreateAccountCommandValidatorTest
{
    [Fact]
    public void Should_ValidationBeInvalid()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("", "", "");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(f => f.UserName);
        result.ShouldHaveValidationErrorFor(f => f.Password);
        result.ShouldHaveValidationErrorFor(f => f.Email);
    }

    [Fact]
    public void Should_ValidationBeInvalid_IfEmailIsInvalid()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand(
            "username",
            "password",
            "aaaa");

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(f => f.Email);
    }

    [Fact]
    public void Should_ValidationBeSuccessful()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand(
            "username",
            "password",
            "email@test.com");

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
