namespace PortfolioApi.Entities.DB;

public class ProfileCareer
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Name { get; set; }
    public string? Position { get; set; }
    public DateTime? Joined { get; set; }
    public DateTime? Leaved { get; set; }

    public List<CareerProject>? Projects { get; set; }
}