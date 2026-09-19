using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;
using PortfolioApi.EntityValueObject;
using PortfolioApi.Utilities;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfileCard : IQueryProfileCard
{
    private readonly IDbConnection<NpgsqlConnection> _dbConnection;

    public QueryProfileCard(IDbConnection<NpgsqlConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Paginated<Profile>> ListCardFromPage(Pagination pagination)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        var parametersForItems = new DynamicParameters();
        var paginationBuilderForItems = new QueryPaginationBuilder(pagination, parametersForItems)
            .ApplySearch()
            .ApplySize()
            .ApplyOffset()
            .InsertSortDi()
            .InsertSortBy("id")
            .InsertFilter("first_name", "last_name", "email");

        var sqlItems = paginationBuilderForItems.Build("""
                                                       select p.*, psk.id, psk.profile_id, psk.category, psk.name, psk.proficiency
                                                       from (select id, external_id, first_name, last_name, email, title, stack, state, about, created_at, updated_at
                                                             from profile
                                                             where (@Search is null or first_name like '%' || @Search || '%'
                                                               or last_name like '%' || @Search || '%'
                                                               or email like '%' || @Search || '%'
                                                               or title like '%' || @Search || '%') and ($Filter)
                                                             order by $Sort $Direction
                                                             limit @Size offset @Offset) p
                                                       left join profile_skill psk on p.id = psk.profile_id
                                                       """);

        var parametersForTotal = new DynamicParameters();
        var paginationBuilderForTotal = new QueryPaginationBuilder(pagination, parametersForTotal)
            .ApplySearch()
            .InsertFilter("first_name", "last_name", "email");

        var sqlTotal = paginationBuilderForTotal.Build("""
                                                       select count(*)
                                                       from profile
                                                       where (@Search is null or first_name like '%' || @Search || '%'
                                                                  or last_name like '%' || @Search || '%'
                                                                  or email like '%' || @Search || '%'
                                                                  or title like '%' || @Search || '%') and
                                                           ($Filter)
                                                       """);

        var items = await connection.QueryAsync<Profile, ProfileSkill, Profile>(sqlItems,
            (p, sk) =>
            {
                p.Skills = [sk];

                return p;
            }, parametersForItems, splitOn: "Id");
        var actual = items.GroupBy(p => p.Id)
            .Select(g =>
            {
                var first = g.First();
                first.Skills = g.Where(x => x.Skills?.Count > 0)
                    .Select(x => x.Skills![0])
                    .ToList();

                return first;
            })
            .ToList();
        var total = await connection.QuerySingleAsync<int>(sqlTotal, parametersForTotal);

        return new Paginated<Profile>(actual, pagination.SanitizePage(), pagination.SanitizeSize(), total);
    }
}
