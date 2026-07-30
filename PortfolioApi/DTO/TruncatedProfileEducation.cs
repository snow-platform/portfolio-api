using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class TruncatedProfileEducation
{
    public TruncatedProfileEducation(ProfileEducation profileEducation)
    {
        Id = profileEducation.Id;
        Degree = profileEducation.Degree;
        DegreeAbbrev = profileEducation.DegreeAbbrev;
        FieldOfStudy = profileEducation.FieldOfStudy;
        FieldOfStudyAbbrev = profileEducation.FieldOfStudyAbbrev;
        School = profileEducation.School;
        Enrolled = profileEducation.Enrolled;
        Graduated = profileEducation.Graduated;
    }

    public int Id { get; set; }
    public string? Degree { get; set; }
    public string? DegreeAbbrev { get; set; }
    public string? FieldOfStudy { get; set; }
    public string? FieldOfStudyAbbrev { get; set; }
    public string? School { get; set; }
    public DateTime? Enrolled { get; set; }
    public DateTime? Graduated { get; set; }
}
