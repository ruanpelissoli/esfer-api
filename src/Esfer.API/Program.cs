using Carter;
using Esfer.API.Domains.Shared.Database;
using Esfer.API.Extensions;
using Esfer.API.Installers;
using Esfer.API.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddMemoryCache();

builder.Services.InstallServices(
    builder.Configuration,
    typeof(Program).Assembly);

builder.Services.AddRateLimiting();

var connectionString = builder.Configuration.GetConnectionString("Esfer");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidProgramException("Db connection string not found");

builder.Services.AddSqlServer<EsferDbContext>(connectionString);

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorMiddleware<,>));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWebApp");

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Map("/", () => Results.Redirect("/swagger"));

app.MapCarter();

app.Run();