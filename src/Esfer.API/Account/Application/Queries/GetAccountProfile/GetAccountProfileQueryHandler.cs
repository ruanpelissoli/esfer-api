using Esfer.API.Account.Domain.Entities;
using Esfer.API.Shared.Domain;
using Esfer.API.Shared.Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Esfer.API.Account.Application.Queries.GetAccountProfile;

internal sealed class GetAccountProfileQueryHandler : IQueryHandler<GetAccountProfileQuery, GetAccountProfileResponse>
{
    readonly UserManager<UserAccount> _userManager;
    readonly IMemoryCache _memoryCache;
    readonly ILogger<GetAccountProfileQueryHandler> _logger;

    public GetAccountProfileQueryHandler(UserManager<UserAccount> userManager, IMemoryCache memoryCache, ILogger<GetAccountProfileQueryHandler> logger)
    {
        _userManager = userManager;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<Result<GetAccountProfileResponse>> Handle(GetAccountProfileQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting account profile information for {AccountId} ", request.AccountId);

        var account = _memoryCache.Get<UserAccount>(request.AccountId);

        if (account == null)
        {
            account = await _userManager.FindByIdAsync(request.AccountId.ToString());

            if (account == null)
                return Result.Failure<GetAccountProfileResponse>(new Error("Account.NotFound", "Account not found"));

            _memoryCache.Set(request.AccountId, account);
        }

        return Result.Success(new GetAccountProfileResponse(account.UserName!, account.Email!));
    }
}
