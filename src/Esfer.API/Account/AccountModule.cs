using Carter;
using Esfer.API.Account.Application.Commands.ChangePassword;
using Esfer.API.Account.Application.Commands.ConfirmAccountEmail;
using Esfer.API.Account.Application.Commands.CreateAccount;
using Esfer.API.Account.Application.Commands.Login;
using Esfer.API.Account.Application.Commands.ResetPassword;
using Esfer.API.Account.Application.Events.SendConfirmationAccountEmail;
using Esfer.API.Account.Application.Queries.GetAccountProfile;
using Esfer.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Esfer.API.Account;

public class AccountModule : CarterModule
{
    public AccountModule() : base("/account")
    {

    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateAccountCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return Results.Ok(result);
        });

        app.MapPost("/login", async (LoginCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return Results.Ok(result.Value);
        });

        app.MapGet("/confirm-email", async (
            [FromQuery] Guid accountId,
            [FromQuery] string token,
            ISender sender) =>
        {
            await sender.Send(new ConfirmAccountEmailCommand(accountId, token));

            return Results.Ok();
        });

        app.MapGet("/send-email-confirmation", async (
            [FromQuery] string email,
            IPublisher publisher) =>
        {
            await publisher.Publish(new SendConfirmationAccountEmailNotification(email));

            return Results.Ok();
        });

        app.MapPost("/reset-password", async (
            [FromBody] ResetPasswordCommand command,
            ISender sender) =>
        {
            await sender.Send(command);

            return Results.Ok();
        });

        app.MapPatch("/change-password", async (
            [FromQuery] Guid accountId,
            [FromQuery] string token,
            [FromBody] string newPassword,
            ISender sender) =>
        {
            var command = new ChangePasswordCommand(accountId, token, newPassword);

            await sender.Send(command);

            return Results.Ok();
        });

        app.MapGet("/me", async (
            ISender sender) =>
        {
            var result = await sender.Send(new GetAccountProfileQuery());

            return Results.Ok(result.Value);
        })
        .RequiredLoggedAccount();
    }
}
