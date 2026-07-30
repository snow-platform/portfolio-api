using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;
using PortfolioApi.EntityValueObject;
using PortfolioApi.Utilities;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfileCard : IQueryProfileCard
{
    private readonly IDbConnection<SqliteConnection> _dbConnection;

    public QueryProfileCard(IDbConnection<SqliteConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Paginated<Profile>> ListCardFromPage(Pagination pagination)
    {
        await using var connection = _dbConnection.OpenConnection();

        var parametersForItems = new DynamicParameters();
        var paginationBuilderForItems = new QueryPaginationBuilder(pagination, parametersForItems)
            .ApplySearch()
            .ApplySize()
            .ApplyOffset()
            .InsertSortDi()
            .InsertSortBy("Id")
            .InsertFilter("FirstName", "LastName", "Email");

        var sqlItems = paginationBuilderForItems.Build("""
                                                       select p.*, psk.Id, psk.ProfileId, psk.Category, psk.Name, psk.Proficiency
                                                       from (select Id, ExternalId, FirstName, LastName, Email, Title, Stack, State, About, CreatedAt, UpdatedAt
                                                             from Profile 
                                                             where (@Search is null or FirstName like ''%' || @Search || '%'' 
                                                              or LastName like ''%' || @Search || '%'' 
                                                              or Email like ''%' || @Search || '%'' 
                                                              or Title like ''%' || @Search || '%'') and ($Filter)
                                                             order by $Sort $Direction
                                                             limit @Size offset @Offset) p
                                                       left join ProfileSkill psk on p.Id = psk.ProfileId
                                                       """);

        var parametersForTotal = new DynamicParameters();
        var paginationBuilderForTotal = new QueryPaginationBuilder(pagination, parametersForTotal)
            .ApplySearch()
            .InsertFilter("FirstName", "LastName", "Email");

        var sqlTotal = paginationBuilderForTotal.Build("""
                                                       select count(*)
                                                       from Profile
                                                       where (@Search is null or FirstName like ''%' || @Search || '%'' 
                                                                  or LastName like ''%' || @Search || '%'' 
                                                                  or Email like ''%' || @Search || '%'' 
                                                                  or Title like ''%' || @Search || '%'') and
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