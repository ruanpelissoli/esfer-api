
using Esfer.API.Domains.Account.Application.Commands.Login;
using Esfer.API.Domains.Account.Domain.Entities;
using Esfer.API.Domains.Account.Domain.Errors;
using Esfer.API.Domains.Account.Domain.Repository;
using Esfer.API.Domains.Account.Infrastructure.Token;

namespace Esfer.UnitTests.Account.Login;

public class LoginCommandHandlerTest
{
    readonly Mock<IAccountRepository> _accountRepositoryMock;
    readonly Mock<ITokenService> _tokenServiceMock;
    readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;

    readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTest()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<LoginCommandHandler>>();

        _handler = new(
            _accountRepositoryMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Should_FindAccountByName()
    {
        var command = GenerateLoginCommandFake();

        _accountRepositoryMock.Setup(s => s.FindByNameAsync(command.UserName))
            .ReturnsAsync(It.IsAny<UserAccount>());

        await _handler.Handle(command, CancellationToken.None);

        _accountRepositoryMock.Verify(v => v.FindByNameAsync(command.UserName),
            Times.Once);
    }

    [Fact]
    public async Task Should_ValidatePassword_And_ReturnFailureResult()
    {
        var command = GenerateLoginCommandFake();
        UserAccount account = new(command.UserName, command.Password);

        _accountRepositoryMock.Setup(s => s.FindByNameAsync(command.UserName))
            .ReturnsAsync(account);

        _accountRepositoryMock.Setup(s => s.CheckPasswordAsync(account, command.Password))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(AccountErrorMessages.InvalidUserNameOrPassword.Code);
        result.Error.Message.Should().Be(AccountErrorMessages.InvalidUserNameOrPassword.Message);
    }

    [Fact]
    public async Task Should_ValidateEmailConfirmed_And_ReturnFailureResult()
    {
        var command = GenerateLoginCommandFake();
        UserAccount account = new(command.UserName, command.Password);

        _accountRepositoryMock.Setup(s => s.FindByNameAsync(command.UserName))
            .ReturnsAsync(account);

        _accountRepositoryMock.Setup(s => s.CheckPasswordAsync(account, command.Password))
            .ReturnsAsync(true);

        _accountRepositoryMock.Setup(s => s.IsEmailConfirmedAsync(account))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(AccountErrorMessages.EmailNotConfirmed.Code);
        result.Error.Message.Should().Be(AccountErrorMessages.EmailNotConfirmed.Message);
    }

    [Fact]
    public async Task Should_GenerateJwtToken()
    {
        var command = GenerateLoginCommandFake();
        UserAccount account = new(command.UserName, command.Password);

        _accountRepositoryMock.Setup(s => s.FindByNameAsync(command.UserName))
            .ReturnsAsync(account);

        _accountRepositoryMock.Setup(s => s.CheckPasswordAsync(account, command.Password))
            .ReturnsAsync(true);

        _accountRepositoryMock.Setup(s => s.IsEmailConfirmedAsync(account))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        _tokenServiceMock.Verify(v => v.GenerateToken(command.UserName),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnResultSuccess()
    {
        var command = GenerateLoginCommandFake();
        var token = "xyz123!@#";
        UserAccount account = new(command.UserName, command.Password);

        _accountRepositoryMock.Setup(s => s.FindByNameAsync(command.UserName))
            .ReturnsAsync(account);

        _accountRepositoryMock.Setup(s => s.CheckPasswordAsync(account, command.Password))
            .ReturnsAsync(true);

        _accountRepositoryMock.Setup(s => s.IsEmailConfirmedAsync(account))
            .ReturnsAsync(true);

        _tokenServiceMock.Setup(s => s.GenerateToken(command.UserName))
            .Returns(token);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<LoginResponse>();
        result.Value!.Token.Should().Be(token);
    }

    private static LoginCommand GenerateLoginCommandFake() =>
        new Faker<LoginCommand>()
            .CustomInstantiator(f => new LoginCommand(
                f.Random.String2(30),
                f.Random.String2(16)
            )).Generate();
}
