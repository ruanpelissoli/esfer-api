using Carter;
using Esfer.API.Account.Application.CreateAccount;
using Esfer.API.Account.Application.Login;
using MediatR;

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
    }
}
