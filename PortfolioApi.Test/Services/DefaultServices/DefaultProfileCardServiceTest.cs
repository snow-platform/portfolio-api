using NSubstitute;
using PortfolioApi.DTO;
using PortfolioApi.Entities.DB;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfileCardServiceTest
{
    private readonly IQueryProfileCard _query = Substitute.For<IQueryProfileCard>();

    [Fact]
    public async Task GetProfilesCard_passes_the_pagination_through_untouched()
    {
        // arrange
        var service = new DefaultProfileCardService(_query);
        var pagination = new Pagination
        {
            Page = 2,
            Size = 5
        };
        
        _query.ListCardFromPage(Arg.Any<Pagination>())
            .Returns(new Paginated<Profile>([], 2, 5, 0));

        // act
        await service.GetProfilesCard(pagination);

        // assert
        await _query.Received(1)
            .ListCardFromPage(pagination);
    }

    [Fact]
    public async Task GetProfilesCard_copies_paging_metadata_and_maps_each_profile()
    {
        // arrange
        var service = new DefaultProfileCardService(_query);
        var pagination = new Pagination
        {
            Page = 1,
            Size = 10
        };
        var profiles = new Paginated<Profile>(
            [
                new Profile
                {
                    Id = 1,
                    FirstName = "Sample",
                    Email = "sample@google.com"
                },
                new Profile
                {
                    Id = 2,
                    FirstName = "Sample",
                    Email = "sample@google.com"
                }
            ], 1, 10, 42);
        
        _query.ListCardFromPage(pagination)
            .Returns(profiles);

        // act
        var result = await service.GetProfilesCard(pagination);

        // assert
        Assert.IsType<ProducesEntityGood<Paginated<ProfileCard>>>(result);
        Assert.NotNull(result.Entity);
        Assert.Equal(1, result.Entity.Page);
        Assert.Equal(10, result.Entity.Size);
        Assert.Equal(42, result.Entity.Total);
        Assert.Equal(2, result.Entity.Items.Count);
        Assert.All(result.Entity.Items, item => Assert.IsType<ProfileCard>(item));
        Assert.Equal("Sample", result.Entity.Items[0].FirstName);
    }

    [Fact]
    public async Task GetProfilesCard_returns_an_empty_page_when_there_are_no_profiles()
    {
        // arrange
        var service = new DefaultProfileCardService(_query);
        var pagination = new Pagination
        {
            Page = 1,
            Size = 10
        };
        
        _query.ListCardFromPage(pagination)
            .Returns(new Paginated<Profile>([], 1, 10, 0));

        // act
        var result = await service.GetProfilesCard(pagination);

        // assert
        Assert.IsType<ProducesEntityGood<Paginated<ProfileCard>>>(result);
        Assert.NotNull(result.Entity);
        Assert.Empty(result.Entity.Items);
        Assert.True(result.Success);
    }
}
