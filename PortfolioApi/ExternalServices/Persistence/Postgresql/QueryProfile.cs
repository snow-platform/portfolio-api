using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryProfile : IQueryProfile
{
    private readonly IDbConnection<NpgsqlConnection> _dbConnection;

    public QueryProfile(IDbConnection<NpgsqlConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Profile?> FindFromExternalId(Guid uuid)
    {;
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           select id, user_id, first_name, last_name, email, photo, title, stack, state, summary, created_at, updated_at
                           from profile
                           where external_id = @ExternalId
                           """;

        return await connection.QueryFirstOrDefaultAsync<Profile>(sql, new { ExternalId = uuid.ToString() });
    }

    public async Task Insert(Profile profile)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           insert into profile (user_id, first_name, last_name, email, photo, title, stack, state, summary, created_at, updated_at)
                           values (@UserId, @FirstName, @LastName, @Email, @Photo, @Title, @Stack, @State, @Summary, @CreatedAt, @UpdatedAt);
                           select last_insert_rowid();
                           """;

        profile.Id = await connection.ExecuteScalarAsync<int>(sql, profile);
    }

    public async Task Update(Profile profile)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           update profile
                           set first_name = @FirstName,
                               last_name = @LastName,
                               email = @Email,
                               photo = @Photo,
                               title = @Title,
                               stack = @Stack,
                               state = @State,
                               summary = @Summary,
                               updated_at = @UpdatedAt
                           where id = @Id
                           """;

        await connection.ExecuteAsync(sql, profile);
    }

    public async Task Delete(int id)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           delete from profile
                           where id = @Id
                           """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }
}