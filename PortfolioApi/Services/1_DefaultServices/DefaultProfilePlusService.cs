using PortfolioApi.DTO;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfilePlusService : IProfilePlusService
{
    private readonly IQueryProfilePlus _queryProfilePlus;

    public DefaultProfilePlusService(IQueryProfilePlus queryProfilePlus)
    {
        _queryProfilePlus = queryProfilePlus;
    }

    public async Task<ProducesEntity<ProfilePlus>> GetProfilePlus(Guid profileId)
    {
        var profile = await _queryProfilePlus.FindFromExternalId(profileId);

        if (profile is null)
        {
            return new ProducesEntityFail<ProfilePlus>(StatusCodes.Status404NotFound,
                "Not Found",
                "The profile was not found");
        }

        return new ProducesEntityGood<ProfilePlus>(new ProfilePlus(profile));
    }
}