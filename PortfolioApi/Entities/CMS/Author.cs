using System.Text.Json.Serialization;

namespace PortfolioApi.Entities.CMS;

public class Author
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("documentId")]
    public string? DocumentId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("publishedAt")]
    public string? PublishedAt { get; set; }
}