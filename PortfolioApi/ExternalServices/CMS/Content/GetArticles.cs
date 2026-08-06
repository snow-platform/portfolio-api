using PortfolioApi.EntityValueObject;

namespace PortfolioApi.ExternalServices.CMS.Content;

public class GetArticles : ICmsContent
{
    private readonly Pagination _pagination;
    private readonly string _token;

    public GetArticles(Pagination pagination, string token)
    {
        _pagination = pagination;
        _token = token;
    }

    public string Endpoint
    {
        get =>
            $"/api/articles?populate=category&populate=tags&pagination[page]={_pagination.SanitizePage()}&pagination[pageSize]={_pagination.SanitizeSize()}&sort[0]=publishedAt:desc";
    }

    public HttpMethod HttpMethod
    {
        get => HttpMethod.Get;
    }

    public HttpContent HttpContent
    {
        get => null!;
    }

    public Dictionary<string, string> Headers
    {
        get => new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {_token}" }
        };
    }
}