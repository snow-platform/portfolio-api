using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class Blocks
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("__component")]
    public string? Component { get; set; }
}