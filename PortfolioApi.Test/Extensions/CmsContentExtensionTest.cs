using System.Text;
using PortfolioApi.Extensions;
using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.CMS.Content;

namespace PortfolioApi.Test.Extensions;

public class CmsContentExtensionTest
{
    [Fact]
    public void ToHttpRequestMessage_maps_the_method_the_endpoint_and_the_content()
    {
        // arrange
        var content = new GetArticle("sample-slug", "sample-token");

        // act
        var request = content.ToHttpRequestMessage();

        // assert
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.NotNull(request.RequestUri);
        Assert.False(request.RequestUri.IsAbsoluteUri);
        Assert.StartsWith("/api/articles/sample-slug", request.RequestUri.OriginalString);
        Assert.Null(request.Content);
    }

    [Fact]
    public void ToHttpRequestMessage_copies_every_header()
    {
        // arrange
        var content = new StubCmsContent
        {
            Headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer sample-token" },
                { "X-Sample", "Sample" }
            }
        };

        // act
        var request = content.ToHttpRequestMessage();

        // assert
        Assert.Equal("Bearer sample-token", request.Headers.GetValues("Authorization").Single());
        Assert.Equal("Sample", request.Headers.GetValues("X-Sample").Single());
    }

    [Fact]
    public void ToHttpRequestMessage_keeps_the_content_when_there_is_one()
    {
        // arrange
        var body = new StringContent("{}", Encoding.UTF8, "application/json");
        var content = new StubCmsContent
        {
            HttpMethod = HttpMethod.Post,
            HttpContent = body
        };

        // act
        var request = content.ToHttpRequestMessage();

        // assert
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Same(body, request.Content);
    }

    private sealed class StubCmsContent : ICmsContent
    {
        public string Endpoint { get; init; } = "/api/samples";
        public HttpMethod HttpMethod { get; init; } = HttpMethod.Get;
        public HttpContent HttpContent { get; init; } = null!;
        public Dictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
    }
}
