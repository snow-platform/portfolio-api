using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class Error
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("details")]
    public object? Details { get; set; }
}