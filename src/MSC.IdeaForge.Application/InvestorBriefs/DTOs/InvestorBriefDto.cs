using System;

namespace MSC.IdeaForge.Application.InvestorBriefs.DTOs;

/// <summary>
/// Yatırımcı briefing verilerini taşımak için kullanılan DTO.
/// </summary>
public class InvestorBriefDto
{
    public Guid Id { get; set; }
    public Guid ProblemId { get; set; }
    public string ExecutiveSummary { get; set; } = string.Empty;
    public string ProblemStatement { get; set; } = string.Empty;
    public string MarketOpportunity { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public string BusinessModel { get; set; } = string.Empty;
    public string CompetitiveAdvantage { get; set; } = string.Empty;
    public string AskAmount { get; set; } = string.Empty;
    public string UseOfFunds { get; set; } = string.Empty;
    public string Traction { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}
