using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class CollectionTypeError
{
    [JsonPropertyName("error")]
    public Error? Error { get; set; }
}