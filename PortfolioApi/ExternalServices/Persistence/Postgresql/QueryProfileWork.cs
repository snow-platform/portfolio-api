using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfileWork : IQueryProfileWork
{
    private readonly IDbConnection<NpgsqlConnection> _connection;

    public QueryProfileWork(IDbConnection<NpgsqlConnection> connection)
    {
        _connection = connection;
    }

    public async Task<List<ProfileCareer>?> FindProfileCareersByExternalId(Guid profileId)
    {
        await using var connection = await _connection.OpenConnectionAsync();

        const string queryP = "select email from profile where external_id = @ExternalId limit 1;";
        const string queryPj = """
                               select pc.*, cp.id, cp.career_id, cp.title, cp.description, cp.significance
                               from profile_career pc
                                        inner join profile p on pc.profile_id = p.id
                                        left join career_project cp on pc.id = cp.career_id
                               where p.external_id = @ExternalId;
                               """;
        const string queryPi = """
                               select pi.*
                               from project_image pi
                                        inner join career_project cp on pi.project_id = cp.id
                                        inner join profile_career pca on cp.career_id = pca.id
                                        inner join profile p on pca.profile_id = p.id
                               where p.external_id = @ExternalId;
                               """;
        const string queryPt = """
                               select pt.*
                               from project_technology pt
                                        inner join career_project cp on pt.project_id = cp.id
                                        inner join profile_career pca on cp.career_id = pca.id
                                        inner join profile p on pca.profile_id = p.id
                               where p.external_id = @ExternalId;
                               """;

        await using var items = await connection.QueryMultipleAsync(
            $"{queryP}{queryPj}{queryPi}{queryPt}",
            new { ExternalId = profileId.ToString() });

        var profileEmail = items.Read<string?>()
            .FirstOrDefault();

        if (profileEmail is null)
        {
            return null;
        }

        var profileCa = items.Read<ProfileCareer, CareerProject, ProfileCareer>((career, proj) =>
            {
                career.Projects = [proj];

                return career;
            }, splitOn: "Id")
            .ToList();
        var images = items.Read<ProjectImage>()
            .ToList();
        var techs = items.Read<ProjectTechnology>()
            .ToList();

        var imijsByProj = images.GroupBy(x => x.ProjectId)
            .ToDictionary(x => x.Key, x => x.ToList());
        var tecksByProj = techs.GroupBy(x => x.ProjectId)
            .ToDictionary(x => x.Key, x => x.ToList());

        var careers = profileCa.GroupBy(x => x.Id)
            .Select(x =>
            {
                var item = x.First();
                item.Projects = x.SelectMany(c => c.Projects ?? [])
                    .Select(c =>
                    {
                        c.Imijs = imijsByProj.GetValueOrDefault(c.Id, []);
                        c.Tecks = tecksByProj.GetValueOrDefault(c.Id, []);

                        return c;
                    })
                    .ToList();

                return item;
            })
            .ToList();

        return careers;
    }
}