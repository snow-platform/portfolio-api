using NSubstitute;
using PortfolioApi.DTO;
using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfilePlusServiceTest
{
    private readonly IQueryProfilePlus _query = Substitute.For<IQueryProfilePlus>();

    [Fact]
    public async Task GetProfilePlus_maps_the_profile_to_a_plus_dto_when_found()
    {
        // arrange
        var service = new DefaultProfilePlusService(_query);
        var id = Guid.NewGuid();
        var profile = new Profile
        {
            ExternalId = id.ToString(),
            FirstName = "Sample",
            Email = "sample@google.com"
        };
        
        _query.FindFromExternalId(id)
            .Returns(profile);

        // act
        var result = await service.GetProfilePlus(id);

        // assert
        Assert.IsType<ProducesEntityGood<ProfilePlus>>(result);
        Assert.NotNull(result.Entity);
        Assert.Equal(profile.FirstName, result.Entity.FirstName);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task GetProfilePlus_returns_404_when_not_found()
    {
        // arrange
        var service = new DefaultProfilePlusService(_query);
        
        _query.FindFromExternalId(Arg.Any<Guid>())
            .Returns((Profile?)null);

        // act
        var result = await service.GetProfilePlus(Guid.NewGuid());

        // assert
        Assert.IsType<ProducesEntityFail<ProfilePlus>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Not Found", result.Error);
        Assert.Equal("The profile was not found", result.Description);
    }
}
