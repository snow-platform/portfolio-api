using PortfolioApi.Entities.CMS;
using PortfolioApi.ExternalServices.CMS;

namespace PortfolioApi.Test.ExternalServices.CMS;

public class CmsEitherTest
{
    [Fact]
    public void Ok_carries_the_status_code_and_the_value()
    {
        // arrange
        var collection = new CollectionType<object>
        {
            Data = []
        };

        // act
        var either = new CmsEitherOk<CollectionType<object>>(StatusCodes.Status200OK, collection);

        // assert
        Assert.IsAssignableFrom<CmsEither>(either);
        Assert.Equal(StatusCodes.Status200OK, either.StatusCode);
        Assert.Same(collection, either.Value);
    }

    [Fact]
    public void Empty_carries_only_the_status_code()
    {
        // arrange & act
        var either = new CmsEitherEmpty(StatusCodes.Status204NoContent);

        // assert
        Assert.IsAssignableFrom<CmsEither>(either);
        Assert.Equal(StatusCodes.Status204NoContent, either.StatusCode);
    }

    [Fact]
    public void Error_carries_the_status_code_the_error_and_the_description()
    {
        // arrange & act
        var either = new CmsEitherError(StatusCodes.Status404NotFound, "NotFoundError", "Not Found");

        // assert
        Assert.IsAssignableFrom<CmsEither>(either);
        Assert.Equal(StatusCodes.Status404NotFound, either.StatusCode);
        Assert.Equal("NotFoundError", either.Error);
        Assert.Equal("Not Found", either.Description);
    }
}
