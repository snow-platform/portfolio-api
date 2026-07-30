namespace PortfolioApi.Entities.DB;

public class ProfileCertificate
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Name { get; set; }
    public string? Issuer { get; set; }
    public string? Proof { get; set; }
    public DateTime? IssuedAt { get; set; }
}