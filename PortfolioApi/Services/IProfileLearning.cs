using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;

namespace PortfolioApi.Services;

public interface IProfileLearning
{
    Task<ProducesEntity<CollectionType<Article>>> GetProfileLearnings(Guid profileExternalId,
        Pagination pagination);
}