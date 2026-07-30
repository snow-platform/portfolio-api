using NSubstitute;
using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfileHeroServiceTest
{
    private readonly IQueryProfileHero _query = Substitute.For<IQueryProfileHero>();

    [Fact]
    public async Task GetProfileHero_returns_the_hero_when_found()
    {
        // arrange
        var service = new DefaultProfileHeroService(_query);
        var id = Guid.NewGuid();
        var hero = new ProfileHero
        {
            Id = 1,
            ProfileId = 1,
            Head = "Head"
        };
        
        _query.FindFromProfileExternalId(id)
            .Returns(hero);

        // act
        var result = await service.GetProfileHero(id);

        // assert
        Assert.IsType<ProducesEntityGood<ProfileHero>>(result);
        Assert.Same(hero, result.Entity);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetProfileHero_returns_404_when_not_found()
    {
        // arrange
        var service = new DefaultProfileHeroService(_query);
        
        _query.FindFromProfileExternalId(Arg.Any<Guid>())
            .Returns((ProfileHero?)null);

        // act
        var result = await service.GetProfileHero(Guid.NewGuid());

        // assert
        Assert.IsType<ProducesEntityFail<ProfileHero>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Not Found", result.Error);
        Assert.Equal("No profile hero found", result.Description);
    }
}
