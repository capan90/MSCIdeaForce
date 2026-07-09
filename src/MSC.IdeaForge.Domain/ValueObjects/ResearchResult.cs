namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından yapılan detaylı araştırma sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class ResearchResult
{
    public string MarketAnalysis { get; set; } = string.Empty;
    public string CompetitorSummary { get; set; } = string.Empty;
    public string TechnologyNotes { get; set; } = string.Empty;
    public string TrendNotes { get; set; } = string.Empty;
    public string Sources { get; set; } = string.Empty;
}
