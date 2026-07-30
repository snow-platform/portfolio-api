using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfileSocial : IQueryProfileSocial
{
    private readonly IDbConnection<SqliteConnection> _dbConnection;

    public QueryProfileSocial(IDbConnection<SqliteConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<ProfileSocial>> ListFromProfileExternalId(Guid uuid)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           select ProfileSocial.Id, ProfileSocial.ProfileId, ProfileSocial.Name, ProfileSocial.Link
                           from ProfileSocial
                           join Profile on Profile.Id = ProfileSocial.ProfileId
                           where Profile.ExternalId = @ExternalId
                           """;

        var profileSocials = await connection.QueryAsync<ProfileSocial>(sql, new { ExternalId = uuid.ToString() });

        return profileSocials.AsList();
    }
}