using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class SingleType<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}