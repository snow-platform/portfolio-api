using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence;

public interface IQueryProfileCms
{
    Task<ProfileCms?> FindFromProfileExternalId(Guid profileExternalId);
}