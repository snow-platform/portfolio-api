using NSubstitute;
using PortfolioApi.Entities.CMS;
using PortfolioApi.Entities.DB;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.CMS.Content;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfileLearningTest
{
    private readonly ICmsInvoker _cmsInvoker = Substitute.For<ICmsInvoker>();
    private readonly IQueryProfileCms _profileCms = Substitute.For<IQueryProfileCms>();

    [Fact]
    public async Task GetProfileLearnings_returns_the_collection_when_the_cms_answers()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();
        var collection = new CollectionType<object>
        {
            Data =
            [
                new
                {
                    Id = 1,
                    Title = "Sample",
                    Slug = "sample"
                },
                new
                {
                    Id = 2,
                    Title = "Sample",
                    Slug = "sample-two"
                }
            ],
            Meta = new Meta
            {
                Pagination = new MetaPagination
                {
                    Page = 1,
                    PageSize = 10,
                    PageCount = 5,
                    Total = 42
                }
            }
        };

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Id = 1,
                ProfileId = 1,
                Name = "Sample",
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherOk<CollectionType<object>>(StatusCodes.Status200OK, collection));

        // act
        var result = await service.GetProfileLearnings(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.IsType<ProducesEntityGood<CollectionType<object>>>(result);
        Assert.Same(collection, result.Entity);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(2, result.Entity!.Data!.Count);
        Assert.Equal(42, result.Entity.Meta!.Pagination!.Total);
    }

    [Fact]
    public async Task GetProfileLearnings_returns_404_when_the_profile_has_no_cms()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);

        _profileCms.FindFromProfileExternalId(Arg.Any<Guid>())
            .Returns((ProfileCms?)null);

        // act
        var result = await service.GetProfileLearnings(Guid.NewGuid(), new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.IsType<ProducesEntityFail<CollectionType<object>>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Not Found", result.Error);
        Assert.Equal("Profile not found", result.Description);
        Assert.False(result.Success);
        await _cmsInvoker.DidNotReceive()
            .Invoke<CollectionType<object>>(Arg.Any<ICmsContent>());
    }

    [Fact]
    public async Task GetProfileLearnings_returns_500_when_the_cms_answers_empty()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherEmpty(StatusCodes.Status200OK));

        // act
        var result = await service.GetProfileLearnings(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.IsType<ProducesEntityFail<CollectionType<object>>>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        Assert.Equal("Unknown Error", result.Error);
        Assert.Equal("Something went wrong when fetching learnings", result.Description);
    }

    [Fact]
    public async Task GetProfileLearnings_propagates_the_cms_error_as_is()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherError(StatusCodes.Status403Forbidden, "ForbiddenError", "No access to reviews"));

        // act
        var result = await service.GetProfileLearnings(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.IsType<ProducesEntityFail<CollectionType<object>>>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        Assert.Equal("ForbiddenError", result.Error);
        Assert.Equal("No access to reviews", result.Description);
    }

    [Fact]
    public async Task GetProfileLearnings_asks_the_cms_with_the_pagination_and_the_profile_token()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();
        ICmsContent? captured = null;

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Do<ICmsContent>(content => captured = content))
            .Returns(new CmsEitherOk<CollectionType<object>>(StatusCodes.Status200OK, new CollectionType<object>()));

        // act
        await service.GetProfileLearnings(id, new Pagination
        {
            Page = 2,
            Size = 5
        });

        // assert
        Assert.IsType<GetReviews>(captured);
        Assert.Equal(
            "/api/reviews?populate=*&pagination[page]=2&pagination[pageSize]=5&sort[0]=publishedAt:desc",
            captured!.Endpoint);
        Assert.Equal("Bearer sample-token", captured.Headers["Authorization"]);
        await _profileCms.Received(1)
            .FindFromProfileExternalId(id);
    }

    [Fact]
    public async Task GetProfileLearnings_sends_an_empty_token_when_the_profile_has_none()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();
        ICmsContent? captured = null;

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = null
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Do<ICmsContent>(content => captured = content))
            .Returns(new CmsEitherOk<CollectionType<object>>(StatusCodes.Status200OK, new CollectionType<object>()));

        // act
        await service.GetProfileLearnings(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.NotNull(captured);
        Assert.Equal("Bearer ", captured.Headers["Authorization"]);
    }

    [Fact]
    public async Task GetProfileLearning_returns_the_single_learning_when_the_cms_answers()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();
        var single = new SingleType<object>
        {
            Data = new
            {
                Id = 1,
                Title = "Sample",
                Slug = "sample"
            }
        };

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<SingleType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherOk<SingleType<object>>(StatusCodes.Status200OK, single));

        // act
        var result = await service.GetProfileLearning(id, "sample");

        // assert
        Assert.IsType<ProducesEntityGood<SingleType<object>>>(result);
        Assert.Same(single, result.Entity);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Same(single.Data, result.Entity!.Data);
    }

    [Fact]
    public async Task GetProfileLearning_returns_404_when_the_profile_has_no_cms()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);

        _profileCms.FindFromProfileExternalId(Arg.Any<Guid>())
            .Returns((ProfileCms?)null);

        // act
        var result = await service.GetProfileLearning(Guid.NewGuid(), "sample");

        // assert
        Assert.IsType<ProducesEntityFail<SingleType<object>>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Not Found", result.Error);
        Assert.Equal("Profile not found", result.Description);
        Assert.False(result.Success);
        await _cmsInvoker.DidNotReceive()
            .Invoke<SingleType<object>>(Arg.Any<ICmsContent>());
    }

    [Fact]
    public async Task GetProfileLearning_returns_500_when_the_cms_answers_empty()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<SingleType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherEmpty(StatusCodes.Status200OK));

        // act
        var result = await service.GetProfileLearning(id, "sample");

        // assert
        Assert.IsType<ProducesEntityFail<SingleType<object>>>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        Assert.Equal("Unknown Error", result.Error);
        Assert.Equal("Something went wrong when fetching the learning", result.Description);
    }

    [Fact]
    public async Task GetProfileLearning_propagates_the_cms_error_as_is()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<SingleType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherError(StatusCodes.Status404NotFound, "NotFoundError", "Not Found"));

        // act
        var result = await service.GetProfileLearning(id, "sample");

        // assert
        Assert.IsType<ProducesEntityFail<SingleType<object>>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("NotFoundError", result.Error);
        Assert.Equal("Not Found", result.Description);
    }

    [Fact]
    public async Task GetProfileLearning_asks_the_cms_for_the_slug_with_the_profile_token()
    {
        // arrange
        var service = new DefaultProfileLearning(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();
        ICmsContent? captured = null;

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<SingleType<object>>(Arg.Do<ICmsContent>(content => captured = content))
            .Returns(new CmsEitherOk<SingleType<object>>(StatusCodes.Status200OK, new SingleType<object>()));

        // act
        await service.GetProfileLearning(id, "sample-slug");

        // assert
        Assert.IsType<GetReview>(captured);
        Assert.Equal("/api/reviews/sample-slug?populate=*", captured!.Endpoint);
        Assert.Equal("Bearer sample-token", captured.Headers["Authorization"]);
        await _profileCms.Received(1)
            .FindFromProfileExternalId(id);
    }
}
