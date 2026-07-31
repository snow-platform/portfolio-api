using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.CMS.Content;
using PortfolioApi.ExternalServices.Persistence;

namespace PortfolioApi.Services;

public class DefaultProfileLearning : IProfileLearning
{
    private readonly ICmsInvoker _cmsInvoker;
    private readonly IQueryProfileCms _profileCms;

    public DefaultProfileLearning(ICmsInvoker cmsInvoker, IQueryProfileCms profileCms)
    {
        _cmsInvoker = cmsInvoker;
        _profileCms = profileCms;
    }

    public async Task<ProducesEntity<CollectionType<Learn>>> GetProfileLearnings(Guid profileExternalId,
        Pagination pagination)
    {
        var profileCms = await _profileCms.FindFromProfileExternalId(profileExternalId);

        if (profileCms is null)
        {
            return new ProducesEntityFail<CollectionType<Learn>>(StatusCodes.Status404NotFound, "Not Found",
                "Profile not found");
        }

        var content = new GetReviews(pagination, profileCms.Token ?? "");
        var either = await _cmsInvoker.Invoke<CollectionType<Learn>>(content);

        if (either is CmsEitherEmpty)
        {
            return new ProducesEntityFail<CollectionType<Learn>>(StatusCodes.Status500InternalServerError,
                "Unknown Error",
                "Something went wrong when fetching learnings");
        }

        if (either is CmsEitherError error)
        {
            return new ProducesEntityFail<CollectionType<Learn>>(error.StatusCode,
                error.Error,
                error.Description);
        }

        return new ProducesEntityGood<CollectionType<Learn>>(((CmsEitherOk<CollectionType<Learn>>)either).Value);
    }
}