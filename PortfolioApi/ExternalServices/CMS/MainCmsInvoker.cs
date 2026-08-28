using System.Text.Json;
using PortfolioApi.Entities.CMS;
using PortfolioApi.Extensions;

namespace PortfolioApi.ExternalServices.CMS;

public class MainCmsInvoker : ICmsInvoker
{
    private readonly HttpClient _httpClient;

    public MainCmsInvoker(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("cms_sender");
    }

    public async Task<CmsEither> Invoke<T>(ICmsContent cms)
    {
        try
        {
            var send = await _httpClient.SendAsync(cms.ToHttpRequestMessage());
            var data = await send.Content.ReadAsStringAsync();

            if (send.IsSuccessStatusCode is false)
            {
                var error = JsonSerializer.Deserialize<CollectionTypeError>(data);

                return new CmsEitherError((int)send.StatusCode, error?.Error?.Name ?? "", error?.Error?.Message ?? "");
            }

            if (data.Trim() is null or "")
            {
                return new CmsEitherEmpty((int)send.StatusCode);
            }

            var json = JsonSerializer.Deserialize<T>(data);

            if (json is null)
            {
                return new CmsEitherEmpty((int)send.StatusCode);
            }

            return new CmsEitherOk<T>((int)send.StatusCode, json);
        }
        catch (TimeoutException ex)
        {
            return new CmsEitherError(StatusCodes.Status500InternalServerError, ex.Source?? "", ex.Message);
        }
        catch (JsonException ex)
        {
            return new CmsEitherError(StatusCodes.Status500InternalServerError, ex.Source ?? "", ex.Message);
        }
    }
}