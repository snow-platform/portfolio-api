using Microsoft.Extensions.Caching.Hybrid;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.ExternalServices.Persistence.Cache;

public class CacheProfileCms : IQueryProfileCms
{
    private readonly IQueryProfileCms _profileCms;
    private readonly HybridCache _cache;

    public CacheProfileCms([FromKeyedServices("db")] IQueryProfileCms profileCms, HybridCache cache)
    {
        _profileCms = profileCms;
        _cache = cache;
    }

    public async Task<ProfileCms?> FindFromProfileExternalId(Guid profileExternalId)
    {
        return await _cache.GetOrCreateAsync<ProfileCms?>(
            $"profile_cms_{profileExternalId}",
            async _ => await _profileCms.FindFromProfileExternalId(profileExternalId));
    }
}