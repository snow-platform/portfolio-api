using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class ProfileWorkCareerProject
{
    public ProfileWorkCareerProject(CareerProject careerProject)
    {
        Id = careerProject.Id;
        CareerId = careerProject.CareerId;
        Title = careerProject.Title;
        Description = careerProject.Description;
        Significance = careerProject.Significance;

        Imijs = careerProject.Imijs;
        Tecks = careerProject.Tecks;
    }

    public int Id { get; set; }
    public int CareerId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Significance { get; set; }

    public List<ProjectImage>? Imijs { get; set; }
    public List<ProjectTechnology>? Tecks { get; set; }
}
