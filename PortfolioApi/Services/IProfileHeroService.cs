using PortfolioApi.Entities.DB;

namespace PortfolioApi.Services;

public interface IProfileHeroService
{
    Task<ProducesEntity<ProfileHero>> GetProfileHero(Guid profileId);
}