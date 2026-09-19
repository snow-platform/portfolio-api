using System.Text.Json;
using Dapper;
using PortfolioApi;
using Scalar.AspNetCore;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        x.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
    });

builder.Services.AddCors(x =>
{
    x.AddDefaultPolicy(c =>
    {
        c.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>()!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddSimpleLogging();
builder.Services.AddSimpleVersioning();
builder.Services.AddSqlDb();
builder.Services.AddCaching();

builder.AddFluentMigration();
builder.AddOptions();

// services
builder.Services.AddDefaultServices();

// external services
// persistence
builder.Services.AddExternalServicesPersistence();

// cms
builder.AddCMS();

// health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseExceptionHandler("/error");

// note:
// - we will show the api documentation even in production
app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers();
app.MapHealthChecks("/health_checks");

app.Run();