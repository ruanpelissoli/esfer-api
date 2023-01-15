using Esfer.API.Domains.Shared.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Esfer.API.Extensions;

public class SwaggerInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Esfer API",
                Description = "Esfer API - A game platform for developers",
                TermsOfService = new Uri("https://www.esfer.gg"),
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Name = JwtBearerDefaults.AuthenticationScheme,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            options.AddSecurityRequirement(new()
            {
                [securityScheme] = new List<string>()
            });
        });
    }
}
