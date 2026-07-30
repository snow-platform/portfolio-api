using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class ProfileCard
{
    public ProfileCard(Profile profile)
    {
        ExternalId = profile.ExternalId;
        FirstName = profile.FirstName;
        LastName = profile.LastName;
        Email = profile.Email;
        Title = profile.Title;
        Stack = profile.Stack;
        State = profile.State;
        About = profile.About;
        CreatedAt = profile.CreatedAt;
        UpdatedAt = profile.UpdatedAt;

        Skills = profile.Skills?.ConvertAll(x => new TruncatedProfileSkill(x));
    }

    public string? ExternalId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public string? Stack { get; set; }
    public string? State { get; set; }
    public string? About { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<TruncatedProfileSkill>? Skills { get; set; }
}