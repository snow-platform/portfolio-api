using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class TruncatedProfileSocial
{
    public TruncatedProfileSocial(ProfileSocial profileSocial)
    {
        Id = profileSocial.Id;
        Name = profileSocial.Name;
        Link = profileSocial.Link;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
}
