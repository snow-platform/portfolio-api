namespace PortfolioApi.Entities.DB;

public class ProfileCms
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Name { get; set; }
    public string? Token { get; set; }
    public string? CreatedAt { get; set; }
}