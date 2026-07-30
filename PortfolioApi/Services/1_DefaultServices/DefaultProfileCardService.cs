using PortfolioApi.DTO;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfileCardService : IProfileCardService
{
    private readonly IQueryProfileCard _queryProfile;

    public DefaultProfileCardService(IQueryProfileCard queryProfile)
    {
        _queryProfile = queryProfile;
    }

    public async Task<ProducesEntity<Paginated<ProfileCard>>> GetProfilesCard(Pagination pagination)
    {
        var profiles = await _queryProfile.ListCardFromPage(pagination);
        var profilesCard = profiles.Items
            .ConvertAll(profile => new ProfileCard(profile));

        return new ProducesEntityGood<Paginated<ProfileCard>>(new Paginated<ProfileCard>(profilesCard,
            profiles.Page,
            profiles.Size,
            profiles.Total));
    }
}