using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class Formats
{
    [JsonPropertyName("small")]
    public Small? Small { get; set; }

    [JsonPropertyName("medium")]
    public Medium? Medium { get; set; }

    [JsonPropertyName("thumbnail")]
    public Thumbnail? Thumbnail { get; set; }
}