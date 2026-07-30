namespace PortfolioApi.Entities.DB;

public class ProfileCV
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? CV { get; set; }
    public DateTime? CreatedAt { get; set; }
}