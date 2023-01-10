using Esfer.API.Shared.Mediator;

namespace Esfer.API.Account.Application.Queries.GetAccountProfile;

public sealed record GetAccountProfileQuery(Guid AccountId) : IQuery<GetAccountProfileResponse>;
