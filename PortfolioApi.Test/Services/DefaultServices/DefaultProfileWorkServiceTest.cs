using NSubstitute;
using PortfolioApi.DTO;
using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfileWorkServiceTest
{
    private readonly IQueryProfileWork _query = Substitute.For<IQueryProfileWork>();

    [Fact]
    public async Task GetProfileWork_maps_the_careers_to_work_dtos_when_found()
    {
        // arrange
        var service = new DefaultProfileWorkService(_query);
        var id = Guid.NewGuid();
        var careers = new List<ProfileCareer>
        {
            new()
            {
                Id = 1,
                Name = "Sample Company",
                Position = "Engineer",
                Projects =
                [
                    new CareerProject
                    {
                        Id = 10,
                        CareerId = 1,
                        Title = "Sample Project",
                        Description = "A sample description"
                    }
                ]
            }
        };

        _query.FindProfileCareersByExternalId(id)
            .Returns(careers);

        // act
        var result = await service.GetProfileWork(id);

        // assert
        Assert.IsType<ProducesEntityGood<List<ProfileWorkCareer>>>(result);
        Assert.NotNull(result.Entity);
        Assert.Single(result.Entity);
        Assert.Equal("Sample Company", result.Entity[0].Name);
        Assert.Equal("A sample description", result.Entity[0].Projects![0].Description);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task GetProfileWork_returns_404_when_not_found()
    {
        // arrange
        var service = new DefaultProfileWorkService(_query);

        _query.FindProfileCareersByExternalId(Arg.Any<Guid>())
            .Returns((List<ProfileCareer>?)null);

        // act
        var result = await service.GetProfileWork(Guid.NewGuid());

        // assert
        Assert.IsType<ProducesEntityFail<List<ProfileWorkCareer>>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Not Found", result.Error);
        Assert.Equal("The profile was not found", result.Description);
    }
}
