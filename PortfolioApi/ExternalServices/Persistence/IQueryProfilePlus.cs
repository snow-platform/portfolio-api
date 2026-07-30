using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence;

public interface IQueryProfilePlus
{
    Task<Profile?> FindFromExternalId(Guid profileId);
}