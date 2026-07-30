using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfileHero : IQueryProfileHero
{
    private readonly IDbConnection<SqliteConnection> _dbConnection;

    public QueryProfileHero(IDbConnection<SqliteConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ProfileHero?> FindFromProfileExternalId(Guid uuid)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           select ProfileHero.Id, ProfileHero.ProfileId, ProfileHero.Head, ProfileHero.Text, ProfileHero.Title, ProfileHero.State, ProfileHero.Status
                           from ProfileHero
                           join Profile on Profile.Id = ProfileHero.ProfileId
                           where Profile.ExternalId = @ExternalId
                           """;

        return await connection.QueryFirstOrDefaultAsync<ProfileHero>(sql, new { ExternalId = uuid.ToString() });
    }
}