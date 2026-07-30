using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfileWork : IQueryProfileWork
{
    private readonly IDbConnection<SqliteConnection> _connection;

    public QueryProfileWork(IDbConnection<SqliteConnection> connection)
    {
        _connection = connection;
    }

    public async Task<List<ProfileCareer>?> FindProfileCareersByExternalId(Guid profileId)
    {
        await using var connection = _connection.OpenConnection();

        const string queryP = "select Email from Profile where ExternalId = @ExternalId limit 1;";
        const string queryPj = """
                               select pc.*, cp.Id, cp.CareerId, cp.Title, cp.Description, cp.Significance
                               from ProfileCareer pc
                                        inner join Profile p on pc.ProfileId = p.Id
                                        left join CareerProject cp on pc.Id = cp.CareerId
                               where p.ExternalId = @ExternalId;
                               """;
        const string queryPi = """
                               select pi.*
                               from ProjectImage pi
                                        inner join CareerProject cp on pi.ProjectId = cp.Id
                                        inner join ProfileCareer pca on cp.CareerId = pca.Id
                                        inner join Profile p on pca.ProfileId = p.Id
                               where p.ExternalId = @ExternalId;
                               """;
        const string queryPt = """
                               select pt.*
                               from ProjectTechnology pt
                                        inner join CareerProject cp on pt.ProjectId = cp.Id
                                        inner join ProfileCareer pca on cp.CareerId = pca.Id
                                        inner join Profile p on pca.ProfileId = p.Id
                               where p.ExternalId = @ExternalId;
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