using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence;

public interface IQueryProfile
{
    Task<Profile?> FindFromExternalId(Guid uuid);
    Task Insert(Profile profile);
    Task Update(Profile profile);
    Task Delete(int id);
}