namespace PortfolioApi.Entities.DB;

public class ProfileEducation
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string? Degree { get; set; }
    public string? DegreeAbbrev { get; set; }
    public string? FieldOfStudy { get; set; }
    public string? FieldOfStudyAbbrev { get; set; }
    public string? School { get; set; }
    public DateTime? Enrolled { get; set; }
    public DateTime? Graduated { get; set; }
}