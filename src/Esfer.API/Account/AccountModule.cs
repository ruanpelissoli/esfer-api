﻿using Carter;
using Esfer.API.Account.Application.Commands.ConfirmAccountEmail;
using Esfer.API.Account.Application.Commands.CreateAccount;
using Esfer.API.Account.Application.Commands.Login;
using Esfer.API.Account.Application.Commands.Logout;
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

        app.MapPost("/logout", async (ISender sender) =>
        {
            await sender.Send(new LogoutCommand());

            return Results.Ok();
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

        app.MapGet("/me", async (HttpContext httpContext, ISender sender) =>
        {
            var accountId = httpContext.GetCurrentAccountId();

            if (accountId == Guid.Empty)
                return Results.Unauthorized();

            var result = await sender.Send(new GetAccountProfileQuery(accountId));

            return Results.Ok(result.Value);
        })
        .RequireAuthorization();
    }
}
