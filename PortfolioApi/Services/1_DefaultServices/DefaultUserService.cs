using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultUserService : IUserService
{
    private readonly IQueryUser _queryUser;

    public DefaultUserService(IQueryUser queryUser)
    {
        _queryUser = queryUser;
    }

    public async Task<ProducesEntity<User>> FindUserFromEmail(string email)
    {
        var user = await _queryUser.FindFromEmail(email);

        if (user is null)
        {
            return new ProducesEntityFail<User>(StatusCodes.Status404NotFound);
        }

        return new ProducesEntityGood<User>(user);
    }
}