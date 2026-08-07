namespace PortfolioApi.ExternalServices.CMS.Content;

public class GetReview : ICmsContent
{
    // ReSharper disable once ReplaceWithFieldKeyword
    private readonly string _slug;
    private readonly string _token;

    public GetReview(string slug, string token)
    {
        _slug = slug;
        _token = token;
    }

    public string Endpoint
    {
        get => $"/api/reviews/{Uri.EscapeDataString(_slug)}?populate=cover&populate=author&populate=author.avatar&populate=category&populate=tags&populate=blocks&populate=blocks.file&populate=blocks.files";
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
