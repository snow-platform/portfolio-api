namespace PortfolioApi.ExternalServices.CMS;

public interface ICmsContent
{
    string Endpoint { get; }
    HttpMethod HttpMethod { get; }
    HttpContent HttpContent { get; }
    Dictionary<string, string> Headers { get; }
}