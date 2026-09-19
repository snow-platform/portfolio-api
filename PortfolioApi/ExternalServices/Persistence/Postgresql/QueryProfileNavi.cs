using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfileNavi : IQueryProfileNavi
{
    private readonly IDbConnection<NpgsqlConnection> _dbConnection;

    public QueryProfileNavi(IDbConnection<NpgsqlConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Profile?> FindNaviFromExternalId(Guid uuid)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           select p.id, p.external_id, p.first_name, p.last_name, cv.id, cv.profile_id, cv.cv, ps.id, ps.profile_id, ps.name, ps.link
                           from profile p
                           left join (select * from profile_cv where id in (select max(id) from profile_cv group by profile_id)) cv
                               on p.id = cv.profile_id
                           left join profile_social ps
                               on p.id = ps.profile_id
                           where external_id = @ExternalId
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