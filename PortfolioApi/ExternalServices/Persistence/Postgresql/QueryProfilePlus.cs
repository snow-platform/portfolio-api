using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfilePlus : IQueryProfilePlus
{
    private readonly IDbConnection<NpgsqlConnection> _connection;

    public QueryProfilePlus(IDbConnection<NpgsqlConnection> connection)
    {
        _connection = connection;
    }

    public async Task<Profile?> FindFromExternalId(Guid profileId)
    {
        await using var connection = await _connection.OpenConnectionAsync();

        const string queryP = "select * from profile where external_id = @ExternalId limit 1;";
        const string queryPv = """
                               select pc.*
                               from profile_cv pc
                                        inner join profile p on pc.profile_id = p.id
                               where p.external_id = @ExternalId
                               order by created_at desc
                               limit 1;
                               """;
        const string queryPs = """
                               select ps.*
                               from profile_skill ps
                                      inner join profile p on ps.profile_id = p.id
                               where p.external_id = @ExternalId;
                               """;
        const string queryPl = """
                               select ps.*
                               from profile_social ps
                                        inner join profile p on ps.profile_id = p.id
                               where p.external_id = @ExternalId;
                               """;
        const string queryPc = """
                               select pc.*
                               from profile_certificate pc
                                        inner join profile p on pc.profile_id = p.id
                               where p.external_id = @ExternalId;
                               """;
        const string queryPe = """
                               select pe.*
                               from profile_education pe
                                        inner join profile p on pe.profile_id = p.id
                               where p.external_id = @ExternalId;
                               """;
        const string queryPj = """
                               select pc.*, cp.id, cp.career_id, cp.title, cp.significance
                               from profile_career pc
                                        inner join profile p on pc.profile_id = p.id
                                        left join career_project cp on pc.id = cp.career_id
                               where p.external_id = @ExternalId;
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