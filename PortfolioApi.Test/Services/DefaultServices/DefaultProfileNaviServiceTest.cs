using NSubstitute;
using PortfolioApi.DTO;
using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfileNaviServiceTest
{
    private readonly IQueryProfileNavi _query = Substitute.For<IQueryProfileNavi>();

    [Fact]
    public async Task GetProfileNavi_maps_the_profile_to_a_navi_dto_when_found()
    {
        // arrange
        var service = new DefaultProfileNaviService(_query);
        var id = Guid.NewGuid();
        var profile = new Profile
        {
            ExternalId = id.ToString(),
            Email = "sample@google.com",
            FirstName = "Sample",
            LastName = "Magician"
        };
        
        _query.FindNaviFromExternalId(id)
            .Returns(profile);

        // act
        var result = await service.GetProfileNavi(id);

        // assert
        Assert.IsType<ProducesEntityGood<ProfileNavi>>(result);
        Assert.NotNull(result.Entity);
        Assert.Equal(profile.Email, result.Entity.Email);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task GetProfileNavi_returns_404_when_not_found()
    {
        // arrange
        var service = new DefaultProfileNaviService(_query);
        
        _query.FindNaviFromExternalId(Arg.Any<Guid>())
            .Returns((Profile?)null);

        // act
        var result = await service.GetProfileNavi(Guid.NewGuid());

        // assert
        Assert.IsType<ProducesEntityFail<ProfileNavi>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Not Found", result.Error);
        Assert.Equal("No profile navi found", result.Description);
    }
}
