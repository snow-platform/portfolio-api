using PortfolioApi.Entities.DB;
using PortfolioApi.EntityValueObject;

namespace PortfolioApi.ExternalServices.Persistence;

public interface IQueryProfileCard
{
    Task<Paginated<Profile>> ListCardFromPage(Pagination pagination);
}