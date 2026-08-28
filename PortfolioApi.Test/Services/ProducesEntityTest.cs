using PortfolioApi.Entities.DB;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services;

public class ProducesEntityTest
{
    [Fact]
    public void Good_defaults_to_200_and_carries_the_entity()
    {
        // arrange
        var user = new User
        {
            Email = "sample@google.com"
        };

        // act
        var produces = new ProducesEntityGood<User>(user);

        // assert
        Assert.Same(user, produces.Entity);
        Assert.Equal(StatusCodes.Status200OK, produces.StatusCode);
        Assert.True(produces.Success);
        Assert.Null(produces.Error);
        Assert.Null(produces.Description);
    }

    [Fact]
    public void Good_honours_an_explicit_status_code()
    {
        // arrange
        var user = new User();

        // act
        var produces = new ProducesEntityGood<User>(user, StatusCodes.Status201Created);

        // assert
        Assert.Equal(StatusCodes.Status201Created, produces.StatusCode);
    }

    [Fact]
    public void Fail_leaves_the_entity_default_and_is_not_successful()
    {
        // arrange & act
        var produces = new ProducesEntityFail<User>(StatusCodes.Status404NotFound);

        // assert
        Assert.Null(produces.Entity);
        Assert.Equal(StatusCodes.Status404NotFound, produces.StatusCode);
        Assert.False(produces.Success);
    }

    [Fact]
    public void Fail_supplies_placeholder_messages_when_none_are_given()
    {
        // arrange & act
        var produces = new ProducesEntityFail<User>(StatusCodes.Status404NotFound);

        // assert
        Assert.Equal("Unknown Error", produces.Error);
        Assert.Equal("Unable to generate an exact error message", produces.Description);
    }

    [Fact]
    public void Fail_carries_the_supplied_messages()
    {
        // arrange & act
        var produces = new ProducesEntityFail<User>(StatusCodes.Status404NotFound, "Not Found", "Unable to find user");

        // assert
        Assert.Equal("Not Found", produces.Error);
        Assert.Equal("Unable to find user", produces.Description);
    }
    
    [Fact]
    public void Fail_over_a_value_type_is_not_successful()
    {
        // arrange & act
        var produces = new ProducesEntityFail<int>(StatusCodes.Status404NotFound);

        // assert
        Assert.False(produces.Success);
    }

    [Fact]
    public void Fail_over_a_reference_type()
    {
        // arrange & act
        var produces = new ProducesEntityFail<object>(StatusCodes.Status404NotFound);

        // assert
        Assert.False(produces.Success);
    }
}
