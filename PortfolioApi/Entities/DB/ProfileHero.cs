namespace PortfolioApi.Entities.DB;

public class ProfileHero
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Head { get; set; }
    public string? Text { get; set; }
    public string? Title { get; set; }
    public string? State { get; set; }
    public string? Status { get; set; }
}