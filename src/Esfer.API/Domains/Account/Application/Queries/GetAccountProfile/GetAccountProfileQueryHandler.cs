﻿using Esfer.API.Domains.Account.Infrastructure.CurrentAuthorizedAccount;
using Esfer.API.Domains.Shared.Domain;
using Esfer.API.Domains.Shared.Mediator;

namespace Esfer.API.Domains.Account.Application.Queries.GetAccountProfile;

internal sealed class GetAccountProfileQueryHandler : IQueryHandler<GetAccountProfileQuery, GetAccountProfileResponse>
{
    readonly CurrentLoggedAccount _currentLoggedAccount;
    readonly ILogger<GetAccountProfileQueryHandler> _logger;

    public GetAccountProfileQueryHandler(
        CurrentLoggedAccount currentLoggedAccount,
        ILogger<GetAccountProfileQueryHandler> logger)
    {
        _currentLoggedAccount = currentLoggedAccount;
        _logger = logger;
    }

    public async Task<Result<GetAccountProfileResponse>> Handle(GetAccountProfileQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting account profile information for {AccountId}", _currentLoggedAccount.Id);

        return await Task.FromResult(
            Result.Success(new GetAccountProfileResponse(
                _currentLoggedAccount.UserName,
                _currentLoggedAccount.Email)));
    }
}
