using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.CMS.Content;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfileArticle : IProfileArticle
{
    private readonly ICmsInvoker _cmsInvoker;
    private readonly IQueryProfileCms _profileCms;

    public DefaultProfileArticle(ICmsInvoker cmsInvoker, IQueryProfileCms profileCms)
    {
        _cmsInvoker = cmsInvoker;
        _profileCms = profileCms;
    }

    public async Task<ProducesEntity<CollectionType<Article>>> GetProfileArticles(Guid profileExternalId,
        Pagination pagination)
    {
        var profileCms = await _profileCms.FindFromProfileExternalId(profileExternalId);

        if (profileCms is null)
        {
            return new ProducesEntityFail<CollectionType<Article>>(StatusCodes.Status404NotFound, "Not Found",
                "Profile not found");
        }

        var content = new GetArticles(pagination, profileCms.Token ?? "");
        var either = await _cmsInvoker.Invoke<CollectionType<Article>>(content);

        if (either is CmsEitherEmpty)
        {
            return new ProducesEntityFail<CollectionType<Article>>(StatusCodes.Status500InternalServerError,
                "Unknown Error",
                "Something went wrong when fetching articles");
        }

        if (either is CmsEitherError error)
        {
            return new ProducesEntityFail<CollectionType<Article>>(error.StatusCode,
                error.Error,
                error.Description);
        }

        return new ProducesEntityGood<CollectionType<Article>>(((CmsEitherOk<CollectionType<Article>>)either).Value);
    }

    public async Task<ProducesEntity<SingleType<Article>>> GetProfileArticle(Guid profileExternalId,
        string slug)
    {
        var profileCms = await _profileCms.FindFromProfileExternalId(profileExternalId);

        if (profileCms is null)
        {
            return new ProducesEntityFail<SingleType<Article>>(StatusCodes.Status404NotFound, "Not Found",
                "Profile not found");
        }

        var content = new GetArticle(slug, profileCms.Token ?? "");
        var either = await _cmsInvoker.Invoke<SingleType<Article>>(content);

        if (either is CmsEitherEmpty)
        {
            return new ProducesEntityFail<SingleType<Article>>(StatusCodes.Status500InternalServerError,
                "Unknown Error",
                "Something went wrong when fetching the article");
        }

        if (either is CmsEitherError error)
        {
            return new ProducesEntityFail<SingleType<Article>>(error.StatusCode,
                error.Error,
                error.Description);
        }

        return new ProducesEntityGood<SingleType<Article>>(((CmsEitherOk<SingleType<Article>>)either).Value);
    }
}