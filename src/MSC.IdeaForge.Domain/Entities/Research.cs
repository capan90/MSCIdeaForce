using System;

namespace MSC.IdeaForge.Domain.Entities;

/// <summary>
/// Bir probleme ait pazar, rakip, teknoloji ve trend araştırma detaylarını temsil eden entity.
/// </summary>
public class Research : BaseEntity
{
    public Guid ProblemId { get; private set; }
    public string? MarketAnalysis { get; private set; }
    public string? CompetitorSummary { get; private set; }
    public string? TechnologyNotes { get; private set; }
    public string? TrendNotes { get; private set; }
    public string? Sources { get; private set; }
    public DateTime? ResearchedAt { get; private set; }

    // EF Core için gizli varsayılan yapıcı metot
    private Research() { }

    /// <summary>
    /// Yeni bir araştırma kaydı oluşturur.
    /// </summary>
    public static Research Create(
        Guid problemId,
        string? marketAnalysis,
        string? competitorSummary,
        string? technologyNotes,
        string? trendNotes,
        string? sources)
    {
        if (problemId == Guid.Empty)
        {
            throw new ArgumentException("Problem ID boş olamaz.", nameof(problemId));
        }

        return new Research
        {
            ProblemId = problemId,
            MarketAnalysis = marketAnalysis,
            CompetitorSummary = competitorSummary,
            TechnologyNotes = technologyNotes,
            TrendNotes = trendNotes,
            Sources = sources,
            ResearchedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Araştırma verilerini günceller.
    /// </summary>
    public void Update(
        string? marketAnalysis,
        string? competitorSummary,
        string? technologyNotes,
        string? trendNotes,
        string? sources)
    {
        MarketAnalysis = marketAnalysis;
        CompetitorSummary = competitorSummary;
        TechnologyNotes = technologyNotes;
        TrendNotes = trendNotes;
        Sources = sources;
        ResearchedAt = DateTime.UtcNow;
        SetUpdated();
    }
}
