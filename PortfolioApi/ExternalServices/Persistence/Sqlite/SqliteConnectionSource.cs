using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using PortfolioApi.Options;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class SqliteConnectionSource : IDbConnection<SqliteConnection>
{
    private readonly string _connectionSource;

    public SqliteConnectionSource(IOptions<ConnectionSource> options)
    {
        _connectionSource = options.Value.Main ?? throw new NullReferenceException("The connection to the database is null");
    }

    public SqliteConnection OpenConnection()
    {
        return new SqliteConnection(_connectionSource);
    }

    public ValueTask<SqliteConnection> OpenConnectionAsync()
    {
        throw new NotImplementedException();
    }
}