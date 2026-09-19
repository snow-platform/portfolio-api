using Microsoft.Extensions.Options;
using Npgsql;
using PortfolioApi.Options;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class PostgresqlConnectionSource : IDbConnection<NpgsqlConnection>
{
    private readonly NpgsqlDataSource _source;

    public PostgresqlConnectionSource(IOptions<ConnectionSource> connectionSource)
    {
        _source = NpgsqlDataSource.Create(connectionSource.Value.Main ??
                                               throw new NullReferenceException("The connection string is null"));
    }

    public NpgsqlConnection OpenConnection()
    {
        return _source.OpenConnection();
    }

    public ValueTask<NpgsqlConnection> OpenConnectionAsync()
    {
        return _source.OpenConnectionAsync();
    }
}