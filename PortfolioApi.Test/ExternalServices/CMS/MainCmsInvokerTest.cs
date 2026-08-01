using System.Net;
using System.Text;
using NSubstitute;
using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;
using PortfolioApi.ExternalServices.CMS;
using PortfolioApi.ExternalServices.CMS.Content;

namespace PortfolioApi.Test.ExternalServices.CMS;

public class MainCmsInvokerTest
{
    private MainCmsInvoker CreateInvoker(StubHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://cms.sample.test")
        };
        var httpClientFactory = Substitute.For<IHttpClientFactory>();

        httpClientFactory.CreateClient("cms_sender")
            .Returns(httpClient);

        return new MainCmsInvoker(httpClientFactory);
    }

    private StubHttpMessageHandler CreateHandler(HttpStatusCode statusCode, string content)
    {
        return new StubHttpMessageHandler(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        });
    }

    [Fact]
    public async Task Invoke_deserializes_a_collection_when_the_cms_answers_200()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.OK, """
                                                       {
                                                           "data": [
                                                               { "id": 1, "title": "Sample", "slug": "sample" },
                                                               { "id": 2, "title": "Sample", "slug": "sample-two" }
                                                           ],
                                                           "meta": { "pagination": { "page": 1, "pageSize": 10, "pageCount": 5, "total": 42 } }
                                                       }
                                                       """);
        var invoker = CreateInvoker(handler);

        // act
        var pagination = new Pagination
        {
            Page = 1,
            Size = 10
        };
        var either = await invoker.Invoke<CollectionType<Article>>(new GetArticles(pagination, "sample-token"));

        // assert
        var ok = Assert.IsType<CmsEitherOk<CollectionType<Article>>>(either);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.Equal(2, ok.Value.Data!.Count);
        Assert.Equal("sample", ok.Value.Data[0].Slug);
        Assert.Equal(42, ok.Value.Meta!.Pagination!.Total);
    }

    [Fact]
    public async Task Invoke_deserializes_a_single_type_when_the_cms_answers_200()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.OK, """
                                                       {
                                                           "data": { "id": 1, "title": "Sample", "slug": "sample" }
                                                       }
                                                       """);
        var invoker = CreateInvoker(handler);

        // act
        var either = await invoker.Invoke<SingleType<Article>>(new GetArticle("sample", "sample-token"));

        // assert
        var ok = Assert.IsType<CmsEitherOk<SingleType<Article>>>(either);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        Assert.NotNull(ok.Value.Data);
        Assert.Equal("Sample", ok.Value.Data.Title);
    }

    [Fact]
    public async Task Invoke_returns_empty_when_the_body_is_blank()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.OK, "");
        var invoker = CreateInvoker(handler);

        // act
        var either = await invoker.Invoke<CollectionType<Article>>(new GetArticles(new Pagination
        {
            Page = 1,
            Size = 10
        }, "sample-token"));

        // assert
        Assert.IsType<CmsEitherEmpty>(either);
        Assert.Equal(StatusCodes.Status200OK, either.StatusCode);
    }

    [Fact]
    public async Task Invoke_returns_empty_when_the_body_is_a_json_null()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.OK, "null");
        var invoker = CreateInvoker(handler);

        // act
        var either = await invoker.Invoke<CollectionType<Article>>(new GetArticles(new Pagination
        {
            Page = 1,
            Size = 10
        }, "sample-token"));

        // assert
        Assert.IsType<CmsEitherEmpty>(either);
        Assert.Equal(StatusCodes.Status200OK, either.StatusCode);
    }

    [Fact]
    public async Task Invoke_returns_a_500_error_when_the_body_is_not_valid_json()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.OK, "{");
        var invoker = CreateInvoker(handler);

        // act
        var either = await invoker.Invoke<CollectionType<Article>>(new GetArticles(new Pagination
        {
            Page = 1,
            Size = 10
        }, "sample-token"));

        // assert
        var error = Assert.IsType<CmsEitherError>(either);
        Assert.Equal(StatusCodes.Status500InternalServerError, error.StatusCode);
        Assert.NotEmpty(error.Error);
    }

    [Fact]
    public async Task Invoke_maps_the_cms_error_payload_when_the_call_fails()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.NotFound, """
                                                             {
                                                                 "error": { "status": "404", "name": "NotFoundError", "message": "Not Found" }
                                                             }
                                                             """);
        var invoker = CreateInvoker(handler);

        // act
        var either = await invoker.Invoke<SingleType<Article>>(new GetArticle("sample", "sample-token"));

        // assert
        var error = Assert.IsType<CmsEitherError>(either);
        Assert.Equal(StatusCodes.Status404NotFound, error.StatusCode);
        Assert.Equal("NotFoundError", error.Error);
        Assert.Equal("Not Found", error.Description);
    }

    [Fact]
    public async Task Invoke_falls_back_to_empty_messages_when_the_failure_carries_no_error()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.InternalServerError, "{}");
        var invoker = CreateInvoker(handler);

        // act
        var either = await invoker.Invoke<SingleType<Article>>(new GetArticle("sample", "sample-token"));

        // assert
        var error = Assert.IsType<CmsEitherError>(either);
        Assert.Equal(StatusCodes.Status500InternalServerError, error.StatusCode);
        Assert.Equal("", error.Error);
        Assert.Equal("", error.Description);
    }

    [Fact]
    public async Task Invoke_sends_the_content_as_a_get_carrying_the_bearer_token()
    {
        // arrange
        var handler = CreateHandler(HttpStatusCode.OK, """{ "data": [] }""");
        var invoker = CreateInvoker(handler);

        // act
        await invoker.Invoke<CollectionType<Article>>(new GetArticles(new Pagination
        {
            Page = 2,
            Size = 5
        }, "sample-token"));

        // assert
        Assert.NotNull(handler.Request);
        Assert.Equal(HttpMethod.Get, handler.Request.Method);
        Assert.Equal("/api/articles", handler.Request.RequestUri!.AbsolutePath);
        Assert.Contains("pagination[page]=2", Uri.UnescapeDataString(handler.Request.RequestUri.Query));
        Assert.Contains("pagination[pageSize]=5", Uri.UnescapeDataString(handler.Request.RequestUri.Query));
        Assert.Equal("Bearer sample-token", handler.Request.Headers.GetValues("Authorization").Single());
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public StubHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        public HttpRequestMessage? Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Request = request;

            return Task.FromResult(_response);
        }
    }
}