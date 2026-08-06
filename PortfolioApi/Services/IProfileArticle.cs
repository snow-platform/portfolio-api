using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;

namespace PortfolioApi.Services;

public interface IProfileArticle
{
    Task<ProducesEntity<CollectionType<object>>> GetProfileArticles(Guid profileExternalId,
        Pagination pagination);

    Task<ProducesEntity<SingleType<object>>> GetProfileArticle(Guid profileExternalId,
        string slug);
}