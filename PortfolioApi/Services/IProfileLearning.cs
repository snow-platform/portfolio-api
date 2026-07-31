using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;

namespace PortfolioApi.Services;

public interface IProfileLearning
{
    Task<ProducesEntity<CollectionType<Learn>>> GetProfileLearnings(Guid profileExternalId,
        Pagination pagination);

    Task<ProducesEntity<SingleType<Learn>>> GetProfileLearning(Guid profileExternalId,
        string slug);
}