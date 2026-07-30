namespace PortfolioApi.Entities.DB;

public class ProfileSkill
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public float Proficiency { get; set; }
    public DateTime? CreatedAt { get; set; }
}