using Dapper;
using Microsoft.Data.Sqlite;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Sqlite;

public class QueryProfile : IQueryProfile
{
    private readonly IDbConnection<SqliteConnection> _dbConnection;

    public QueryProfile(IDbConnection<SqliteConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Profile?> FindFromExternalId(Guid uuid)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           select Id, UserId, FirstName, LastName, Email, Photo, Title, Stack, State, Summary, CreatedAt, UpdatedAt
                           from Profile
                           where ExternalId = @ExternalId
                           """;

        return await connection.QueryFirstOrDefaultAsync<Profile>(sql, new { ExternalId = uuid.ToString() });
    }

    public async Task Insert(Profile profile)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           insert into Profile (UserId, FirstName, LastName, Email, Photo, Title, Stack, State, Summary, CreatedAt, UpdatedAt)
                           values (@UserId, @FirstName, @LastName, @Email, @Photo, @Title, @Stack, @State, @Summary, @CreatedAt, @UpdatedAt);
                           select last_insert_rowid();
                           """;

        profile.Id = await connection.ExecuteScalarAsync<int>(sql, profile);
    }

    public async Task Update(Profile profile)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           update Profile
                           set FirstName = @FirstName,
                               LastName = @LastName,
                               Email = @Email,
                               Photo = @Photo,
                               Title = @Title,
                               Stack = @Stack,
                               State = @State,
                               Summary = @Summary,
                               UpdatedAt = @UpdatedAt
                           where Id = @Id
                           """;

        await connection.ExecuteAsync(sql, profile);
    }

    public async Task Delete(int id)
    {
        await using var connection = _dbConnection.OpenConnection();

        const string sql = """
                           delete from Profile
                           where Id = @Id
                           """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }
}