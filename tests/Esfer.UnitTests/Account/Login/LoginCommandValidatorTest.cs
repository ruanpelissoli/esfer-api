using Esfer.API.Domains.Account.Application.Commands.Login;

namespace Esfer.UnitTests.Account.Login;
public class LoginCommandValidatorTest
{
    [Fact]
    public void Should_ValidationBeInvalid()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand("", "");

        var result = validator.Validate(command);

        result.Errors.Any(a => a.PropertyName == nameof(LoginCommand.UserName));
        result.Errors.Any(a => a.PropertyName == nameof(LoginCommand.Password));
    }

    [Fact]
    public void Should_ValidationBeSuccessful()
    {
        var validator = new LoginCommandValidator();
        var command = new LoginCommand("username", "password");

        var result = validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
