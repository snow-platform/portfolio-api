using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;

namespace PortfolioApi.Services;

public interface IProfileArticle
{
    Task<ProducesEntity<CollectionType<Article>>> GetProfileArticles(Guid profileExternalId,
        Pagination pagination);
}