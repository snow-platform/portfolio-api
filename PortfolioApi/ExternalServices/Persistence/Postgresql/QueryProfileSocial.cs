using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfileSocial : IQueryProfileSocial
{
    private readonly IDbConnection<NpgsqlConnection> _dbConnection;

    public QueryProfileSocial(IDbConnection<NpgsqlConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<ProfileSocial>> ListFromProfileExternalId(Guid uuid)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           select profile_social.id, profile_social.profile_id, profile_social.name, profile_social.link
                           from profile_social
                           join profile on profile.id = profile_social.profile_id
                           where profile.external_id = @ExternalId
                           """;

        var profileSocials = await connection.QueryAsync<ProfileSocial>(sql, new { ExternalId = uuid.ToString() });

        return profileSocials.AsList();
    }
}