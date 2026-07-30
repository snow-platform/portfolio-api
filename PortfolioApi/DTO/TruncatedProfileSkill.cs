using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class TruncatedProfileSkill
{
    public TruncatedProfileSkill(ProfileSkill profileSkill)
    {
        Id = profileSkill.Id;
        Category = profileSkill.Category;
        Name = profileSkill.Name;
        Proficiency = profileSkill.Proficiency;
        CreatedAt = profileSkill.CreatedAt;
    }

    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public float Proficiency { get; set; }
    public DateTime? CreatedAt { get; set; }
}