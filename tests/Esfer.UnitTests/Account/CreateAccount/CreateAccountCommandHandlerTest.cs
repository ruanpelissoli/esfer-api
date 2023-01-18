using Esfer.API.Domains.Account.Application.Commands.CreateAccount;
using Esfer.API.Domains.Account.Application.Events.SendConfirmationAccountEmail;
using Esfer.API.Domains.Account.Domain.Errors;
using Esfer.API.Domains.Account.Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Esfer.UnitTests.Account.CreateAccount;

public class CreateAccountCommandHandlerTest
{
    readonly Mock<IAccountRepository> _accountRepositoryMock;
    readonly Mock<IPublisher> _publisherMock;
    readonly Mock<ILogger<CreateAccountCommandHandler>> _loggerMock;

    readonly CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTest()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _publisherMock = new Mock<IPublisher>();
        _loggerMock = new Mock<ILogger<CreateAccountCommandHandler>>();

        _handler = new(
            _accountRepositoryMock.Object,
            _publisherMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Should_CreateAccount()
    {
        var command = GenerateCreateAccountCommandFake();

        _accountRepositoryMock.Setup(s => s.CreateAsync(
            command.UserName,
            command.Email,
            command.Password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(command, CancellationToken.None);

        _accountRepositoryMock.Verify(v => v.CreateAsync(
            command.UserName,
            command.Email,
            command.Password),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnFailureResult_If_AccountCreationFails()
    {
        var command = GenerateCreateAccountCommandFake();

        var identityErrors = new IdentityError[]
        {
            new IdentityError()
            {
                Description = "Password length invalid",
            },
            new IdentityError()
            {
                Description = "Password should contain upper case letters",
            }
        };

        var createCreationFailed = new AccountErrorMessages.CreateCreationFailed(identityErrors);

        _accountRepositoryMock.Setup(s => s.CreateAsync(
            command.UserName,
            command.Email,
            command.Password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(createCreationFailed.Code);
        result.Error.Message.Should().Be(createCreationFailed.Message);
    }

    [Fact]
    public async Task Should_SendAccountConfirmationEmail()
    {
        var command = GenerateCreateAccountCommandFake();
        SendConfirmationAccountEmailNotification sendAccountConfirmationEmail =
            new(command.Email);

        _accountRepositoryMock.Setup(s => s.CreateAsync(
            command.UserName,
            command.Email,
            command.Password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(command, CancellationToken.None);

        _publisherMock.Verify(v => v.Publish(
            sendAccountConfirmationEmail,
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_ReturnSuccessfulResult()
    {
        var command = GenerateCreateAccountCommandFake();
        SendConfirmationAccountEmailNotification sendAccountConfirmationEmail =
            new(command.Email);

        _accountRepositoryMock.Setup(s => s.CreateAsync(
            command.UserName,
            command.Email,
            command.Password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    private static CreateAccountCommand GenerateCreateAccountCommandFake() =>
        new Faker<CreateAccountCommand>()
            .CustomInstantiator(f => new CreateAccountCommand(
                f.Random.String2(30),
                f.Random.String2(16),
                f.Internet.Email()
            )).Generate();
}
