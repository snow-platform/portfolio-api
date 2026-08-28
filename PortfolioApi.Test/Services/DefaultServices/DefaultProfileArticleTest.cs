using NSubstitute;
using PortfolioApi.Entities.CMS;
using PortfolioApi.Entities.DB;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.CMS.Content;
using PortfolioApi.ExternalServices.Persistence;
using PortfolioApi.Services;

namespace PortfolioApi.Test.Services.DefaultServices;

public class DefaultProfileArticleTest
{
    private readonly ICmsInvoker _cmsInvoker = Substitute.For<ICmsInvoker>();
    private readonly IQueryProfileCms _profileCms = Substitute.For<IQueryProfileCms>();

    [Fact]
    public async Task GetProfileArticles_returns_the_collection_when_the_cms_answers()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
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
        var result = await service.GetProfileArticles(id, new Pagination
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
    public async Task GetProfileArticles_returns_404_when_the_profile_has_no_cms()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);

        _profileCms.FindFromProfileExternalId(Arg.Any<Guid>())
            .Returns((ProfileCms?)null);

        // act
        var result = await service.GetProfileArticles(Guid.NewGuid(), new Pagination
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
    public async Task GetProfileArticles_returns_500_when_the_cms_answers_empty()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherEmpty(StatusCodes.Status200OK));

        // act
        var result = await service.GetProfileArticles(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.IsType<ProducesEntityFail<CollectionType<object>>>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        Assert.Equal("Unknown Error", result.Error);
        Assert.Equal("Something went wrong when fetching articles", result.Description);
    }

    [Fact]
    public async Task GetProfileArticles_propagates_the_cms_error_as_is()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherError(StatusCodes.Status403Forbidden, "ForbiddenError", "No access to articles"));

        // act
        var result = await service.GetProfileArticles(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.IsType<ProducesEntityFail<CollectionType<object>>>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        Assert.Equal("ForbiddenError", result.Error);
        Assert.Equal("No access to articles", result.Description);
    }

    [Fact]
    public async Task GetProfileArticles_asks_the_cms_with_the_pagination_and_the_profile_token()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
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
        await service.GetProfileArticles(id, new Pagination
        {
            Page = 2,
            Size = 5
        });

        // assert
        Assert.NotNull(captured);
        Assert.IsType<GetArticles>(captured);
        Assert.Equal(
            "/api/articles?populate=category&pagination[page]=2&pagination[pageSize]=5&sort[0]=publishedAt:desc",
            captured.Endpoint);
        Assert.Equal("Bearer sample-token", captured.Headers["Authorization"]);
        await _profileCms.Received(1)
            .FindFromProfileExternalId(id);
    }

    [Fact]
    public async Task GetProfileArticles_sends_an_empty_token_when_the_profile_has_none()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();
        ICmsContent? captured = null;

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = null
            });
        _cmsInvoker.Invoke<CollectionType<object>>(Arg.Do<ICmsContent>(content => captured = content))
            .Returns(new CmsEitherOk<CollectionType<object>>(StatusCodes.Status200OK,
                new CollectionType<object>()));

        // act
        await service.GetProfileArticles(id, new Pagination
        {
            Page = 1,
            Size = 10
        });

        // assert
        Assert.NotNull(captured);
        Assert.Equal("Bearer ", captured.Headers["Authorization"]);
    }

    [Fact]
    public async Task GetProfileArticle_returns_the_single_article_when_the_cms_answers()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
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
        var result = await service.GetProfileArticle(id, "sample");

        // assert
        Assert.IsType<ProducesEntityGood<SingleType<object>>>(result);
        Assert.Same(single, result.Entity);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Same(single.Data, result.Entity!.Data);
    }

    [Fact]
    public async Task GetProfileArticle_returns_404_when_the_profile_has_no_cms()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);

        _profileCms.FindFromProfileExternalId(Arg.Any<Guid>())
            .Returns((ProfileCms?)null);

        // act
        var result = await service.GetProfileArticle(Guid.NewGuid(), "sample");

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
    public async Task GetProfileArticle_returns_500_when_the_cms_answers_empty()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<SingleType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherEmpty(StatusCodes.Status200OK));

        // act
        var result = await service.GetProfileArticle(id, "sample");

        // assert
        Assert.IsType<ProducesEntityFail<SingleType<object>>>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        Assert.Equal("Unknown Error", result.Error);
        Assert.Equal("Something went wrong when fetching the article", result.Description);
    }

    [Fact]
    public async Task GetProfileArticle_propagates_the_cms_error_as_is()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
        var id = Guid.NewGuid();

        _profileCms.FindFromProfileExternalId(id)
            .Returns(new ProfileCms
            {
                Token = "sample-token"
            });
        _cmsInvoker.Invoke<SingleType<object>>(Arg.Any<ICmsContent>())
            .Returns(new CmsEitherError(StatusCodes.Status404NotFound, "NotFoundError", "Not Found"));

        // act
        var result = await service.GetProfileArticle(id, "sample");

        // assert
        Assert.IsType<ProducesEntityFail<SingleType<object>>>(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("NotFoundError", result.Error);
        Assert.Equal("Not Found", result.Description);
    }

    [Fact]
    public async Task GetProfileArticle_asks_the_cms_for_the_slug_with_the_profile_token()
    {
        // arrange
        var service = new DefaultProfileArticle(_cmsInvoker, _profileCms);
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
        await service.GetProfileArticle(id, "sample-slug");

        // assert
        Assert.NotNull(captured);
        Assert.IsType<GetArticle>(captured);
        Assert.Equal(
            "/api/articles/sample-slug?populate=cover&populate=author&populate=author.avatar&populate=category&populate=blocks&populate=blocks.file&populate=blocks.files",
            captured.Endpoint);
        Assert.Equal("Bearer sample-token", captured.Headers["Authorization"]);
        await _profileCms.Received(1)
            .FindFromProfileExternalId(id);
    }
}