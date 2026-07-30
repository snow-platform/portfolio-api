using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class TruncatedProfileCertificate
{
    public TruncatedProfileCertificate(ProfileCertificate profileCertificate)
    {
        Id = profileCertificate.Id;
        Name = profileCertificate.Name;
        Issuer = profileCertificate.Issuer;
        Proof = profileCertificate.Proof;
        IssuedAt = profileCertificate.IssuedAt;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Issuer { get; set; }
    public string? Proof { get; set; }
    public DateTime? IssuedAt { get; set; }
}
