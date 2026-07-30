namespace PortfolioApi.Entities.DB;

public class ProfileSocial
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
}