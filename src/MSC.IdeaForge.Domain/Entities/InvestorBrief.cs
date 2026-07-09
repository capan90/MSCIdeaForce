using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait yapay zeka tarafından üretilen yatırımcı briefing kaydını temsil eden entity.
/// </summary>
public class InvestorBrief : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string ExecutiveSummary { get; private set; } = null!;
    public string ProblemStatement { get; private set; } = string.Empty;
    public string MarketOpportunity { get; private set; } = string.Empty;
    public string Solution { get; private set; } = string.Empty;
    public string BusinessModel { get; private set; } = string.Empty;
    public string CompetitiveAdvantage { get; private set; } = string.Empty;
    public string AskAmount { get; private set; } = string.Empty;
    public string UseOfFunds { get; private set; } = string.Empty;
    public string Traction { get; private set; } = string.Empty;
    public DateTime GeneratedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private InvestorBrief() { }

    /// <summary>
    /// Yeni bir yatırımcı briefing kaydı oluşturur.
    /// </summary>
    public static InvestorBrief Create(
        Guid problemId,
        string executiveSummary,
        string problemStatement,
        string marketOpportunity,
        string solution,
        string businessModel,
        string competitiveAdvantage,
        string askAmount,
        string useOfFunds,
        string traction)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(executiveSummary);

        return new InvestorBrief
        {
            ProblemId = problemId,
            ExecutiveSummary = executiveSummary,
            ProblemStatement = problemStatement ?? string.Empty,
            MarketOpportunity = marketOpportunity ?? string.Empty,
            Solution = solution ?? string.Empty,
            BusinessModel = businessModel ?? string.Empty,
            CompetitiveAdvantage = competitiveAdvantage ?? string.Empty,
            AskAmount = askAmount ?? string.Empty,
            UseOfFunds = useOfFunds ?? string.Empty,
            Traction = traction ?? string.Empty,
            GeneratedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Mevcut yatırımcı briefing kaydını günceller.
    /// </summary>
    public void Update(
        string executiveSummary,
        string problemStatement,
        string marketOpportunity,
        string solution,
        string businessModel,
        string competitiveAdvantage,
        string askAmount,
        string useOfFunds,
        string traction)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(executiveSummary);

        ExecutiveSummary = executiveSummary;
        ProblemStatement = problemStatement ?? string.Empty;
        MarketOpportunity = marketOpportunity ?? string.Empty;
        Solution = solution ?? string.Empty;
        BusinessModel = businessModel ?? string.Empty;
        CompetitiveAdvantage = competitiveAdvantage ?? string.Empty;
        AskAmount = askAmount ?? string.Empty;
        UseOfFunds = useOfFunds ?? string.Empty;
        Traction = traction ?? string.Empty;
        GeneratedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
