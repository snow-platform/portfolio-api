using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class Meta
{
    [JsonPropertyName("pagination")]
    public MetaPagination? Pagination { get; set; }
}