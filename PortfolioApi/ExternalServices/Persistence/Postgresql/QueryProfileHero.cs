using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfileHero : IQueryProfileHero
{
    private readonly IDbConnection<NpgsqlConnection> _dbConnection;

    public QueryProfileHero(IDbConnection<NpgsqlConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ProfileHero?> FindFromProfileExternalId(Guid uuid)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           select profile_hero.id, profile_hero.profile_id, profile_hero.head, profile_hero.text, profile_hero.title, profile_hero.state, profile_hero.status
                           from profile_hero
                           join profile on profile.id = profile_hero.profile_id
                           where profile.external_id = @ExternalId
                           """;

        return await connection.QueryFirstOrDefaultAsync<ProfileHero>(sql, new { ExternalId = uuid.ToString() });
    }
}