using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Esfer.API.Domains.Account.Infrastructure.Token;

public interface ITokenService
{
    string GenerateToken(string userName);
}

internal class TokenService : ITokenService
{
    readonly IAuthenticationConfigurationProvider _authenticationConfigurationProvider;

    public TokenService(IAuthenticationConfigurationProvider authenticationConfigurationProvider)
    {
        _authenticationConfigurationProvider = authenticationConfigurationProvider;
    }

    public string GenerateToken(string userName)
    {
        var bearerSection = _authenticationConfigurationProvider.GetSchemeConfiguration(
       JwtBearerDefaults.AuthenticationScheme);

        var section = bearerSection.GetSection("SigningKeys:0");

        var issuer = bearerSection["ValidIssuer"] ?? throw new InvalidOperationException("Issuer is not specified");
        var signingKeyBase64 = section["Value"] ?? throw new InvalidOperationException("Signing Key is not specified");

        var signinKeyBytes = Convert.FromBase64String(signingKeyBase64);

        var jwtSigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(signinKeyBytes),
            SecurityAlgorithms.HmacSha256Signature);

        var audiences = bearerSection.GetSection("ValidAudiences").GetChildren()
            .Where(s => !string.IsNullOrEmpty(s.Value))
            .Select(s => new Claim(JwtRegisteredClaimNames.Aud, s.Value!))
            .ToArray();

        var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userName));

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        identity.AddClaims(audiences);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateJwtSecurityToken(
            issuer,
            audience: null,
            identity,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(1),
            issuedAt: DateTime.UtcNow,
            jwtSigningCredentials);

        return handler.WriteToken(token);
    }
}
