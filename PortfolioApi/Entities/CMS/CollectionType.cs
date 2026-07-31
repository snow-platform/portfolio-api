using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class CollectionType<T>
{
    [JsonPropertyName("data")]
    public List<T>? Data { get; set; }

    [JsonPropertyName("meta")]
    public Meta? Meta { get; set; }
}