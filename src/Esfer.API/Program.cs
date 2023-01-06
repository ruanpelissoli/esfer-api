using Carter;
using Esfer.API;
using Esfer.API.Shared.Database;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.InstallServices(
    builder.Configuration,
    typeof(Program).Assembly);

var connectionString = builder.Configuration.GetConnectionString("Esfer");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidProgramException("Db connection string not found");

builder.Services.AddSqlServer<EsferDbContext>(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

builder.Services.AddCarter();

builder.Services.AddMediatR(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();