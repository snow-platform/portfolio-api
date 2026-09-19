using Dapper;
using Npgsql;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Postgresql;

public class QueryUser : IQueryUser
{
    private readonly IDbConnection<NpgsqlConnection> _dbConnection;

    public QueryUser(IDbConnection<NpgsqlConnection> dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<User?> FindFromEmail(string email)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = "select * from \"user\" where email = @Email";

        var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });

        return user;
    }

    public async Task Insert(User user)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           insert into "user" (email, created_at, updated_at)
                           values (@Email, @CreatedAt, @UpdatedAt)
                           select last_insert_rowid();
                           """;

        user.Id = await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task Update(User user)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           update "user"
                           set updated_at = @UpdatedAt
                           where email = @Email
                           """;

        await connection.ExecuteAsync(sql, user);
    }

    public async Task Delete(int id)
    {
        await using var connection = await _dbConnection.OpenConnectionAsync();

        const string sql = """
                           delete from "user"
                           where id = @Id
                           """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }
}