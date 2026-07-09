namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından üretilen yatırımcı briefing sonuçlarını temsil eden değer nesnesi (Value Object).
/// </summary>
public class InvestorBriefResult
{
    public string ExecutiveSummary { get; set; } = string.Empty;
    public string ProblemStatement { get; set; } = string.Empty;
    public string MarketOpportunity { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public string BusinessModel { get; set; } = string.Empty;
    public string CompetitiveAdvantage { get; set; } = string.Empty;
    public string AskAmount { get; set; } = string.Empty;
    public string UseOfFunds { get; set; } = string.Empty;
    public string Traction { get; set; } = string.Empty;
}
