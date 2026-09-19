namespace PortfolioApi.ExternalServices.Persistence;

public interface IDbConnection<T>
{
    T OpenConnection();
    ValueTask<T> OpenConnectionAsync();
}