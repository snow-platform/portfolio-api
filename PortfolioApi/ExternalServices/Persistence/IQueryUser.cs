using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence;

public interface IQueryUser
{
    Task<User?> FindFromEmail(string email);
    Task Insert(User user);
    Task Update(User user);
    Task Delete(int id);
}