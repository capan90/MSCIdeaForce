namespace MSC.IdeaForge.Application.Opportunities.DTOs;

/// <summary>
/// Fırsat değerlendirme verilerini dışarıya aktarmak için kullanılan veri transfer nesnesi (DTO).
/// </summary>
public class OpportunityDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public int ProblemSeverity { get; set; }
    public int MarketSize { get; set; }
    public int FounderFit { get; set; }
    public int RiskScore { get; set; }
    public int TechnicalFeasibility { get; set; }
    public double TotalScore { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
