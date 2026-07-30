namespace PortfolioApi.Entities.DB;

public class CareerProject
{
    public int Id { get; set; }
    public int CareerId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Significance { get; set; }

    public List<ProjectImage>? Imijs { get; set; }
    public List<ProjectTechnology>? Tecks { get; set; }
}