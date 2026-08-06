using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;

namespace PortfolioApi.Services;

public interface IProfileLearning
{
    Task<ProducesEntity<CollectionType<object>>> GetProfileLearnings(Guid profileExternalId,
        Pagination pagination);

    Task<ProducesEntity<SingleType<object>>> GetProfileLearning(Guid profileExternalId,
        string slug);
}