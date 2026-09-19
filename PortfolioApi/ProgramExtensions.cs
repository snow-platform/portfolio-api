using Asp.Versioning;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.ExternalServices.Persistence.Postgresql;
using PortfolioApi.Options;

namespace PortfolioApi;

public static class ProgramExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSimpleLogging()
        {
            services.AddLogging(x =>
            {
                x.AddSimpleConsole(c =>
                {
                    c.IncludeScopes = true;
                    c.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                    c.ColorBehavior = LoggerColorBehavior.Enabled;
                });
            });

            services.AddHttpLogging(x =>
            {
                x.LoggingFields = HttpLoggingFields.Duration
                                  | HttpLoggingFields.Request
                                  | HttpLoggingFields.RequestQuery
                                  | HttpLoggingFields.Response;
                x.RequestBodyLogLimit = 8192;
                x.ResponseBodyLogLimit = 8192;
            });

            return services;
        }

        public IServiceCollection AddSimpleVersioning()
        {
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1);
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(x =>
            {
                x.GroupNameFormat = "'v'V";
                x.DefaultApiVersion = new ApiVersion(1);
                x.ApiVersionParameterSource = new UrlSegmentApiVersionReader();
                x.AssumeDefaultVersionWhenUnspecified = true;
            });

            return services;
        }

        public IServiceCollection AddAuth()
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "portfolio",
                        ValidAudience = "portfolio-app",
                        IssuerSigningKey = new SymmetricSecurityKey("portfolio"u8.ToArray())
                    };
                });

            return services;
        }

        public IServiceCollection AddSqlDb()
        {
            // sqlite
            // services.AddSingleton<IDbConnection<SqliteConnection>, SqliteConnectionSource>();

            // postgresql
            services.AddSingleton<IDbConnection<NpgsqlConnection>, PostgresqlConnectionSource>();

            return services;
        }

        public IServiceCollection AddCaching()
        {
            services.AddHybridCache(x =>
            {
                x.MaximumPayloadBytes = 1024 * 1024;
                x.MaximumKeyLength = 1024;
                x.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromHours(2),
                    LocalCacheExpiration = TimeSpan.FromHours(2)
                };
            });

            return services;
        }
    }

    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddFluentMigration()
        {
            var connection = builder.Configuration.GetConnectionString("Main");

            // when multiple schemas are use for migration
            // build a service collection with different IConventionSet for schemas
            builder.Services.AddScoped<IConventionSet>(_ => new DefaultConventionSet("cv", null));

            builder.Services
                .AddFluentMigratorCore()
                .ConfigureRunner(x =>
                {
                    x.AddPostgres()
                        .WithGlobalConnectionString(connection)
                        .ScanIn(typeof(Program).Assembly).For.Migrations();
                });

            return builder;
        }

        public IHostApplicationBuilder AddOptions()
        {
            var connectionString = builder.Configuration.GetConnectionString("Main");

            if (connectionString is "" or null)
            {
                throw new NullReferenceException("The connection string is null");
            }

            builder.Services.Configure<ConnectionSource>(x => x.Main = connectionString);

            return builder;
        }
    }
}