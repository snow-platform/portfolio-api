using NSubstitute;
using PortfolioApi.Entities.DB;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultUserServiceTest
{
    private readonly IQueryUser _query = Substitute.For<IQueryUser>();

    [Fact]
    public async Task FindUserFromEmail_returns_the_user_when_found()
    {
        // arrange
        var service = new DefaultUserService(_query);
        var user = new User
        {
            Id = 1,
            Email = "sample@google.com"
        };
        
        _query.FindFromEmail("sample@google.com")
            .Returns(user);

        // act
        var result = await service.FindUserFromEmail("sample@google.com");

        // assert
        Assert.IsType<ProducesEntityGood<User>>(result);
        Assert.Same(user, result.Entity);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task FindUserFromEmail_returns_404_with_default_messages_when_not_found()
    {
        // arrange
        var service = new DefaultUserService(_query);
        
        _query.FindFromEmail(Arg.Any<string>())
            .Returns((User?)null);

        // act
        var result = await service.FindUserFromEmail("sample@google.com");

        // assert
        // the call site passes no error/description, so the placeholders come through
        Assert.IsType<ProducesEntityFail<User>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Unknown Error", result.Error);
        Assert.Equal("Unable to generate an exact error message", result.Description);
        Assert.False(result.Success);
    }
}
