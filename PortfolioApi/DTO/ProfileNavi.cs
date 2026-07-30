using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class ProfileNavi
{
    public ProfileNavi(Profile profile)
    {
        ExternalId = profile.ExternalId;
        Email = profile.Email;
        FirstName = profile.FirstName;
        LastName = profile.LastName;
        CV = profile.CV is not null
            ? new TruncatedProfileCV(profile.CV)
            : null;
        Socials = profile.Socials?.ConvertAll(x => new TruncatedProfileSocial(x));
    }

    public string? ExternalId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public TruncatedProfileCV? CV { get; set; }
    public List<TruncatedProfileSocial>? Socials { get; set; }
}