using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfileCms : IQueryProfileCms
{
    private readonly IDbConnection<NpgsqlConnection> _connection;

    public QueryProfileCms(IDbConnection<NpgsqlConnection> connection)
    {
        _connection = connection;
    }

    public async Task<ProfileCms?> FindFromProfileExternalId(Guid profileExternalId)
    {
        await using var connection = await _connection.OpenConnectionAsync();

        const string sql = """
                           select pm.* from profile_cms pm
                           inner join profile p
                               on pm.profile_id = p.id
                           where p.external_id = @ProfileExternalId
                           """;

        return await connection.QueryFirstOrDefaultAsync<ProfileCms>(sql,
            new
            {
                ProfileExternalId = profileExternalId.ToString()
            });
    }
}