namespace PortfolioApi.ExternalServices.CMS;

public interface ICmsInvoker
{
    Task<CmsEither> Invoke<T>(ICmsContent cms);
}