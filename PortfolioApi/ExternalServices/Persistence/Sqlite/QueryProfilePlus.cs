using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfilePlus : IQueryProfilePlus
{
    private readonly IDbConnection<SqliteConnection> _connection;

    public QueryProfilePlus(IDbConnection<SqliteConnection> connection)
    {
        _connection = connection;
    }

    public async Task<Profile?> FindFromExternalId(Guid profileId)
    {
        await using var connection = _connection.OpenConnection();

        const string queryP = "select * from Profile where ExternalId = @ExternalId limit 1;";
        const string queryPv = """
                               select pc.*
                               from ProfileCV pc
                                        inner join Profile p on pc.ProfileId = p.Id
                               where p.ExternalId = @ExternalId
                               order by CreatedAt desc
                               limit 1;
                               """;
        const string queryPs = """
                               select ps.* 
                               from ProfileSkill ps
                                      inner join Profile p on ps.ProfileId = p.Id
                               where p.ExternalId = @ExternalId;
                               """;
        const string queryPl = """
                               select ps.*
                               from ProfileSocial ps
                                        inner join Profile p on ps.ProfileId = p.Id
                               where p.ExternalId = @ExternalId;
                               """;
        const string queryPc = """
                               select pc.*
                               from ProfileCertificate pc
                                        inner join Profile p on pc.ProfileId = p.Id
                               where p.ExternalId = @ExternalId;
                               """;
        const string queryPe = """
                               select pe.*
                               from ProfileEducation pe
                                        inner join Profile p on pe.ProfileId = p.Id
                               where p.ExternalId = @ExternalId;
                               """;
        const string queryPj = """
                               select pc.*, cp.Id, cp.CareerId, cp.Title, cp.Significance
                               from ProfileCareer pc
                                        inner join Profile p on pc.ProfileId = p.Id
                                        left join CareerProject cp on pc.Id = cp.CareerId
                               where p.ExternalId = @ExternalId;
                               """;

        await using var items = await connection.QueryMultipleAsync(
            $"{queryP}{queryPv}{queryPs}{queryPl}{queryPc}{queryPe}{queryPj}",
            new { ExternalId = profileId.ToString() });

        var profile = items.Read<Profile>()
            .FirstOrDefault();
        var profileCv = items.Read<ProfileCV>()
            .FirstOrDefault();
        var profileSk = items.Read<ProfileSkill>()
            .ToList();
        var profileSc = items.Read<ProfileSocial>()
            .ToList();
        var profileCt = items.Read<ProfileCertificate>()
            .ToList();
        var profileEd = items.Read<ProfileEducation>()
            .ToList();
        var profileCa = items.Read<ProfileCareer, CareerProject, ProfileCareer>((career, proj) =>
            {
                career.Projects = [proj];
                return career;
            }, splitOn: "Id")
            .ToList();

        if (profile is null)
        {
            return null;
        }

        var career = profileCa.GroupBy(x => x.Id)
            .Select(x =>
            {
                var item = x.First();
                item.Projects = x.Select(c => c.Projects!.Single())
                    .Select(c => c)
                    .ToList();

                return item;
            })
            .ToList();

        profile.CV = profileCv;
        profile.Career = career;
        profile.Skills = profileSk;
        profile.Socials = profileSc;
        profile.Certificates = profileCt;
        profile.Educations = profileEd;

        return profile;
    }
}