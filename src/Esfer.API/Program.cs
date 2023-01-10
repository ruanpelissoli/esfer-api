using Carter;
using Esfer.API.Installers;
using Esfer.API.Shared.Database;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();

builder.Services.InstallServices(
    builder.Configuration,
    typeof(Program).Assembly);

var connectionString = builder.Configuration.GetConnectionString("Esfer");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidProgramException("Db connection string not found");

builder.Services.AddSqlServer<EsferDbContext>(connectionString);

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