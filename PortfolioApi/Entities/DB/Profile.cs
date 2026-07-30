namespace PortfolioApi.Entities.DB;

public class Profile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? ExternalId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Photo { get; set; }
    public string? Title { get; set; }
    public string? Stack { get; set; }
    public string? State { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProfileCV? CV { get; set; }
    public List<ProfileCareer>? Career { get; set; }
    public List<ProfileSkill>? Skills { get; set; }
    public List<ProfileCertificate>? Certificates { get; set; }
    public List<ProfileEducation>? Educations { get; set; }
    public List<ProfileSocial>? Socials { get; set; }
}