using PortfolioApi.DTO;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfileWorkService : IProfileWorkService
{
    private readonly IQueryProfileWork _queryProfileWork;

    public DefaultProfileWorkService(IQueryProfileWork queryProfileWork)
    {
        _queryProfileWork = queryProfileWork;
    }

    public async Task<ProducesEntity<List<ProfileWorkCareer>>> GetProfileWork(Guid profileId)
    {
        var careers = await _queryProfileWork.FindProfileCareersByExternalId(profileId);

        if (careers is null)
        {
            return new ProducesEntityFail<List<ProfileWorkCareer>>(StatusCodes.Status404NotFound,
                "Not Found",
                "The profile was not found");
        }

        return new ProducesEntityGood<List<ProfileWorkCareer>>(careers.ConvertAll(x => new ProfileWorkCareer(x)));
    }
}
