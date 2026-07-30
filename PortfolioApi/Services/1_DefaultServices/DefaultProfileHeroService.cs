using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfileHeroService : IProfileHeroService
{
    private readonly IQueryProfileHero _queryProfileHero;

    public DefaultProfileHeroService(IQueryProfileHero queryProfileHero)
    {
        _queryProfileHero = queryProfileHero;
    }

    public async Task<ProducesEntity<ProfileHero>> GetProfileHero(Guid profileId)
    {
        var profileHero = await _queryProfileHero.FindFromProfileExternalId(profileId);

        if (profileHero is null)
        {
            return new ProducesEntityFail<ProfileHero>(StatusCodes.Status404NotFound, "Not Found",
                "No profile hero found");
        }

        return new ProducesEntityGood<ProfileHero>(profileHero);
    }
}