using PortfolioApi.Entities.DB;

namespace PortfolioApi.DTO;

public class TruncatedProfileCV
{
    public TruncatedProfileCV(ProfileCV profileCV)
    {
        Id = profileCV.Id;
        CV = profileCV.CV;
        CreatedAt = profileCV.CreatedAt;
    }

    public int Id { get; set; }
    public string? CV { get; set; }
    public DateTime? CreatedAt { get; set; }
}
