using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait yapay zeka analiz sonuçlarını temsil eden entity.
/// </summary>
public class ProblemAnalysis : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string Summary { get; private set; } = null!;
    public string SuggestedCategory { get; private set; } = null!;
    public string SuggestedTags { get; private set; } = null!;
    public string RiskLevel { get; private set; } = null!;
    public string SolutionTypeSuggestion { get; private set; } = null!;
    public double ConfidenceScore { get; private set; }
    public DateTime AnalyzedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private ProblemAnalysis() { }

    /// <summary>
    /// Yeni bir yapay zeka analizi kaydı oluşturur.
    /// </summary>
    public static ProblemAnalysis Create(
        Guid problemId,
        string summary,
        string suggestedCategory,
        string suggestedTags,
        string riskLevel,
        string solutionTypeSuggestion,
        double confidenceScore)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);
        ArgumentException.ThrowIfNullOrWhiteSpace(suggestedCategory);
        ArgumentException.ThrowIfNullOrWhiteSpace(suggestedTags);
        ArgumentException.ThrowIfNullOrWhiteSpace(riskLevel);
        ArgumentException.ThrowIfNullOrWhiteSpace(solutionTypeSuggestion);

        return new ProblemAnalysis
        {
            ProblemId = problemId,
            Summary = summary,
            SuggestedCategory = suggestedCategory,
            SuggestedTags = suggestedTags,
            RiskLevel = riskLevel,
            SolutionTypeSuggestion = solutionTypeSuggestion,
            ConfidenceScore = confidenceScore,
            AnalyzedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Yapay zeka analizi kaydını günceller.
    /// </summary>
    public void Update(
        string summary,
        string suggestedCategory,
        string suggestedTags,
        string riskLevel,
        string solutionTypeSuggestion,
        double confidenceScore)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(summary);
        ArgumentException.ThrowIfNullOrWhiteSpace(suggestedCategory);
        ArgumentException.ThrowIfNullOrWhiteSpace(suggestedTags);
        ArgumentException.ThrowIfNullOrWhiteSpace(riskLevel);
        ArgumentException.ThrowIfNullOrWhiteSpace(solutionTypeSuggestion);

        Summary = summary;
        SuggestedCategory = suggestedCategory;
        SuggestedTags = suggestedTags;
        RiskLevel = riskLevel;
        SolutionTypeSuggestion = solutionTypeSuggestion;
        ConfidenceScore = confidenceScore;
        AnalyzedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
