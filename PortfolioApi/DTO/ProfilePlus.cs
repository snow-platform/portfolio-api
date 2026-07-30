using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class ProfilePlus
{
    public ProfilePlus(Profile profile)
    {
        ExternalId = profile.ExternalId;
        FirstName = profile.FirstName;
        LastName = profile.LastName;
        Email = profile.Email;
        Photo = profile.Photo;
        Title = profile.Title;
        Stack = profile.Stack;
        State = profile.State;
        Summary = profile.Summary;
        CreatedAt = profile.CreatedAt;
        UpdatedAt = profile.UpdatedAt;

        CV = profile.CV is not null
            ? new TruncatedProfileCV(profile.CV)
            : null;
        Career = profile.Career?.ConvertAll(x => new ProfilePlusCareer(x));
        Skills = profile.Skills?.ConvertAll(x => new TruncatedProfileSkill(x));
        Certificates = profile.Certificates?.ConvertAll(x => new TruncatedProfileCertificate(x));
        Educations = profile.Educations?.ConvertAll(x => new TruncatedProfileEducation(x));
        Socials = profile.Socials?.ConvertAll(x => new TruncatedProfileSocial(x));
    }

    public string? ExternalId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Photo { get; set; }
    public string? Title { get; set; }
    public string? Stack { get; set; }
    public string? State { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public TruncatedProfileCV? CV { get; set; }
    public List<ProfilePlusCareer>? Career { get; set; }
    public List<TruncatedProfileSkill>? Skills { get; set; }
    public List<TruncatedProfileCertificate>? Certificates { get; set; }
    public List<TruncatedProfileEducation>? Educations { get; set; }
    public List<TruncatedProfileSocial>? Socials { get; set; }
}