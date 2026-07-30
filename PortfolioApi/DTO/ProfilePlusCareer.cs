using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class ProfilePlusCareer
{
    public ProfilePlusCareer(ProfileCareer profileCareer)
    {
        Id = profileCareer.Id;
        Name = profileCareer.Name;
        Position = profileCareer.Position;
        Joined = profileCareer.Joined;
        Leaved = profileCareer.Leaved;
        Projects = profileCareer.Projects?.ConvertAll(x => new ProfilePlusCareerProject(x));
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Position { get; set; }
    public DateTime? Joined { get; set; }
    public DateTime? Leaved { get; set; }

    public List<ProfilePlusCareerProject>? Projects { get; set; }
}