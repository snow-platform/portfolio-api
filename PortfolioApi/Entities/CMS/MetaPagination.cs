using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class MetaPagination
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}