namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından yapılan sektör analizi sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class SectorAnalysisResult
{
    public string TrendSummary { get; set; } = string.Empty;
    public string Opportunities { get; set; } = string.Empty;
    public string Threats { get; set; } = string.Empty;
    public string GrowthPotential { get; set; } = string.Empty;
    public string KeyPlayers { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}
