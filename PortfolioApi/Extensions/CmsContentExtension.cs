using PortfolioApi.ExternalServices.CMS;

namespace PortfolioApi.Extensions;

public static class CmsContentExtension
{
    extension(ICmsContent cmsContent)
    {
        public HttpRequestMessage ToHttpRequestMessage()
        {
            var request = new HttpRequestMessage
            {
                Method = cmsContent.HttpMethod,
                RequestUri = new Uri(cmsContent.Endpoint, UriKind.RelativeOrAbsolute),
                Content = cmsContent.HttpContent
            };

            foreach (var header in cmsContent.Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }
    }
}