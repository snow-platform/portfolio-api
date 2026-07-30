using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfileNavi : IQueryProfileNavi
{
    private readonly IDbConnection<SqliteConnection> _dbConnection;

    public QueryProfileNavi(IDbConnection<SqliteConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Profile?> FindNaviFromExternalId(Guid uuid)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           select p.Id, p.ExternalId, p.FirstName, p.LastName, cv.Id, cv.ProfileId, cv.CV, ps.Id, ps.ProfileId, ps.Name, ps.Link
                           from Profile p
                           left join (select * from ProfileCV where id in (select max(id) from ProfileCV group by ProfileId)) cv
                               on p.Id = cv.ProfileId
                           left join ProfileSocial ps 
                               on p.Id = ps.ProfileId
                           where ExternalId = @ExternalId
                           """;

        var item = await connection.QueryAsync<Profile, ProfileCV, ProfileSocial, Profile>(sql,
            (profile, cv, social) =>
            {
                profile.CV = cv;
                profile.Socials = [social];

                return profile;
            }, new
            {
                ExternalId = uuid.ToString()
            },
            splitOn: "Id,Id");

        var corr = item.GroupBy(x => x.Id)
            .Select(x =>
            {
                var pf = x.First();
                pf.CV = pf.CV;
                pf.Socials = x.Where(c => c.Socials?.Count > 0)
                    .Select(c => c.Socials![0])
                    .ToList();

                return pf;
            })
            .First();

        return corr;
    }
}