using PortfolioApi.DTO;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfileNaviService : IProfileNaviService
{
    private readonly IQueryProfileNavi _queryProfileNavi;

    public DefaultProfileNaviService(IQueryProfileNavi queryProfileNavi)
    {
        _queryProfileNavi = queryProfileNavi;
    }

    public async Task<ProducesEntity<ProfileNavi>> GetProfileNavi(Guid profileId)
    {
        var profileNavi = await _queryProfileNavi.FindNaviFromExternalId(profileId);

        if (profileNavi is null)
        {
            return new ProducesEntityFail<ProfileNavi>(StatusCodes.Status404NotFound, "Not Found",
                "No profile navi found");
        }

        return new ProducesEntityGood<ProfileNavi>(new ProfileNavi(profileNavi));
    }
}