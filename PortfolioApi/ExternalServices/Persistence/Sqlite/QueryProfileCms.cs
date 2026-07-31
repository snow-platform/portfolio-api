using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfileCms : IQueryProfileCms
{
    private readonly IDbConnection<SqliteConnection> _connection;

    public QueryProfileCms(IDbConnection<SqliteConnection> connection)
    {
        _connection = connection;
    }

    public async Task<ProfileCms?> FindFromProfileExternalId(Guid profileExternalId)
    {
        await using var connection = _connection.OpenConnection();

        const string sql = """
                           select pm.* from ProfileCms pm
                           inner join Profile p 
                               on pm.ProfileId = p.Id
                           where p.ExternalId = @ProfileExternalId
                           """;

        return await connection.QueryFirstOrDefaultAsync<ProfileCms>(sql,
            new
            {
                ProfileExternalId = profileExternalId.ToString()
            });
    }
}